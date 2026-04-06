using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78226.Data;

namespace oop_s2_3_mvc_78226.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (profile == null) return NotFound();

            return View(profile);
        }

        public async Task<IActionResult> Enrolments()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (profile == null) return NotFound();

            var enrolments = await _context.CourseEnrolments
                .Where(e => e.StudentProfileId == profile.Id)
                .Include(e => e.Course)
                .ToListAsync();

            return View(enrolments);
        }

        public async Task<IActionResult> Grades()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (profile == null) return NotFound();

            var results = await _context.AssignmentResults
                .Where(r => r.StudentProfileId == profile.Id)
                .Include(r => r.Assignment)
                .ToListAsync();

            return View(results);
        }

        public async Task<IActionResult> ExamResults()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (profile == null) return NotFound();

            // Only show released results
            var results = await _context.ExamResults
                .Where(r => r.StudentProfileId == profile.Id && r.Exam.ResultsReleased)
                .Include(r => r.Exam)
                .ToListAsync();

            return View(results);
        }

        public async Task<IActionResult> Attendance()
        {
            var userId = _userManager.GetUserId(User);
            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (profile == null) return NotFound();

            var attendance = await _context.AttendanceRecords
                .Where(a => a.CourseEnrolment.StudentProfileId == profile.Id)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            return View(attendance);
        }
    }
}