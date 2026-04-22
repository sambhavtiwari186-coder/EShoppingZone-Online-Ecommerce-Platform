using EShoppingZone.Wallet.API.Domain;
using EShoppingZone.Wallet.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Wallet.API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("addNew")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> AddNewWallet([FromBody] EWallet wallet)
        {
            var result = await _walletService.AddWalletAsync(wallet);
            return Ok(result);
        }

        [HttpPost("addMoney/{id}/{amt}")]
        [Authorize(Roles = "CUSTOMER")]
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

        [HttpPost("payMoney/{id}/{amt}/{orderId}")]
        [Authorize(Roles = "CUSTOMER")]
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
                return BadRequest(ex.Message); // Sufficient balance check
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
}
