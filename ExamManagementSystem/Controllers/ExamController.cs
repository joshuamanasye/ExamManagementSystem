using ExamManagementSystem.App_Data;
using ExamManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ExamController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExamController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("Role");

        if (role != UserRole.Department.ToString() && role != UserRole.Scheduler.ToString())
        {
            return Unauthorized();
        }

        ViewBag.Role = role;

        var exams = _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Room)
            .ToList();

        return View(exams);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        ViewBag.Courses = _context.Courses.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(int courseId, int durationMinutes)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
        if (course == null)
        {
            ModelState.AddModelError("", "Invalid course selected.");
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        var exam = new Exam
        {
            Course = course,
            DurationMinutes = TimeSpan.FromMinutes(durationMinutes)
        };

        _context.Exams.Add(exam);
        _context.SaveChanges();

        return RedirectToAction("Index", "Exam");
    }

    [HttpGet]
    public IActionResult Scheduling(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Scheduler.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Room)
            .FirstOrDefault(e => e.Id == id);

        if (exam == null)
        {
            return NotFound();
        }

        ViewBag.Rooms = _context.Rooms.ToList();
        return View(exam);
    }

    [HttpPost]
    public IActionResult Scheduling(int id, DateTime date, TimeSpan time, int roomId)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Scheduler.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Room)
            .FirstOrDefault(e => e.Id == id);

        if (exam == null)
        {
            return NotFound();
        }

        var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
        if (room == null || !room.IsAvailable)
        {
            ModelState.AddModelError("", "Ruang yang dipilih tidak tersedia.");
            ViewBag.Rooms = _context.Rooms.ToList();
            return View(exam);
        }

        var scheduledDateTime = date.Date + time;
        exam.Date = scheduledDateTime;
        exam.Room = room;

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
