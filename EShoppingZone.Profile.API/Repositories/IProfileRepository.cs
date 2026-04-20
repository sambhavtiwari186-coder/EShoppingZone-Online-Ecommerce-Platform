using EShoppingZone.Profile.API.Domain;
using System.Threading.Tasks;

namespace EShoppingZone.Profile.API.Repositories
{
    public interface IProfileRepository
    {
        Task<UserProfile?> FindByMobileNumberAsync(long mobileNumber);
        Task<UserProfile?> FindByEmailIdAsync(string emailId);
        Task<UserProfile?> FindByFullNameAsync(string fullName);
        Task<IEnumerable<UserProfile>> GetAllProfilesAsync();
        Task<UserProfile?> GetProfileByIdAsync(int id);
        Task AddProfileAsync(UserProfile profile);
        Task UpdateProfileAsync(UserProfile profile);
        Task DeleteProfileAsync(int id);
    }
}
