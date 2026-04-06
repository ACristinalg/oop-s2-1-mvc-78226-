using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78226.Data;
using oop_s2_3_mvc_78226.Models;

namespace oop_s2_3_mvc_78226.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Branches
        public async Task<IActionResult> Branches()
        {
            return View(await _context.Branches.ToListAsync());
        }

        // Courses
        public async Task<IActionResult> Courses()
        {
            return View(await _context.Courses.Include(c => c.Branch).ToListAsync());
        }

        // Students
        public async Task<IActionResult> Students()
        {
            return View(await _context.StudentProfiles.ToListAsync());
        }

        // Faculty
        public async Task<IActionResult> Faculty()
        {
            return View(await _context.FacultyProfiles.ToListAsync());
        }

        // Enrolments
        public async Task<IActionResult> Enrolments()
        {
            return View(await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .ToListAsync());
        }

        // Release exam results
        public async Task<IActionResult> ReleaseResults(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                exam.ResultsReleased = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Exams");
        }

        public async Task<IActionResult> Exams()
        {
            return View(await _context.Exams.Include(e => e.Course).ToListAsync());
        }
    }
}