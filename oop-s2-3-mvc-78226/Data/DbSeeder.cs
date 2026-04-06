using Microsoft.AspNetCore.Identity;
using oop_s2_3_mvc_78226.Models;

namespace oop_s2_3_mvc_78226.Data
{
    public static class DbSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            // Roles
            if (!roleManager.RoleExistsAsync("Admin").Result)
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            if (!roleManager.RoleExistsAsync("Faculty").Result)
                roleManager.CreateAsync(new IdentityRole("Faculty")).Wait();
            if (!roleManager.RoleExistsAsync("Student").Result)
                roleManager.CreateAsync(new IdentityRole("Student")).Wait();

            // Admin
            if (userManager.FindByEmailAsync("admin@vgc.ie").Result == null)
            {
                var admin = new IdentityUser { UserName = "admin@vgc.ie", Email = "admin@vgc.ie", EmailConfirmed = true };
                userManager.CreateAsync(admin, "Admin123!").Wait();
                userManager.AddToRoleAsync(admin, "Admin").Wait();
            }

            // Faculty
            if (userManager.FindByEmailAsync("faculty@vgc.ie").Result == null)
            {
                var faculty = new IdentityUser { UserName = "faculty@vgc.ie", Email = "faculty@vgc.ie", EmailConfirmed = true };
                userManager.CreateAsync(faculty, "Faculty123!").Wait();
                userManager.AddToRoleAsync(faculty, "Faculty").Wait();
            }

            if (!context.FacultyProfiles.Any())
            {
                var facultyUser = userManager.FindByEmailAsync("faculty@vgc.ie").Result;
                context.FacultyProfiles.Add(new FacultyProfile
                {
                    IdentityUserId = facultyUser!.Id,
                    Name = "John Smith",
                    Email = "faculty@vgc.ie",
                    Phone = "0851234567"
                });
                context.SaveChanges();
            }

            // Students
            if (userManager.FindByEmailAsync("student1@vgc.ie").Result == null)
            {
                var s1 = new IdentityUser { UserName = "student1@vgc.ie", Email = "student1@vgc.ie", EmailConfirmed = true };
                userManager.CreateAsync(s1, "Student123!").Wait();
                userManager.AddToRoleAsync(s1, "Student").Wait();
            }

            if (userManager.FindByEmailAsync("student2@vgc.ie").Result == null)
            {
                var s2 = new IdentityUser { UserName = "student2@vgc.ie", Email = "student2@vgc.ie", EmailConfirmed = true };
                userManager.CreateAsync(s2, "Student123!").Wait();
                userManager.AddToRoleAsync(s2, "Student").Wait();
            }

            if (!context.StudentProfiles.Any())
            {
                var s1User = userManager.FindByEmailAsync("student1@vgc.ie").Result;
                var s2User = userManager.FindByEmailAsync("student2@vgc.ie").Result;

                context.StudentProfiles.AddRange(
                    new StudentProfile { IdentityUserId = s1User!.Id, Name = "Alice Murphy", Email = "student1@vgc.ie", Phone = "0861234567", Address = "12 Main St, Dublin", StudentNumber = "VGC001" },
                    new StudentProfile { IdentityUserId = s2User!.Id, Name = "Brian Kelly", Email = "student2@vgc.ie", Phone = "0871234567", Address = "45 Oak Ave, Cork", StudentNumber = "VGC002" }
                );
                context.SaveChanges();
            }

            // Branches
            if (!context.Branches.Any())
            {
                context.Branches.AddRange(
                    new Branch { Name = "Dublin", Address = "10 College Green, Dublin 2" },
                    new Branch { Name = "Cork", Address = "5 Patrick St, Cork" },
                    new Branch { Name = "Galway", Address = "8 Shop St, Galway" }
                );
                context.SaveChanges();
            }

            // Courses
            if (!context.Courses.Any())
            {
                var dublin = context.Branches.First(b => b.Name == "Dublin");
                var cork = context.Branches.First(b => b.Name == "Cork");
                context.Courses.AddRange(
                    new Course { Name = "Computer Science", BranchId = dublin.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) },
                    new Course { Name = "Business Studies", BranchId = cork.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) }
                );
                context.SaveChanges();
            }

            // Faculty course assignment
            if (!context.FacultyCourseAssignments.Any())
            {
                var facultyProfile = context.FacultyProfiles.First();
                var course = context.Courses.First();
                context.FacultyCourseAssignments.Add(new FacultyCourseAssignment
                {
                    FacultyProfileId = facultyProfile.Id,
                    CourseId = course.Id
                });
                context.SaveChanges();
            }

            // Enrolments
            if (!context.CourseEnrolments.Any())
            {
                var student = context.StudentProfiles.First();
                var course = context.Courses.First();
                context.CourseEnrolments.Add(new CourseEnrolment
                {
                    StudentProfileId = student.Id,
                    CourseId = course.Id,
                    EnrolDate = new DateTime(2025, 9, 1),
                    Status = "Active"
                });
                context.SaveChanges();
            }

            // Assignments
            if (!context.Assignments.Any())
            {
                var course = context.Courses.First();
                context.Assignments.Add(new Assignment
                {
                    CourseId = course.Id,
                    Title = "Assignment 1",
                    MaxScore = 100,
                    DueDate = new DateTime(2025, 10, 15)
                });
                context.SaveChanges();
            }

            // Exams
            if (!context.Exams.Any())
            {
                var course = context.Courses.First();
                context.Exams.Add(new Exam
                {
                    CourseId = course.Id,
                    Title = "Midterm Exam",
                    Date = new DateTime(2025, 11, 1),
                    MaxScore = 100,
                    ResultsReleased = false
                });
                context.SaveChanges();
            }
        }
    }
}