namespace WebApp.Models
{
    public class RepetitiveTask : UserTask
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Category { get; set; }
        public bool Weekly { get; set; }
        public bool Daily { get; set; }
        public string? Color { get; set; }

        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public bool OnSunday { get; set; }
    }
}
