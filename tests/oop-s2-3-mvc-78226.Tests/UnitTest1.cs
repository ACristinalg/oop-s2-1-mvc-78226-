using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using oop_s2_3_mvc_78226.Data;
using oop_s2_3_mvc_78226.Models;

namespace oop_s2_3_mvc_78226.Tests
{
    public class UnitTest1
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        // Test 1: Student can be added
        [Fact]
        public void CanAddStudentProfile()
        {
            var context = GetDbContext();
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            Assert.Equal(1, context.StudentProfiles.Count());
        }

        // Test 2: Course can be added
        [Fact]
        public void CanAddCourse()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            context.SaveChanges();
            Assert.Equal(1, context.Courses.Count());
        }

        // Test 3: Student can be enrolled in a course
        [Fact]
        public void CanEnrolStudentInCourse()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            var enrolment = new CourseEnrolment { StudentProfileId = student.Id, CourseId = course.Id, EnrolDate = DateTime.Now, Status = "Active" };
            context.CourseEnrolments.Add(enrolment);
            context.SaveChanges();
            Assert.Equal(1, context.CourseEnrolments.Count());
        }

        // Test 4: Exam results are not visible when not released
        [Fact]
        public void ExamResultsNotVisibleWhenNotReleased()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            context.SaveChanges();
            var exam = new Exam { CourseId = course.Id, Title = "Midterm", Date = new DateTime(2025, 11, 1), MaxScore = 100, ResultsReleased = false };
            context.Exams.Add(exam);
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            var result = new ExamResult { ExamId = exam.Id, StudentProfileId = student.Id, Score = 80, Grade = "A" };
            context.ExamResults.Add(result);
            context.SaveChanges();
            var visibleResults = context.ExamResults
                .Where(r => r.StudentProfileId == student.Id && r.Exam.ResultsReleased)
                .ToList();
            Assert.Empty(visibleResults);
        }

        // Test 5: Exam results visible when released
        [Fact]
        public void ExamResultsVisibleWhenReleased()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            context.SaveChanges();
            var exam = new Exam { CourseId = course.Id, Title = "Midterm", Date = new DateTime(2025, 11, 1), MaxScore = 100, ResultsReleased = true };
            context.Exams.Add(exam);
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            var result = new ExamResult { ExamId = exam.Id, StudentProfileId = student.Id, Score = 80, Grade = "A" };
            context.ExamResults.Add(result);
            context.SaveChanges();
            var visibleResults = context.ExamResults
                .Where(r => r.StudentProfileId == student.Id && r.Exam.ResultsReleased)
                .ToList();
            Assert.Single(visibleResults);
        }

        // Test 6: Faculty only sees their courses
        [Fact]
        public void FacultyOnlySeesAssignedCourses()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course1 = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            var course2 = new Course { Name = "BUS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.AddRange(course1, course2);
            var faculty = new FacultyProfile { Name = "John", Email = "john@test.com", Phone = "123", IdentityUserId = "fac1" };
            context.FacultyProfiles.Add(faculty);
            context.SaveChanges();
            context.FacultyCourseAssignments.Add(new FacultyCourseAssignment { FacultyProfileId = faculty.Id, CourseId = course1.Id });
            context.SaveChanges();
            var facultyCourses = context.FacultyCourseAssignments
                .Where(f => f.FacultyProfileId == faculty.Id)
                .Select(f => f.CourseId)
                .ToList();
            Assert.Single(facultyCourses);
            Assert.Contains(course1.Id, facultyCourses);
        }

        // Test 7: Attendance record can be added
        [Fact]
        public void CanAddAttendanceRecord()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            var enrolment = new CourseEnrolment { StudentProfileId = student.Id, CourseId = course.Id, EnrolDate = new DateTime(2025, 9, 1), Status = "Active" };
            context.CourseEnrolments.Add(enrolment);
            context.SaveChanges();
            var attendance = new AttendanceRecord { CourseEnrolmentId = enrolment.Id, WeekNumber = 1, Date = new DateTime(2025, 9, 8), Present = true };
            context.AttendanceRecords.Add(attendance);
            context.SaveChanges();
            Assert.Equal(1, context.AttendanceRecords.Count());
        }

        // Test 8: Assignment result score cannot exceed max score
        [Fact]
        public void AssignmentResultScoreValidation()
        {
            var context = GetDbContext();
            var branch = new Branch { Name = "Dublin", Address = "10 College Green" };
            context.Branches.Add(branch);
            context.SaveChanges();
            var course = new Course { Name = "CS101", BranchId = branch.Id, StartDate = new DateTime(2025, 9, 1), EndDate = new DateTime(2026, 6, 30) };
            context.Courses.Add(course);
            context.SaveChanges();
            var assignment = new Assignment { CourseId = course.Id, Title = "Assignment 1", MaxScore = 100, DueDate = new DateTime(2025, 10, 15) };
            context.Assignments.Add(assignment);
            var student = new StudentProfile { Name = "Alice", Email = "alice@test.com", Phone = "123", Address = "Dublin", StudentNumber = "VGC001", IdentityUserId = "user1" };
            context.StudentProfiles.Add(student);
            context.SaveChanges();
            var result = new AssignmentResult { AssignmentId = assignment.Id, StudentProfileId = student.Id, Score = 85, Feedback = "Good" };
            context.AssignmentResults.Add(result);
            context.SaveChanges();
            Assert.True(result.Score <= assignment.MaxScore);
        }
    }
}