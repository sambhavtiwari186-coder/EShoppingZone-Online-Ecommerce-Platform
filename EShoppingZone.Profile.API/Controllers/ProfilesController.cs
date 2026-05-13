using EShoppingZone.Profile.API.Domain;
using EShoppingZone.Profile.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Profile.API.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("addCustomer")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCustomer([FromBody] UserProfile profile)
        {
            await _profileService.AddCustomerAsync(profile);
            return Ok(profile);
        }

        [HttpPost("addMerchant")]
        [AllowAnonymous]
        public async Task<IActionResult> AddMerchant([FromBody] UserProfile profile)
        {
            await _profileService.AddMerchantAsync(profile);
            return Ok(profile);
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _profileService.GetAllProfilesAsync();
            return Ok(profiles);
        }

        [HttpGet("count")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUserCount()
        {
            var profiles = await _profileService.GetAllProfilesAsync();
            return Ok(profiles.Count());
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _profileService.GetProfileByIdAsync(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("byPhone/{no}")]
        public async Task<IActionResult> GetProfileByPhone(long no)
        {
            var profile = await _profileService.FindByMobileNumberAsync(no);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetProfileByName(string name)
        {
            var profile = await _profileService.FindByFullNameAsync(name);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfile profile)
        {
            await _profileService.UpdateProfileAsync(profile);
            return Ok(profile);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            await _profileService.DeleteProfileAsync(id);
            return Ok();
        }

        [HttpPut("suspend/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SuspendUser(int id)
        {
            await _profileService.SuspendUserAsync(id);
            return Ok(new { Message = "User suspended successfully" });
        }

        [HttpPut("reactivate/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ReactivateUser(int id)
        {
            await _profileService.ReactivateUserAsync(id);
            return Ok(new { Message = "User reactivated successfully" });
        }

        [HttpPost("deliveryAgents")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RegisterDeliveryAgent([FromBody] DeliveryAgent agent)
        {
            await _profileService.RegisterDeliveryAgentAsync(agent);
            return Ok(agent);
        }

        [HttpGet("deliveryAgents")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllDeliveryAgents()
        {
            var agents = await _profileService.GetAllDeliveryAgentsAsync();
            return Ok(agents);
        }

        [HttpDelete("deliveryAgents/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteDeliveryAgent(int id)
        {
            await _profileService.DeleteDeliveryAgentAsync(id);
            return Ok();
        }
    }
}
