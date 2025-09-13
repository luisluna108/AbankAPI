using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AbankAPI.Services;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using AbankAPI.Models;
using AbankAPI.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using AbankAPI.Configuration;

namespace AbankAPI.Application.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _jwtSettings = Options.Create(new JwtSettings
            {
                SecretKey = "clave_super_secreta_1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationInHours = 1
            });
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordServiceMock.Object,
                _jwtSettings
            );
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            var loginRequest = new LoginRequestDto { Email = "test@test.com", Password = "1234" };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new User { Id = 1, Email = "test@test.com", Password = "hashed" };
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);
            _passwordServiceMock.Setup(p => p.VerifyPassword("1234", "hashed"))
                .Returns(false);
            var loginRequest = new LoginRequestDto { Email = user.Email, Password = "1234" };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert
            Assert.Null(result);
        }

        
    }
}
