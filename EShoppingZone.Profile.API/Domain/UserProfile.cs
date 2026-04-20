using System;
using System.Collections.Generic;

namespace EShoppingZone.Profile.API.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public int HouseNumber { get; set; }
        public string StreetName { get; set; } = string.Empty;
        public string ColonyName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int Pincode { get; set; }
        public int ProfileId { get; set; }
        
        public UserProfile Profile { get; set; } = null!;
    }

    public class UserProfile
    {
        public int ProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public long MobileNumber { get; set; }
        public string About { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
