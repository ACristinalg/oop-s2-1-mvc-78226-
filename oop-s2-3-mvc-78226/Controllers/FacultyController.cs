using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78226.Data;

namespace oop_s2_3_mvc_78226.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FacultyController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(f => f.IdentityUserId == userId);

            if (facultyProfile == null) return NotFound();

            var courses = await _context.FacultyCourseAssignments
                .Where(f => f.FacultyProfileId == facultyProfile.Id)
                .Include(f => f.Course)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Students(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(f => f.IdentityUserId == userId);

            if (facultyProfile == null) return NotFound();

            // Verify faculty teaches this course
            var teaches = await _context.FacultyCourseAssignments
                .AnyAsync(f => f.FacultyProfileId == facultyProfile.Id && f.CourseId == courseId);

            if (!teaches) return Forbid();

            var students = await _context.CourseEnrolments
                .Where(e => e.CourseId == courseId)
                .Include(e => e.StudentProfile)
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> Gradebook(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(f => f.IdentityUserId == userId);

            if (facultyProfile == null) return NotFound();

            var teaches = await _context.FacultyCourseAssignments
                .AnyAsync(f => f.FacultyProfileId == facultyProfile.Id && f.CourseId == courseId);

            if (!teaches) return Forbid();

            var results = await _context.AssignmentResults
                .Where(r => r.Assignment.CourseId == courseId)
                .Include(r => r.Assignment)
                .Include(r => r.StudentProfile)
                .ToListAsync();

            return View(results);
        }
    }
}