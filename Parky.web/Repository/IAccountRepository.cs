using System.Threading.Tasks;
using Parky.web.Models;

namespace Parky.web.Repository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);
        Task<bool> RegisterAsync(string url, User user);
    }
}
