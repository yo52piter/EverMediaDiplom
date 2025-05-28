using EverMediaDiplom.Data;
using EverMediaDiplom.Models;
using EverMediaDiplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EverMediaDiplom.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                {
                    _logger.LogWarning("Invalid user id in claims");
                    return RedirectToAction("Login", "Account");
                }

                var user = await _context.Users
                    .Include(u => u.ClientSessions)
                        .ThenInclude(ps => ps.Service)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning($"User with id {userId} not found");
                    return RedirectToAction("Login", "Account");
                }

                var model = new ProfileViewModel
                {
                    User = user,
                    NewName = user.Name,
                    NewEmail = user.Email
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Profile/Index");
                return View("Error", new ErrorViewModel { Message = "Ошибка при загрузке профиля" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    model.User = await _context.Users.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                    return View("Index", model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading user data for invalid model");
                    return View("Error", new ErrorViewModel { Message = "Ошибка при загрузке данных пользователя" });
                }
            }

            try
            {
                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
                {
                    _logger.LogWarning("Invalid user id in claims");
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with id {userId} not found for update");
                    return NotFound();
                }

                // Проверка уникальности email
                if (await _context.Users.AnyAsync(u => u.Email == model.NewEmail && u.UserId != userId))
                {
                    ModelState.AddModelError("NewEmail", "Этот email уже используется другим пользователем");
                    model.User = user;
                    return View("Index", model);
                }

                user.Name = model.NewName;
                user.Email = model.NewEmail;

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    user.Password = model.NewPassword;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Профиль успешно обновлен!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error in Profile/UpdateProfile");
                ModelState.AddModelError("", "Ошибка при обновлении профиля в базе данных");
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Profile/UpdateProfile");
                return View("Error", new ErrorViewModel { Message = "Произошла непредвиденная ошибка при обновлении профиля" });
            }
        }
    }
}