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
        var userId = HttpContext.Session.GetInt32("UserId");
        ViewBag.UserId = userId;

        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString() && role != UserRole.Scheduler.ToString() && role != UserRole.QuestionMaker.ToString() && role != UserRole.Student.ToString())
        {
            return Unauthorized();
        }
        ViewBag.Role = role;

        var exams = _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Room)
            .Include(e => e.QuestionMaker)
            .Include(e => e.ExamFile)
            .ToList();

        if (role == UserRole.Student.ToString() && userId.HasValue)
        {
            var attendances = _context.Attendances
                .Where(a => a.StudentId == userId.Value)
                .ToList();
            ViewBag.Attendances = attendances;
        }

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
        if (room != null) room.IsAvailable = true;

        if (room == null || !room.IsAvailable)
        {
            ModelState.AddModelError("", "Ruang yang dipilih tidak tersedia.");
            ViewBag.Rooms = _context.Rooms.ToList();
            return View(exam);
        }

        var scheduledDateTime = date.Date + time;
        exam.Date = scheduledDateTime;
        exam.Room = room;

        room.IsAvailable = false;

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Exam/SetQuestionMaker/{id}
    [HttpGet]
    public IActionResult SetQuestionMaker(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.Course)
            .Include(e => e.QuestionMaker)
            .FirstOrDefault(e => e.Id == id);

        if (exam == null)
        {
            return NotFound();
        }

        var questionMakers = _context.Users
            .Where(u => u.Role == UserRole.QuestionMaker)
            .ToList();

        ViewBag.QuestionMakers = questionMakers;

        return View(exam);
    }

    // POST: Exam/SetQuestionMaker/{id}
    [HttpPost]
    public IActionResult SetQuestionMaker(int id, int questionMakerId)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .FirstOrDefault(e => e.Id == id);

        if (exam == null)
        {
            return NotFound();
        }

        var questionMaker = _context.Users
            .FirstOrDefault(u => u.Id == questionMakerId && u.Role == UserRole.QuestionMaker);

        if (questionMaker == null)
        {
            ModelState.AddModelError("", "Invalid Question Maker selected.");
            ViewBag.QuestionMakers = _context.Users.Where(u => u.Role == UserRole.QuestionMaker).ToList();
            return View(exam);
        }

        exam.QuestionMaker = questionMaker;
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult UploadExamFile(int examId)
    {
        var role = HttpContext.Session.GetString("Role");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (role != UserRole.QuestionMaker.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.QuestionMaker)
            .Include(e => e.Course)
            .FirstOrDefault(e => e.Id == examId);


        if (exam == null || exam.QuestionMaker == null || exam.QuestionMaker.Id != userId)
        {
            return Unauthorized();
        }

        ViewBag.ExamId = examId;
        ViewBag.CourseName = exam.Course?.Name;

        return View();
    }

    [HttpPost]
    public IActionResult UploadExamFile(int examId, IFormFile file)
    {
        var role = HttpContext.Session.GetString("Role");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (role != UserRole.QuestionMaker.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.QuestionMaker)
            .FirstOrDefault(e => e.Id == examId);

        if (exam == null || exam.QuestionMaker == null || exam.QuestionMaker.Id != userId)
        {
            return Unauthorized();
        }

        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Please select a file to upload.");
            ViewBag.ExamId = examId;
            ViewBag.CourseName = exam.Course?.Name;
            return View();
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var examFile = new ExamFile
        {
            FilePath = "/uploads/" + uniqueFileName,
            Status = ApprovalStatus.Pending,
            QuestionMaker = _context.Users.Find(userId)
        };

        _context.ExamFiles.Add(examFile);
        _context.SaveChanges();

        exam.ExamFile = examFile;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Exam/ReviewExamFile/{id}
    [HttpGet]
    public IActionResult ReviewExamFile(int id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.ExamFile)
            .ThenInclude(ef => ef.QuestionMaker)
            .Include(e => e.Course)
            .FirstOrDefault(e => e.Id == id);

        if (exam == null)
            return NotFound();

        return View(exam);
    }

    // POST: Exam/ReviewExamFile/{id}
    [HttpPost]
    public IActionResult ReviewExamFile(int id, ApprovalStatus approvalStatus)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Department.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.ExamFile)
            .FirstOrDefault(e => e.Id == id);

        if (exam == null || exam.ExamFile == null)
            return NotFound();

        exam.ExamFile.Status = approvalStatus;

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult PrintQueue()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Printer.ToString())
        {
            return Unauthorized();
        }

        var approvedExams = _context.Exams
            .Include(e => e.Course)
                .ThenInclude(c => c.StudentCourses)
            .Include(e => e.ExamFile)
            .Where(e => e.ExamFile != null && e.ExamFile.Status == ApprovalStatus.Approved)
            .ToList();

        return View(approvedExams);
    }

    public IActionResult DownloadExamFile(int examId)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Printer.ToString())
        {
            return Unauthorized();
        }

        var exam = _context.Exams
            .Include(e => e.ExamFile)
            .FirstOrDefault(e => e.Id == examId);

        if (exam == null || exam.ExamFile == null || exam.ExamFile.Status != ApprovalStatus.Approved)
            return NotFound();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", exam.ExamFile.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = "application/pdf";
        var fileName = Path.GetFileName(filePath);

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, contentType, fileName);
    }

}
