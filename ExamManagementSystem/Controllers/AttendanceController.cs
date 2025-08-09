using ExamManagementSystem.App_Data;
using ExamManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AttendanceController : Controller
{
    private readonly ApplicationDbContext _context;

    public AttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Attend(int examId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return Unauthorized();
        }

        var existing = _context.Attendances
            .FirstOrDefault(a => a.ExamId == examId && a.StudentId == userId);

        if (existing != null)
        {
            TempData["Message"] = "You have already attended this exam.";
            return RedirectToAction("Index", "Exam");
        }

        var attendance = new Attendance(examId, userId.Value, true);

        _context.Attendances.Add(attendance);
        _context.SaveChanges();

        TempData["Message"] = "Attendance recorded.";
        return RedirectToAction("Index", "Exam");
    }
}
