using EverMediaDiplom.Data;
using EverMediaDiplom.Models;
using EverMediaDiplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EverMediaDiplom.Controllers
{
    [Authorize]
    public class PhotoSessionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PhotoSessionController> _logger;

        public PhotoSessionController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<PhotoSessionController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int serviceId)
        {
            try
            {
                var service = await _context.Services
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

                if (service == null)
                {
                    _logger.LogWarning($"Service with id {serviceId} not found");
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var model = new PhotoSessionViewModel
                {
                    ServiceId = serviceId,
                    ServiceName = service.Name,
                    ServiceDescription = service.Description,
                    ServicePrice = service.Price,
                    ServiceDurationHours = service.DurationHours,
                    SessionDate = DateTime.Now.AddDays(1).Date.AddHours(10),
                    ClientName = user.Name ?? User.Identity?.Name,
                    ClientPhone = user.PhoneNumber
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading create form for service {serviceId}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhotoSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var service = await _context.Services
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.ServiceId == model.ServiceId);

                    if (service != null)
                    {
                        model.ServiceName = service.Name;
                        model.ServiceDescription = service.Description;
                        model.ServicePrice = service.Price;
                        model.ServiceDurationHours = service.DurationHours;
                    }

                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reloading service data for invalid model");
                    ModelState.AddModelError("", "Ошибка при загрузке данных услуги");
                    return View(model);
                }
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Проверка доступности даты
                var endTime = model.SessionDate.AddHours(model.ServiceDurationHours);

                bool isDateAvailable = !await _context.PhotoSessions
                    .Include(ps => ps.Service)
                    .AnyAsync(ps => ps.Status != "Canceled" &&
                        ((ps.SessionDate <= model.SessionDate &&
                          ps.SessionDate.AddHours(ps.Service.DurationHours) > model.SessionDate) ||
                         (ps.SessionDate < endTime &&
                          ps.SessionDate.AddHours(ps.Service.DurationHours) >= endTime) ||
                         (model.SessionDate <= ps.SessionDate &&
                          endTime >= ps.SessionDate.AddHours(ps.Service.DurationHours))));

                if (!isDateAvailable)
                {
                    ModelState.AddModelError("SessionDate", "Выбранное время уже занято");
                    var service = await _context.Services
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.ServiceId == model.ServiceId);

                    if (service != null)
                    {
                        model.ServiceName = service.Name;
                        model.ServiceDescription = service.Description;
                        model.ServicePrice = service.Price;
                        model.ServiceDurationHours = service.DurationHours;
                    }

                    return View(model);
                }

                var photoSession = new PhotoSession
                {
                    ServiceId = model.ServiceId,
                    ClientId = user.Id,
                    SessionDate = model.SessionDate,
                    SpecialRequests = model.SpecialRequests,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                await _context.PhotoSessions.AddAsync(photoSession);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Фотосессия успешно забронирована!";
                return RedirectToAction("Index", "Profile");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating photo session");
                ModelState.AddModelError("", "Ошибка при сохранении данных. Пожалуйста, попробуйте позже.");

                var service = await _context.Services
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ServiceId == model.ServiceId);

                if (service != null)
                {
                    model.ServiceName = service.Name;
                    model.ServiceDescription = service.Description;
                    model.ServicePrice = service.Price;
                    model.ServiceDurationHours = service.DurationHours;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating photo session");
                return View("Error");
            }
        }
    }
}