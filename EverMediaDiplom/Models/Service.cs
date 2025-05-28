// Service.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EverMediaDiplom.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationHours { get; set; }

        [JsonIgnore]
        public ICollection<PhotoSession> PhotoSessions { get; set; } = new List<PhotoSession>();
    }
}