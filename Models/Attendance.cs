namespace dancelog.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public Student Student { get; set; }
        public string Status { get; set; }
    }
}
