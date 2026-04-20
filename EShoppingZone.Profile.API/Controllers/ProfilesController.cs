using EShoppingZone.Profile.API.Domain;
using EShoppingZone.Profile.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingZone.Profile.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] UserProfile profile)
        {
            await _profileService.AddCustomerAsync(profile);
            return Ok(profile);
        }

        [HttpPost("addMerchant")]
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

        [HttpGet("byId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _profileService.GetProfileByIdAsync(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("byPhone/{no}")]
        [Authorize]
        public async Task<IActionResult> GetProfileByPhone(long no)
        {
            var profile = await _profileService.FindByMobileNumberAsync(no);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("byName/{name}")]
        [Authorize]
        public async Task<IActionResult> GetProfileByName(string name)
        {
            var profile = await _profileService.FindByFullNameAsync(name);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("update")]
        [Authorize]
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
    }
}
