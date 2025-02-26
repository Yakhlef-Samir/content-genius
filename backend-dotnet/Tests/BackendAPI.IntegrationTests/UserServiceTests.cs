using System;
using System.Threading.Tasks;
using BackendAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BackendAPI.IntegrationTests
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserServiceTests()
        {
            var services = new ServiceCollection();
            
            // Dans un environnement réel, nous utiliserions une base de données de test
            services.AddScoped<IUserRepository, TestUserRepository>();
            services.AddScoped<IUserService, UserService>();

            var serviceProvider = services.BuildServiceProvider();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        }

        [Fact]
        public async Task HasAvailableCredits_WithPositiveBalance_ReturnsTrue()
        {
            // Arrange
            var userId = "test-user-1";
            await _userRepository.UpdateUserCreditsAsync(userId, 10);

            // Act
            var result = await _userService.HasAvailableCreditsAsync(userId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeductCredits_WithSufficientBalance_ReducesBalance()
        {
            // Arrange
            var userId = "test-user-2";
            await _userRepository.UpdateUserCreditsAsync(userId, 10);

            // Act
            await _userService.DeductCreditsAsync(userId, 5);
            var newBalance = await _userService.GetCreditsBalanceAsync(userId);

            // Assert
            newBalance.Should().Be(5);
        }

        [Fact]
        public async Task AddCredits_IncreasesBalance()
        {
            // Arrange
            var userId = "test-user-3";
            await _userRepository.UpdateUserCreditsAsync(userId, 10);

            // Act
            await _userService.AddCreditsAsync(userId, 5);
            var newBalance = await _userService.GetCreditsBalanceAsync(userId);

            // Assert
            newBalance.Should().Be(15);
        }
    }

    // Repository de test pour les tests d'intégration
    public class TestUserRepository : IUserRepository
    {
        private readonly Dictionary<string, int> _userCredits = new();

        public Task<int> GetUserCreditsAsync(string userId)
        {
            return Task.FromResult(_userCredits.GetValueOrDefault(userId));
        }

        public Task UpdateUserCreditsAsync(string userId, int newBalance)
        {
            _userCredits[userId] = newBalance;
            return Task.CompletedTask;
        }
    }
}
