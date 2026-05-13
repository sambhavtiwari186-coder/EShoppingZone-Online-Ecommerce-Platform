using System.ComponentModel.DataAnnotations;

namespace EShoppingZone.Profile.API.Domain
{
    public class DeliveryAgent
    {
        [Key]
        public int AgentId { get; set; }
        public int ProfileId { get; set; } // Link to UserProfile
        public string FullName { get; set; } = string.Empty;
        public string VehicleNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string ServiceArea { get; set; } = string.Empty;
        public string Status { get; set; } = "Active"; // Active, Inactive, OnDelivery
    }
}
