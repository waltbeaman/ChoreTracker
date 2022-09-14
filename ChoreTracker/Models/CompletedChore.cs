namespace ChoreTracker.Models
{
    public class CompletedChore
    {
        public int Id { get; set; }
        public int ChoreId { get; set; }
        public Chore Chore { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime DateComplete { get; set; }
        public bool IsComplete { get; set; }
    }
}
