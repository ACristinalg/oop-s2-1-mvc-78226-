using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace oop_s2_3_mvc_78226.Models
{

    public class AssignmentResult
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }

        [ValidateNever]
        public Assignment Assignment { get; set; } = null!;
        public int StudentProfileId { get; set; }

        [ValidateNever]
        public StudentProfile StudentProfile { get; set; } = null!;
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
