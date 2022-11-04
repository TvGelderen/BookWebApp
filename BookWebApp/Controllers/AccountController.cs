using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Login(string returnUrl)
        {
            return string.IsNullOrEmpty(returnUrl) ? Redirect("/") : Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        [HttpGet("/Denied")]
        public IActionResult Denied()
        {
            TempData["denied"] = "You do not have permission to perform this action.";

            return Redirect("/");
        }
    }
}
