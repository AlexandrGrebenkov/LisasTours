using System.Collections.Generic;
using System.Threading.Tasks;
using LisasTours.Models.Identity;

namespace LisasTours.Application.Queries
{
    public interface IUsersQueries
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        ApplicationUser GetUser(string id);
    }
}
