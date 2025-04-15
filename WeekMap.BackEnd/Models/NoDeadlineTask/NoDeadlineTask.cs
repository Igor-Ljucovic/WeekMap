namespace WebApp.Models
{
    public class NoDeadlineTask : UserTask
    {
        public string? Priority { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public bool IsCompleted { get; set; }
    }
}
