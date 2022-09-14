namespace ChoreTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<AssignedChore> AssignedChores { get; set; } = null!;
        public ICollection<CompletedChore> CompletedChores { get; set; } = null!;

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
