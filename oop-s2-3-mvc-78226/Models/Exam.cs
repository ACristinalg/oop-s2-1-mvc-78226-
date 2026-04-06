namespace oop_s2_3_mvc_78226.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int MaxScore { get; set; }
        public bool ResultsReleased { get; set; } = false;
        public ICollection<ExamResult> Results { get; set; } = new List<ExamResult>();
    }
}
