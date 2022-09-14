namespace ChoreTracker.Models
{
    public class AssignedChoreView
    {
        public int ChoreId { get; set; }
        public string ChoreName { get; set; } = null!;
        public bool IsComplete { get; set; }
    }
}
