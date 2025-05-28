using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EverMediaDiplom.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // "User", "Photographer", "Admin"

        [JsonIgnore]
        public ICollection<PhotoSession> ClientSessions { get; set; } = new List<PhotoSession>();

        [JsonIgnore]
        public ICollection<PhotoSession> PhotographerSessions { get; set; } = new List<PhotoSession>();

        [JsonIgnore]
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

    }
}