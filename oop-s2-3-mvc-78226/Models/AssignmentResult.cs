namespace oop_s2_3_mvc_78226.Models
{
    public class AssignmentResult
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
