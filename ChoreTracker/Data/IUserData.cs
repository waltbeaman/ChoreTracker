using ChoreTracker.Models;

namespace ChoreTracker.Data
{
    public interface IUserData
    {
        Task<List<User>> GetUsers();
        Task AssignChoreToUser(AssignedChore assignedChore);
    }
}