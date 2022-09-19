using ChoreTracker.Models;

namespace ChoreTracker.Data
{
    public interface IChoreData
    {
        Task<List<Chore>> GetChores();
        Task<List<AssignedChoreView>> GetTodaysChores(string userName);
        Task<List<CompletedChoreView>> GetCompletedChores();
        Task<List<MonthlyCompletedChoreView>> GetCompletedChoresByMonth();
        Task<List<AssignedChore>> GetAssignedChores();
        Task<List<ModifyChoreView>> GetChoresToModify();
        Task InsertChore(Chore chore);
        Task RemoveChore(int choreId);
        Task MarkChoreComplete(int choreId);
        Task MarkChoreIncomplete(int choreId);
        Task AddCompletedChores(List<CompletedChore> completedChores);
        Task<Chore> GetChore(int choreId);
        Task ModifyChore(Chore updatedChore);
        Task AddDummyCompletedChoreData();
        Task<bool> CheckIfDoneForDay(string userName);
    }
}