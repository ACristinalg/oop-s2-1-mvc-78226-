using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace oop_s2_3_mvc_78226.Models
{

    public class AssignmentResult
    {
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [ValidateNever]
        public Assignment Assignment { get; set; } = null!;

        [Required]
        public int StudentProfileId { get; set; }

        [ValidateNever]
        public StudentProfile StudentProfile { get; set; } = null!;

        [Required]
        [Range(0, 100)]
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
