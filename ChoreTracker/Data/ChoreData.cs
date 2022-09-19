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

        public async Task<List<CompletedChoreView>> GetCompletedChores()
        {
            List<CompletedChoreView> completedChores = await (
                from completedChore in _context.CompletedChores
                join chore in _context.Chores on completedChore.ChoreId equals chore.Id
                orderby completedChore.DateComplete descending
                select new CompletedChoreView
                {
                    ChoreId = completedChore.ChoreId,
                    ChoreName = chore.Name,
                    UserName = completedChore.UserName,
                    DateComplete = completedChore.DateComplete,
                    Value = chore.Value
                }).ToListAsync();

            return completedChores;

        }

        public async Task<List<MonthlyCompletedChoreView>> GetCompletedChoresByMonth()
        {
            //List<CompletedChoreView> completedChores = await GetCompletedChores();
            //List<MonthlyCompletedChoreView> monthlyChores = new();

            //var monthlyChores = completedChores
            //                    .Select(x =>
            //                    new
            //                    {
            //                        UserName = x.UserName,
            //                        Month = x.DateComplete.Month.ToString(),
            //                        Value = x.Value,
            //                    })
            //                    .GroupBy(s => new { s.UserName, s.Month })
            //                    .Select(g => new
            //                    {
            //                        UserName = g.Key.UserName,
            //                        Month = g.Key.Month,
            //                        MoneyEarned = g.Sum(x => x.Value)
            //                    }).ToList();


            List<MonthlyCompletedChoreView> monthlyCompletedChores = await (
                from completedChore in _context.CompletedChores
                join chore in _context.Chores on completedChore.ChoreId equals chore.Id
                select new
                {
                    UserName = completedChore.UserName,
                    Month = completedChore.DateComplete.Month,
                    Value = chore.Value
                } into x
                group x by new
                {
                    x.UserName,
                    x.Month
                } into s
                orderby s.Key.Month descending
                select new MonthlyCompletedChoreView
                {
                    UserName = s.Key.UserName,
                    Month = s.Key.Month,
                    ChoreCount = s.Count(),
                    MoneyEarned = s.Sum(x => x.Value)
                }).ToListAsync();

            return monthlyCompletedChores;

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

            var  choreToUpdate = await _context.AssignedChores.FindAsync(choreId);
            if (choreToUpdate != null) choreToUpdate.IsComplete = true;
            
            await _context.SaveChangesAsync();


            #region OldMethod
            //List<AssignedChore> assignedChores = await (from assignedChore in _context.AssignedChores
            //                                            where assignedChore.ChoreId == choreId
            //                                            select assignedChore).ToListAsync();

            //foreach (AssignedChore chore in assignedChores)
            //{
            //    if (chore.ChoreId == choreId)
            //    {
            //        chore.IsComplete = true;
            //        _context.AssignedChores.Update(chore);
            //    }
            //}

            //await _context.SaveChangesAsync(); 
            #endregion
        }

        public async Task MarkChoreIncomplete(int choreId)
        {
            var choreToUpdate = await _context.AssignedChores.FindAsync(choreId);
            if (choreToUpdate != null) choreToUpdate.IsComplete = false;

            await _context.SaveChangesAsync();

            #region OldMethod
            //List<AssignedChore> assignedChores = await (from assignedChore in _context.AssignedChores
            //                                            where assignedChore.ChoreId == choreId
            //                                            select assignedChore).ToListAsync();

            //foreach (AssignedChore chore in assignedChores)
            //{
            //    if (chore.ChoreId == choreId)
            //    {
            //        chore.IsComplete = false;
            //        _context.AssignedChores.Update(chore);
            //    }
            //}
            //await _context.SaveChangesAsync(); 
            #endregion
        }


        public async Task<bool> CheckIfDoneForDay(string userName)
        {
            var checkIfDone = await (from chore in _context.CompletedChores
                                     where chore.DateComplete == DateTime.Today && chore.UserName == userName
                                     select chore).FirstOrDefaultAsync();

            return checkIfDone == null;
        }

        // TODO: Complete assigned chore retrieval 
        public async Task<List<AssignedChoreView>> GetTodaysChores(string userName)
        {
            List<AssignedChoreView> joinedChores = await (from chore in _context.Chores
                               join assignedChore in _context.AssignedChores on chore.Id equals assignedChore.ChoreId
                               select new AssignedChoreView
                               {
                                   Id = assignedChore.Id,
                                   ChoreId = chore.Id,
                                   ChoreName = chore.Name,
                                   IsComplete = assignedChore.IsComplete,
                                   Monday = chore.Monday,
                                   Tuesday = chore.Tuesday,
                                   Wednesday = chore.Wednesday,
                                   Thursday = chore.Thursday,
                                   Friday = chore.Friday,
                                   Saturday = chore.Saturday,
                                   Sunday = chore.Sunday,
                                   UserName = assignedChore.UserName
                               }).ToListAsync();

            DateTime today = DateTime.Now;
            string dayOfWeek = today.DayOfWeek.ToString();
            List<AssignedChoreView> todaysChores = new List<AssignedChoreView>();

            foreach (var chore in joinedChores)
            {

                if (chore.UserName == userName)
                {
                    if (dayOfWeek == "Monday" && chore.Monday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }

                    if (dayOfWeek == "Tuesday" && chore.Tuesday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }
                    if (dayOfWeek == "Wednesday" && chore.Wednesday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }
                    if (dayOfWeek == "Thursday" && chore.Thursday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }
                    if (dayOfWeek == "Friday" && chore.Friday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }
                    if (dayOfWeek == "Saturday" && chore.Saturday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    }
                    if (dayOfWeek == "Sunday" && chore.Sunday == true)
                    {
                        AssignedChoreView newChore = new AssignedChoreView();
                        newChore.Id = chore.Id;
                        newChore.ChoreId = chore.ChoreId;
                        newChore.ChoreName = chore.ChoreName;
                        newChore.IsComplete = chore.IsComplete;
                        todaysChores.Add(newChore);
                    } 
                }
            }


            #region Old Method
            //DateTime today = DateTime.Today;
            //string dayOfWeek = today.DayOfWeek.ToString();

            // Load chores from database
            //List<Chore> chores = new List<Chore>();
            //chores = await GetChores();

            // Load user's assigned chores from database
            //List<AssignedChore> assignedChores = new List<AssignedChore>();
            //assignedChores = await GetAssignedChores();

            // Join assigned chores with chores
            //var joinedChores = from chore in chores
            //                   join assignedChore in assignedChores on chore.Id equals assignedChore.Id
            //                   select new
            //                   {
            //                       ChoreID = chore.Id,
            //                       ChoreName = chore.Name,
            //                       Monday = chore.Monday,
            //                       Tuesday = chore.Tuesday,
            //                       Wednesday = chore.Wednesday,
            //                       Thursday = chore.Thursday,
            //                       Friday = chore.Friday,
            //                       Saturday = chore.Saturday,
            //                       Sunday = chore.Sunday,
            //                       UserName = assignedChore.UserName,
            //                       IsComplete = assignedChore.IsComplete
            //                   };

            //List<AssignedChoreView> todaysChores = new List<AssignedChoreView>();

            //if (chores != null)
            //{
            //    foreach (var chore in joinedChores)
            //    {
            //        if (chore.UserName == userName)
            //        {
            //            if (dayOfWeek == "Monday" && chore.Monday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }

            //            if (dayOfWeek == "Tuesday" && chore.Tuesday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }
            //            if (dayOfWeek == "Wednesday" && chore.Wednesday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }
            //            if (dayOfWeek == "Thursday" && chore.Thursday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }
            //            if (dayOfWeek == "Friday" && chore.Friday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }
            //            if (dayOfWeek == "Saturday" && chore.Saturday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            }
            //            if (dayOfWeek == "Sunday" && chore.Sunday == true)
            //            {
            //                AssignedChoreView newChore = new AssignedChoreView();
            //                newChore.ChoreId = chore.ChoreID;
            //                newChore.ChoreName = chore.ChoreName;
            //                newChore.IsComplete = chore.IsComplete;
            //                todaysChores.Add(newChore);
            //            } 
            //        }
            //    }
            //}
            #endregion

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

        public async Task AddDummyCompletedChoreData()
        {
            List<int> choreIds = (from chore in _context.Chores
                                  select chore.Id).ToList();
            List<string> users = (from user in _context.Users
                                select user.Name).ToList();

            var random = new Random();

            for (int i = 500; i > 0; i--)
            {
                CompletedChore completedChore = new CompletedChore();

                int choreIndex = random.Next(choreIds.Count);
                int userIndex = random.Next(users.Count);
                DateTime start = new DateTime(2022, 3, 1);
                int range = (DateTime.Today - start).Days;

                completedChore.ChoreId = choreIds[choreIndex];
                completedChore.UserName = users[userIndex];
                completedChore.DateComplete = start.AddDays(random.Next(range));
                completedChore.IsComplete = random.Next(100) <= 20 ? true : false;
                await _context.CompletedChores.AddAsync(completedChore);
            }

            await _context.SaveChangesAsync();
        }
    }
}
