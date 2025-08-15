using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User/Login - Giriş formu
        public IActionResult Login()
        {
            ViewData["Title"] = "Giriş Yap";
            return View();
        }

        // POST: User/Login - Giriş işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            try
            {
                var result = await _userService.LoginAsync(loginDto);
                
                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.User.Username),
                    new Claim(ClaimTypes.Email, result.User.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Remember me functionality
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                // Sign in with cookie authentication
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["Success"] = "Başarıyla giriş yapıldı.";
                
                // Ana sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(loginDto);
            }
        }

        // GET: User/Register - Kayıt formu
        public IActionResult Register()
        {
            ViewData["Title"] = "Kayıt Ol";
            return View();
        }

        // POST: User/Register - Kayıt işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            try
            {
                var result = await _userService.RegisterAsync(registerDto);
                
                TempData["Success"] = "Hesap başarıyla oluşturuldu. Şimdi giriş yapabilirsiniz.";
                return RedirectToAction(nameof(Login));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(registerDto);
            }
        }

        // GET: User/Profile - Kullanıcı profili
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Profil görüntülemek için giriş yapmalısınız.";
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bilgileri bulunamadı.";
                    return RedirectToAction(nameof(Index), "Home");
                }

                ViewData["Title"] = "Profil";
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Profil bilgileri yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        // POST: User/Logout - Çıkış işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            TempData["Success"] = "Başarıyla çıkış yapıldı.";
            return RedirectToAction("Index", "Home");
        }
    }
}
