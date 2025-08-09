using ExamManagementSystem.App_Data;
using ExamManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Room/Index
    public IActionResult Index()
    {
        // Cek role admin dulu (sesuaikan sesuai kebutuhan)
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Admin.ToString())
        {
            return Unauthorized();
        }

        var rooms = _context.Rooms
            .Include(r => r.Invigilator) // load relasi Invigilator (User)
            .ToList();

        var invigilators = _context.Users
            .Where(u => u.Role == UserRole.Invigilator)
            .ToList();

        ViewBag.Invigilators = invigilators;

        return View(rooms);
    }

    // POST: Room/AssignInvigilator
    [HttpPost]
    public IActionResult AssignInvigilator(int roomId, int? invigilatorId)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != UserRole.Admin.ToString())
        {
            return Unauthorized();
        }

        var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
        if (room == null)
        {
            return NotFound();
        }

        if (invigilatorId.HasValue)
        {
            var invigilator = _context.Users.FirstOrDefault(u => u.Id == invigilatorId && u.Role == UserRole.Invigilator);
            if (invigilator == null)
            {
                ModelState.AddModelError("", "Invigilator tidak valid.");
                return RedirectToAction(nameof(Index));
            }
            room.Invigilator = invigilator;
        }
        else
        {
            room.Invigilator = null; // batal assign invigilator
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
