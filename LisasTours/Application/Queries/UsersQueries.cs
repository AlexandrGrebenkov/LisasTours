using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Data;
using LisasTours.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Application.Queries
{
    public class UsersQueries : IUsersQueries
    {
        private readonly ApplicationDbContext context;

        public UsersQueries(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }

        public ApplicationUser GetUser(int id)
        {
            return context.Users.Find(id);
        }
    }
}
