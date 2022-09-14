using System.ComponentModel.DataAnnotations.Schema;

namespace ChoreTracker.Models
{
    public class AssignedChore
    {
        public int Id { get; set; }
        public int ChoreId { get; set; }
        public Chore Chore { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public bool IsComplete { get; set; }
    }
}
