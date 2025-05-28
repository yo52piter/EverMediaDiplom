using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EverMediaDiplom.Models
{
    public class PhotoSession
    {
        public int PhotoSessionId { get; set; }
        public DateTime SessionDate { get; set; }
        public string Status { get; set; } // "Pending", "Confirmed", "Completed", "Canceled"
        public string SpecialRequests { get; set; }

        // Foreign keys
        public int ClientId { get; set; }
        public int? PhotographerId { get; set; }
        public int ServiceId { get; set; }

        // Navigation properties
        public User Client { get; set; }
        public User Photographer { get; set; }
        public Service Service { get; set; }

        [JsonIgnore]
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

        [JsonIgnore]
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}