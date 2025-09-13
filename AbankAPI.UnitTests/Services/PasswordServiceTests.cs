using AbankAPI.Services;
using AbankAPI.Interfaces;
using Xunit;

namespace AbankAPI.Application.Tests.Services
{
    public class PasswordServiceTests
    {
        private readonly IPasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hashed = _passwordService.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(hashed));
            Assert.NotEqual(password, hashed);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashed = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(password, hashed);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            var wrongPassword = "WrongPassword456!";
            var hashed = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(wrongPassword, hashed);

            // Assert
            Assert.False(result);
        }
    }
}
