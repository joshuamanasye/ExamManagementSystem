using Microsoft.AspNetCore.Mvc;

namespace ExamManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.Username = username;
            return View();
        }
    }
}
