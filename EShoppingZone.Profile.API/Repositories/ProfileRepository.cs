using EShoppingZone.Profile.API.Data;
using EShoppingZone.Profile.API.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShoppingZone.Profile.API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileDbContext _context;

        public ProfileRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> FindByMobileNumberAsync(long mobileNumber)
        {
            return await _context.Profiles.Include(p => p.Addresses).FirstOrDefaultAsync(p => p.MobileNumber == mobileNumber);
        }

        public async Task<UserProfile?> FindByEmailIdAsync(string emailId)
        {
            return await _context.Profiles.Include(p => p.Addresses).FirstOrDefaultAsync(p => p.EmailId == emailId);
        }

        public async Task<UserProfile?> FindByFullNameAsync(string fullName)
        {
            return await _context.Profiles.Include(p => p.Addresses).FirstOrDefaultAsync(p => p.FullName == fullName);
        }

        public async Task<IEnumerable<UserProfile>> GetAllProfilesAsync()
        {
            return await _context.Profiles.Include(p => p.Addresses).ToListAsync();
        }

        public async Task<UserProfile?> GetProfileByIdAsync(int id)
        {
            return await _context.Profiles.Include(p => p.Addresses).FirstOrDefaultAsync(p => p.ProfileId == id);
        }

        public async Task AddProfileAsync(UserProfile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(UserProfile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddDeliveryAgentAsync(DeliveryAgent agent)
        {
            await _context.DeliveryAgents.AddAsync(agent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeliveryAgent>> GetAllDeliveryAgentsAsync()
        {
            return await _context.DeliveryAgents.ToListAsync();
        }

        public async Task DeleteDeliveryAgentAsync(int id)
        {
            var agent = await _context.DeliveryAgents.FindAsync(id);
            if (agent != null)
            {
                _context.DeliveryAgents.Remove(agent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
