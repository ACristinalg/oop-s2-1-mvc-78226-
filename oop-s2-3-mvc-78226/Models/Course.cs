using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace oop_s2_3_mvc_78226.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int BranchId { get; set; }

        [ValidateNever]
        public Branch Branch { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ValidateNever]
        public ICollection<CourseEnrolment> Enrolments { get; set; } = new List<CourseEnrolment>();
        [ValidateNever]
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        [ValidateNever]
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        [ValidateNever]
        public ICollection<FacultyCourseAssignment> FacultyAssignments { get; set; } = new List<FacultyCourseAssignment>();
    }
}
