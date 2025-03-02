using System.Threading.Tasks;

namespace BackendAPI.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Récupère le nombre de crédits restants pour un utilisateur
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <returns>Le nombre de crédits restants</returns>
        Task<int> GetRemainingCredits(string userId);
        Task<bool> HasAvailableCreditsAsync(string userId);
        Task DeductCreditsAsync(string userId, int amount = 1);
        Task AddCreditsAsync(string userId, int amount);
        Task<int> GetCreditsBalanceAsync(string userId);
    }
}
