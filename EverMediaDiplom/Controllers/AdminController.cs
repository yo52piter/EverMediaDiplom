using EverMediaDiplom.Data;
using EverMediaDiplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Users(string searchQuery, string roleFilter)
    {
        var users = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            users = users.Where(u =>
                u.Name.Contains(searchQuery) ||
                u.Email.Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(roleFilter))
        {
            users = users.Where(u => u.Role == roleFilter);
        }

        var model = new UserManagementViewModel
        {
            Users = await users.ToListAsync(),
            SearchQuery = searchQuery,
            RoleFilter = roleFilter
        };

        return View(model);
    }
}