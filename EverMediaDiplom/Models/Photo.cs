using System;
using System.Text.Json.Serialization;

namespace EverMediaDiplom.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; }

        // Foreign keys
        public int PhotoSessionId { get; set; }
        public int UserId { get; set; } // Добавлено

        // Navigation properties
        [JsonIgnore]
        public PhotoSession PhotoSession { get; set; }

        [JsonIgnore]
        public User User { get; set; } // Добавлено
    }
}