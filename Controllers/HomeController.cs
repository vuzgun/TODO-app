using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TodoApi.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewData["Title"] = "Ana Sayfa";
            ViewData["User"] = HttpContext.User.Identity.Name;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Hakkında";
            return View();
        }
    }
}
