namespace ChoreTracker.Models
{
    public class AssignedChoreView
    {
        public int Id { get; set; }
        public int ChoreId { get; set; }
        public string ChoreName { get; set; } = null!;
        public bool IsComplete { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string UserName { get; set; } = null!;

    }
}
