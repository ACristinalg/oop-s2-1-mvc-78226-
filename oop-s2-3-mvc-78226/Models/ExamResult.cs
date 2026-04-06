using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace oop_s2_3_mvc_78226.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        [Required]
        public int ExamId { get; set; }

        [ValidateNever]
        public Exam Exam { get; set; } = null!;

        [Required]
        public int StudentProfileId { get; set; }

        [ValidateNever]
        public StudentProfile StudentProfile { get; set; } = null!;

        [Required]
        [Range(0, 100)]
        public int Score { get; set; }

        public string Grade { get; set; } = string.Empty;
    }
}
