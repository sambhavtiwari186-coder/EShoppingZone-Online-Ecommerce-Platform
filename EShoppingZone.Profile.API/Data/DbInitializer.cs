using EShoppingZone.Profile.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EShoppingZone.Profile.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProfileDbContext context, IPasswordHasher<UserProfile> passwordHasher)
        {
            context.Database.EnsureCreated();

            if (context.Profiles.Any())
            {
                return;
            }

            var profiles = new UserProfile[]
            {
                new UserProfile { 
                    FullName = "Sambhav Tiwari", 
                    EmailId = "sambhav@gmail.com", 
                    MobileNumber = 1234567890, 
                    About = "E-commerce enthusiast and developer.", 
                    DateOfBirth = new DateTime(1995, 1, 1), 
                    Gender = "Male", 
                    Role = "CUSTOMER", 
                    Password = "12345678", // In a real app, this should be hashed
                    Addresses = new List<Address>
                    {
                        new Address { HouseNumber = 101, StreetName = "Main St", ColonyName = "Downtown", City = "Mumbai", State = "Maharashtra", Pincode = 400001 }
                    }
                },
                new UserProfile { 
                    FullName = "Chirag", 
                    EmailId = "chirag@gmail.com", 
                    MobileNumber = 1234567891, 
                    About = "E-commerce Merchant.", 
                    DateOfBirth = new DateTime(1990, 10, 10), 
                    Gender = "Male", 
                    Role = "MERCHANT", 
                    Password = "12345678",
                    Addresses = new List<Address>()
                },
                new UserProfile { 
                    FullName = "Amit Tiwari", 
                    EmailId = "amit@gmail.com", 
                    MobileNumber = 9876543211, 
                    About = "System Administrator.", 
                    DateOfBirth = new DateTime(1985, 5, 20), 
                    Gender = "Male", 
                    Role = "ADMIN", 
                    Password = "12345678",
                    Addresses = new List<Address>()
                }
            };

            foreach (var p in profiles)
            {
                p.Password = passwordHasher.HashPassword(p, p.Password);
                context.Profiles.Add(p);
            }
            context.SaveChanges();
        }
    }
}
