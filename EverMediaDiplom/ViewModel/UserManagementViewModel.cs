using System.Collections.Generic;
using EverMediaDiplom.Models;

namespace EverMediaDiplom.ViewModels
{
    public class UserManagementViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public string SearchQuery { get; set; }
        public string RoleFilter { get; set; }
    }
}