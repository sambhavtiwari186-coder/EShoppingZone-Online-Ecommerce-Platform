using EShoppingZone.Wallet.API.Domain;
using EShoppingZone.Wallet.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace EShoppingZone.Wallet.API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IConfiguration _configuration;

        public WalletController(IWalletService walletService, IConfiguration configuration)
        {
            _walletService = walletService;
            _configuration = configuration;
        }

        // ── Razorpay: Create Order ───────────────────────────────────────────────

        /// <summary>
        /// Creates a Razorpay order for the given amount (in INR).
        /// The frontend opens the Razorpay checkout with the returned orderId.
        /// </summary>
        [HttpPost("razorpay/createOrder")]
        [Authorize(Roles = "CUSTOMER")]
        public IActionResult CreateRazorpayOrder([FromBody] RazorpayOrderRequest req)
        {
            try
            {
                var key    = _configuration["Razorpay:Key"]!;
                var secret = _configuration["Razorpay:Secret"]!;

                var client = new RazorpayClient(key, secret);

                // Razorpay requires amount in paise (1 INR = 100 paise)
                var options = new Dictionary<string, object>
                {
                    { "amount",   (int)(req.Amount * 100) },
                    { "currency", "INR" },
                    { "receipt",  $"wallet_topup_{req.WalletId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}" },
                    { "payment_capture", 1 }
                };

                Order order = client.Order.Create(options);

                return Ok(new
                {
                    orderId  = order["id"].ToString(),
                    amount   = req.Amount,
                    currency = "INR",
                    key      = key
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create Razorpay order.", detail = ex.Message });
            }
        }

        // ── Razorpay: Verify Payment & Credit Wallet ─────────────────────────────

        /// <summary>
        /// After the Razorpay checkout succeeds, the frontend posts the payment
        /// signature here for server-side verification. On success the wallet is credited.
        /// </summary>
        [HttpPost("razorpay/verifyAndCredit")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> VerifyAndCredit([FromBody] RazorpayVerifyRequest req)
        {
            try
            {
                var secret = _configuration["Razorpay:Secret"]!;

                // HMAC-SHA256 verification
                var payload  = $"{req.RazorpayOrderId}|{req.RazorpayPaymentId}";
                var keyBytes = Encoding.UTF8.GetBytes(secret);
                using var hmac = new HMACSHA256(keyBytes);
                var hash = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload))).ToLower();

                if (hash != req.RazorpaySignature)
                    return BadRequest(new { message = "Payment signature verification failed." });

                // Credit the wallet
                await _walletService.AddMoneyAsync(req.WalletId, req.Amount);

                return Ok(new { message = $"₹{req.Amount} added to your wallet successfully!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Payment verified but wallet credit failed.", detail = ex.Message });
            }
        }

        // ── Standard wallet endpoints ─────────────────────────────────────────────

        [HttpPost("addNew")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> AddNewWallet([FromBody] EWallet wallet)
        {
            var result = await _walletService.AddWalletAsync(wallet);
            return Ok(result);
        }

        [HttpPost("addMoney/{id}/{amt}")]
        public async Task<IActionResult> AddMoney(int id, decimal amt)
        {
            try
            {
                await _walletService.AddMoneyAsync(id, amt);
                return Ok(new { message = "Money added successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("withdrawMoney/{id}/{amt}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> WithdrawMoney(int id, decimal amt)
        {
            try
            {
                await _walletService.WithdrawMoneyAsync(id, amt);
                return Ok(new { message = "Money withdrawn successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("payMoney/{id}/{amt}/{orderId}")]
        public async Task<IActionResult> PayMoney(int id, decimal amt, int orderId)
        {
            try
            {
                var result = await _walletService.PayMoneyAsync(id, amt, orderId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllWallets()
        {
            var result = await _walletService.GetAllWalletsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetWalletById(int id)
        {
            var result = await _walletService.GetWalletByIdAsync(id);
            if (result == null) return NotFound("Wallet not found");
            return Ok(result);
        }

        [HttpGet("statements/{id}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetStatements(int id)
        {
            var result = await _walletService.GetStatementsByWalletIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            await _walletService.DeleteWalletAsync(id);
            return Ok(new { message = "Wallet deleted successfully" });
        }
    }

    // ── Request DTOs ─────────────────────────────────────────────────────────────

    public class RazorpayOrderRequest
    {
        public int     WalletId { get; set; }
        public decimal Amount   { get; set; }
    }

    public class RazorpayVerifyRequest
    {
        public int     WalletId           { get; set; }
        public decimal Amount             { get; set; }
        public string  RazorpayOrderId    { get; set; } = string.Empty;
        public string  RazorpayPaymentId  { get; set; } = string.Empty;
        public string  RazorpaySignature  { get; set; } = string.Empty;
    }
}
