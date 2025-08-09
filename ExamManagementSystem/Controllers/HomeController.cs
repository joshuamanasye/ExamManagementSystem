using ExamManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            System.Diagnostics.Debug.WriteLine($"Session Role: '{role}'");

            if (role == UserRole.Department.ToString() || role == UserRole.Scheduler.ToString() || role == UserRole.QuestionMaker.ToString())
            {
                return RedirectToAction("Index", "Exam");
            }

            if (role == UserRole.Printer.ToString())
            {
                return RedirectToAction("PrintQueue", "Exam");
            }

            if (role == UserRole.Admin.ToString())
            {
                return RedirectToAction("Index", "Room");
            }

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
