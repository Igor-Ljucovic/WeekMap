namespace WebApp.Models
{
    public class CalendarTask : UserTask
    {
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool RemindMeNotification { get; set; }
        public bool RemindMeEmail { get; set; }
        public int? HoursBeforeNotification { get; set; }
        public int? HoursBeforeEmail { get; set; }
        public bool IsCompleted { get; set; }
        public string? Color { get; set; }
    }
}
