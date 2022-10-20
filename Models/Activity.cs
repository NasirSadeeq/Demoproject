namespace Demoproject.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string Priorty { get; set; }
        public string AssignBy { get; set; }
        public string AssignTo { get; set; }
    }
}
