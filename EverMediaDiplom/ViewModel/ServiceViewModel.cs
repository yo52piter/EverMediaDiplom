// ServiceViewModel.cs
using System.Collections.Generic;
using EverMediaDiplom.Models;

namespace EverMediaDiplom.ViewModels
{
    public class ServiceViewModel
    {
        public IEnumerable<Service> Services { get; set; } = new List<Service>();
        public string SearchQuery { get; set; }
    }
}