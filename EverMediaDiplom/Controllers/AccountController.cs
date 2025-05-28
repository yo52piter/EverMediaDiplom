using EverMediaDiplom.Data;
using EverMediaDiplom.Models;
using EverMediaDiplom.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EverMediaDiplom.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View(new LoginViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Account/Login GET");
                return View("Error", new ErrorViewModel { Message = "Произошла ошибка при загрузке страницы входа" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный email или пароль");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = model.RememberMe });

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error in Account/Login POST");
                ModelState.AddModelError("", "Ошибка базы данных при попытке входа");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Account/Login POST");
                return View("Error", new ErrorViewModel { Message = "Произошла непредвиденная ошибка при входе" });
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View(new RegisterViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Account/Register GET");
                return View("Error", new ErrorViewModel { Message = "Произошла ошибка при загрузке страницы регистрации" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    return View(model);
                }

                var user = new User
                {
                    Name = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Client"
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error in Account/Register POST");
                ModelState.AddModelError("", "Ошибка при сохранении пользователя в базу данных");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Account/Register POST");
                return View("Error", new ErrorViewModel { Message = "Произошла непредвиденная ошибка при регистрации" });
            }
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Account/Logout");
                return View("Error", new ErrorViewModel { Message = "Произошла ошибка при выходе из системы" });
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}