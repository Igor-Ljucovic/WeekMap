using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserTask
    {
        public int UserTaskID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? Location { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
