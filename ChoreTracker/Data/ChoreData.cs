using ChoreTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChoreTracker.Data
{
    public class ChoreData : IChoreData
    {
        private ApplicationDbContext _context;

        public ChoreData(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Chore>> GetChores()
        {
            List<Chore> chores = await (from chore in _context.Chores
                                        orderby chore.Name
                                        select chore).ToListAsync();
            return chores;
        }

        public async Task<List<CompletedChore>> GetCompletedChores()
        {
            List<CompletedChore> completedChores = await (from chore in _context.CompletedChores
                                                          orderby chore.DateComplete
                                                          select chore).ToListAsync();

            return completedChores;
        }

        public async Task<List<AssignedChore>> GetAssignedChores()
        {
            List<AssignedChore> assignedChores = await (from assignedChore in _context.AssignedChores
                                                        select assignedChore).ToListAsync();
            return assignedChores;
        }

        public async Task InsertChore(Chore chore)
        {
            await _context.Chores.AddAsync(chore);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveChore(int choreId)
        {
            Chore choreToRemove = await _context.Chores.FindAsync(choreId);
            _context.Chores.Remove(choreToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task AddCompletedChores(List<CompletedChore> completedChores)
        {
            foreach (CompletedChore chore in completedChores)
            {
                await _context.CompletedChores.AddAsync(chore);
            }

            await _context.SaveChangesAsync();
        }

        public async Task MarkChoreComplete(int choreId)
        {
            List<AssignedChore> assignedChores = await (from assignedChore in _context.AssignedChores
                                                        where assignedChore.ChoreId == choreId
                                                        select assignedChore).ToListAsync();

            foreach (AssignedChore chore in assignedChores)
            {
                if (chore.ChoreId == choreId)
                {
                    chore.IsComplete = true;
                    _context.AssignedChores.Update(chore);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task MarkChoreIncomplete(int choreId)
        {
            List<AssignedChore> assignedChores = await (from assignedChore in _context.AssignedChores
                                                        where assignedChore.ChoreId == choreId
                                                        select assignedChore).ToListAsync();

            foreach (AssignedChore chore in assignedChores)
            {
                if (chore.ChoreId == choreId)
                {
                    chore.IsComplete = false;
                    _context.AssignedChores.Update(chore);
                }
            }
            await _context.SaveChangesAsync();
        }

        // TODO: Complete assigned chore retrieval 
        public async Task<List<AssignedChoreView>> GetTodaysChores(string userName)
        {
            DateTime today = DateTime.Today;
            string dayOfWeek = today.DayOfWeek.ToString();
            
            // Load chores from database
            List<Chore> chores = new List<Chore>();
            chores = await GetChores();

            // Load user's assigned chores from database
            List<AssignedChore> assignedChores = new List<AssignedChore>();
            assignedChores = await GetAssignedChores();

            // Join assigned chores with chores
            var joinedChores = from chore in chores
                               join assignedChore in assignedChores on chore.Id equals assignedChore.Id
                               select new
                               {
                                   chore.Id,
                                   chore.Name,
                                   chore.Monday,
                                   chore.Tuesday,
                                   chore.Wednesday,
                                   chore.Thursday,
                                   chore.Friday,
                                   chore.Saturday,
                                   chore.Sunday,
                                   assignedChore.UserName,
                                   assignedChore.IsComplete
                               };

            List<AssignedChoreView> todaysChores = new List<AssignedChoreView>();

            if (chores != null)
            {
                foreach (var chore in joinedChores)
                {
                    if (chore.UserName == userName)
                    {
                        if (dayOfWeek == "Monday" && chore.Monday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }

                        if (dayOfWeek == "Tuesday" && chore.Tuesday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }
                        if (dayOfWeek == "Wednesday" && chore.Wednesday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }
                        if (dayOfWeek == "Thursday" && chore.Thursday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }
                        if (dayOfWeek == "Friday" && chore.Friday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }
                        if (dayOfWeek == "Saturday" && chore.Saturday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        }
                        if (dayOfWeek == "Sunday" && chore.Sunday == true)
                        {
                            AssignedChoreView newChore = new AssignedChoreView();
                            newChore.ChoreId = chore.Id;
                            newChore.ChoreName = chore.Name;
                            newChore.IsComplete = chore.IsComplete;
                            todaysChores.Add(newChore);
                        } 
                    }
                }
            }

            return todaysChores;
        }

        public async Task<List<ModifyChoreView>> GetChoresToModify()
        {
            // Load chores from database
            List<Chore> chores = new List<Chore>();
            chores = await GetChores();

            // Load user's assigned chores from database
            List<AssignedChore> assignedChores = new List<AssignedChore>();
            assignedChores = await GetAssignedChores();
            
            var joinedChores = (from chore in chores
                               join assignedChore in assignedChores
                                    on chore.Id equals assignedChore.Id
                                    into mc
                               select new ModifyChoreView
                               {
                                   ChoreId = chore.Id,
                                   ChoreName = chore.Name,
                                   Value = chore.Value,
                                   Monday = chore.Monday,
                                   Tuesday = chore.Tuesday,
                                   Wednesday = chore.Wednesday,
                                   Thursday = chore.Thursday,
                                   Friday = chore.Friday,
                                   Saturday = chore.Saturday,
                                   Sunday = chore.Sunday,
                                   Users = new List<User>()
                               }).ToList();

            foreach (ModifyChoreView chore in joinedChores)
            {
                foreach (AssignedChore assignedChore in assignedChores)
                {
                    if (assignedChore.ChoreId == chore.ChoreId)
                    {
                        chore.Users.Add(new User(assignedChore.UserId, assignedChore.UserName));
                    }
                }
                
                
            }

            return joinedChores;
        }

        public async Task<Chore> GetChore(int choreId)
        {
            Chore choreToModify = await _context.Chores.FindAsync(choreId);

            return choreToModify;
        }

        public async Task ModifyChore(Chore updatedChore)
        {
            Chore choreToRemove = await _context.Chores.FindAsync(updatedChore.Id);
            _context.Chores.Remove(choreToRemove);
            await _context.Chores.AddAsync(updatedChore);
            await _context.SaveChangesAsync();
        }
    }
}
