namespace oop_s2_3_mvc_78226.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int CourseEnrolmentId { get; set; }
        public CourseEnrolment CourseEnrolment { get; set; } = null!;
        public int WeekNumber { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; }
    }
}
