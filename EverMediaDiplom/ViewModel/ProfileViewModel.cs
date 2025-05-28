using System.Collections.Generic;
using EverMediaDiplom.Models;

namespace EverMediaDiplom.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public string NewName { get; set; }
        public string NewEmail { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}