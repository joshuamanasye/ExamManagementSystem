using ExamManagementSystem.App_Data;
using ExamManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExamManagementSystem.Controllers
{
    public class ScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var lecturer = _context.Users.FirstOrDefault(u => u.Id == userId && u.Role == UserRole.Lecturer);
            if (lecturer == null)
            {
                return Forbid();
            }

            // Ambil semua exam di course yang diajar dosen
            var exams = _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.Lecturer != null && e.Course.Lecturer.Id == lecturer.Id)
                .ToList();

            var examIds = exams.Select(e => e.Id).ToList();

            // Ambil attendances mahasiswa yang hadir untuk exam tersebut
            var attendances = _context.Attendances
                .Include(a => a.Student)
                .Where(a => examIds.Contains(a.ExamId) && a.IsPresent)
                .ToList();

            // Ambil semua grades yang sudah diinput
            var grades = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Exam)
                .Where(g => examIds.Contains(g.Exam.Id))
                .ToList();

            ViewBag.Attendances = attendances;
            ViewBag.Grades = grades;

            return View(exams);
        }

        [HttpPost]
        public IActionResult SubmitScore(int examId, int studentId, int score)
        {
            var existingGrade = _context.Grades
                .FirstOrDefault(g => g.Exam.Id == examId && g.Student.Id == studentId);

            if (existingGrade != null)
            {
                existingGrade.Score = score;
                _context.Grades.Update(existingGrade);
            }
            else
            {
                var exam = _context.Exams.Find(examId);
                var student = _context.Students.Find(studentId);

                if (exam == null || student == null)
                {
                    return NotFound();
                }

                var newGrade = new Grade
                {
                    Exam = exam,
                    Student = student,
                    Score = score
                };

                _context.Grades.Add(newGrade);
            }

            _context.SaveChanges();

            TempData["Message"] = "Score saved successfully.";
            return RedirectToAction("Index");
        }
    }
}
