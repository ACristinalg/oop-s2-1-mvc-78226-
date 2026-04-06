namespace oop_s2_3_mvc_78226.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public int MaxScore { get; set; }
        public DateTime DueDate { get; set; }
        public ICollection<AssignmentResult> Results { get; set; } = new List<AssignmentResult>();
    }
}
