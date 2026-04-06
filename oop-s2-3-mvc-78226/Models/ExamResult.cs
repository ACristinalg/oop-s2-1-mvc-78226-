namespace oop_s2_3_mvc_78226.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;
        public int Score { get; set; }
        public string Grade { get; set; } = string.Empty;
    }
}
