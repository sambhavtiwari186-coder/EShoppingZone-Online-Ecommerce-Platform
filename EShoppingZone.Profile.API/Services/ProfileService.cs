using EShoppingZone.Profile.API.Domain;
using EShoppingZone.Profile.API.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EShoppingZone.Profile.API.Services
{
    public class ProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IPasswordHasher<UserProfile> _passwordHasher;

        public ProfileService(IProfileRepository profileRepository, IPasswordHasher<UserProfile> passwordHasher)
        {
            _profileRepository = profileRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserProfile?> FindByMobileNumberAsync(long mobileNumber)
        {
            return await _profileRepository.FindByMobileNumberAsync(mobileNumber);
        }

        public async Task<UserProfile?> FindByEmailIdAsync(string emailId)
        {
            return await _profileRepository.FindByEmailIdAsync(emailId);
        }

        public async Task<UserProfile?> FindByFullNameAsync(string fullName)
        {
            return await _profileRepository.FindByFullNameAsync(fullName);
        }

        public async Task<IEnumerable<UserProfile>> GetAllProfilesAsync()
        {
            return await _profileRepository.GetAllProfilesAsync();
        }

        public async Task<UserProfile?> GetProfileByIdAsync(int id)
        {
            return await _profileRepository.GetProfileByIdAsync(id);
        }

        public async Task AddCustomerAsync(UserProfile profile)
        {
            profile.Role = "CUSTOMER";
            profile.Password = _passwordHasher.HashPassword(profile, profile.Password);
            await _profileRepository.AddProfileAsync(profile);
        }

        public async Task AddMerchantAsync(UserProfile profile)
        {
            profile.Role = "MERCHANT";
            profile.Password = _passwordHasher.HashPassword(profile, profile.Password);
            await _profileRepository.AddProfileAsync(profile);
        }

        public async Task UpdateProfileAsync(UserProfile profile)
        {
             // To simplify logic, we don't handle password changes here.
            await _profileRepository.UpdateProfileAsync(profile);
        }

        public async Task DeleteProfileAsync(int id)
        {
            await _profileRepository.DeleteProfileAsync(id);
        }

        public bool VerifyPassword(UserProfile user, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
