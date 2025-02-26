using System.Threading.Tasks;

namespace BackendAPI.Services
{
    public interface IUserRepository
    {
        Task<int> GetUserCreditsAsync(string userId);
        Task UpdateUserCreditsAsync(string userId, int newBalance);
    }
}
