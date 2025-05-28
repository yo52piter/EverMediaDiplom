// ServiceController.cs
using EverMediaDiplom.Data;
using EverMediaDiplom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EverMediaDiplom.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var servicesQuery = _context.Services.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                servicesQuery = servicesQuery.Where(s =>
                    s.Name.Contains(searchQuery) ||
                    s.Description.Contains(searchQuery));
            }

            var model = new ServiceViewModel
            {
                Services = await servicesQuery.ToListAsync(),
                SearchQuery = searchQuery
            };

            return View(model);
        }
    }
}