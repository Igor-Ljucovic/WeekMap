namespace WebApp.Models
{
    public class TaskCategory
    {
        public int TaskCategoryID { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
