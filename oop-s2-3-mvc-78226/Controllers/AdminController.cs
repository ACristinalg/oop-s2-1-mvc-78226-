using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IActionResult> CreateEnrolment()
        {
            ViewBag.Students = await _context.StudentProfiles.ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(CourseEnrolment enrolment)
        {
            enrolment.EnrolDate = DateTime.Now;
            _context.CourseEnrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Enrolments");
        }

        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentProfile profile, string password)
        {
            var user = new IdentityUser { UserName = profile.Email, Email = profile.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
                profile.IdentityUserId = user.Id;
                _context.StudentProfiles.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Students");
            }
            return View(profile);
        }

        //Create para Courses

        public async Task<IActionResult> CreateCourse()
        {
            ViewBag.Branches = await _context.Branches.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Courses");
        }

        //Attendance

        public async Task<IActionResult> Attendance()
        {
            return View(await _context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.StudentProfile)
                .Include(a => a.CourseEnrolment)
                    .ThenInclude(e => e.Course)
                .ToListAsync());
        }

        public async Task<IActionResult> CreateAttendance()
        {
            ViewBag.Enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendance(AttendanceRecord record)
        {
            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return RedirectToAction("Attendance");
        }

        // AssignmentResults

        public async Task<IActionResult> AssignmentResults()
        {
            return View(await _context.AssignmentResults
                .Include(r => r.Assignment)
                .Include(r => r.StudentProfile)
                .ToListAsync());
        }

        public async Task<IActionResult> CreateAssignmentResult()
        {
            ViewBag.Assignments = await _context.Assignments.Include(a => a.Course).ToListAsync();
            ViewBag.Students = await _context.StudentProfiles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignmentResult(AssignmentResult result)
        {
            _context.AssignmentResults.Add(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("AssignmentResults");
        }

        //ExamResults
        public async Task<IActionResult> ExamResults()
        {
            return View(await _context.ExamResults
                .Include(r => r.Exam)
                .Include(r => r.StudentProfile)
                .ToListAsync());
        }

        public async Task<IActionResult> CreateExamResult()
        {
            ViewBag.Exams = await _context.Exams.Include(e => e.Course).ToListAsync();
            ViewBag.Students = await _context.StudentProfiles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamResult(ExamResult result)
        {
            _context.ExamResults.Add(result);
            await _context.SaveChangesAsync();
            return RedirectToAction("ExamResults");
        }
    }
}