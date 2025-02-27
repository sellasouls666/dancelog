namespace dancelog.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public DateTime DateTime { get; set; }
    }
}
