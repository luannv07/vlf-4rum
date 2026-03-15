using Microsoft.AspNetCore.Mvc;
using vlf_4rum.Models.ViewModels;
using VlfForum.Controllers;
using VlfForum.Services.Interfaces;

namespace vlf_4rum.Controllers
{
    public class AuthController : BaseController
    {
        protected readonly IAuthService _authService;

        public AuthController(ICurrentUserService currentUser, IAuthService authService) : base(currentUser)
        {
            _authService = authService;
        }

        // ── Register ──────────────────────────────────────────
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _authService.RegisterAsync(
                model.Username, model.Email, model.Password);

            if (!success)
            {
                ViewData["Toast.Variant"] = "danger";
                ViewData["Toast.Message"] = error;
                return View(model);
            }

            TempData["Toast.Variant"] = "success";
            TempData["Toast.Message"] = "Đăng ký thành công! Hãy đăng nhập.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var (success, error) = await _authService.LoginAsync(
                model.Username, model.Password, model.RememberMe);

            if (!success)
            {
                ViewData["Toast.Variant"] = "danger";
                ViewData["Toast.Message"] = error;
                return View(model);
            }

            TempData["Toast.Variant"] = "success";
            TempData["Toast.Message"] = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Auth/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            Console.WriteLine($"[Logout] User: {username} đang đăng xuất...");

            try
            {
                await _authService.LogoutAsync();
                Console.WriteLine($"[Logout] SignOut thành công cho: {username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logout] Lỗi: {ex.Message}");
                throw;
            }

            TempData["Toast.Variant"] = "success";
            TempData["Toast.Message"] = "Đã đăng xuất thành công.";
            Console.WriteLine($"[Logout] Redirect về Login");
            return RedirectToAction("Login");
        }
    }
}