namespace ChoreTracker.Models
{
    public class MonthlyCompletedChoreView
    {
        public string UserName { get; set; }
        public int Month { get; set; }
        public int ChoreCount { get; set; }
        public decimal MoneyEarned { get; set; }
    }
}
