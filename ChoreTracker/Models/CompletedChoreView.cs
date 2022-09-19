namespace ChoreTracker.Models
{
    public class CompletedChoreView
    {
        public int ChoreId { get; set; }
        public string ChoreName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime DateComplete { get; set; }
        public decimal Value { get; set; }
    }
}
