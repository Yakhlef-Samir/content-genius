using System;
using System.Threading.Tasks;

namespace BackendAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> HasAvailableCreditsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            var credits = await _userRepository.GetUserCreditsAsync(userId);
            return credits > 0;
        }

        public async Task DeductCreditsAsync(string userId, int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            var currentCredits = await _userRepository.GetUserCreditsAsync(userId);
            
            if (currentCredits < amount)
                throw new InvalidOperationException("Insufficient credits");

            await _userRepository.UpdateUserCreditsAsync(userId, currentCredits - amount);
        }

        public async Task AddCreditsAsync(string userId, int amount)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            var currentCredits = await _userRepository.GetUserCreditsAsync(userId);
            await _userRepository.UpdateUserCreditsAsync(userId, currentCredits + amount);
        }

        public async Task<int> GetCreditsBalanceAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            return await _userRepository.GetUserCreditsAsync(userId);
        }
    }
}
