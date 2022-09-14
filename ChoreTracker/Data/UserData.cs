using ChoreTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChoreTracker.Data
{
    public class UserData : IUserData
    {
        private ApplicationDbContext _context;

        public UserData(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> users = await (from user in _context.Users
                                      orderby user.Name
                                      select user).ToListAsync();

            return users;
        }

        public async Task AssignChoreToUser(AssignedChore assignedChore)
        {
            await _context.AssignedChores.AddAsync(assignedChore);
            await _context.SaveChangesAsync();
        }
    }
}
