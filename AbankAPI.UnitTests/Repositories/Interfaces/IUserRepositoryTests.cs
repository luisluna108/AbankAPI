using System.Collections.Generic;
using System.Threading.Tasks;
using AbankAPI.Models;
using AbankAPI.Repositories.Interfaces;
using Moq;
using Xunit;

namespace AbankAPI.Infrastructure.Tests.Repositories.Interfaces
{
    public class IUserRepositoryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public IUserRepositoryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" },
                new User { Id = 2, Nombres = "Ana", Apellidos = "García", Email = "ana@correo.com" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Collection(result,
                user => Assert.Equal("Juan", user.Nombres),
                user => Assert.Equal("Ana", user.Nombres));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = 1, Nombres = "Juan", Email = "juan@correo.com" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userRepositoryMock.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync("noexiste@correo.com")).ReturnsAsync((User?)null);

            // Act
            var result = await _userRepositoryMock.Object.GetByEmailAsync("noexiste@correo.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsNewUserId()
        {
            // Arrange
            var user = new User { Nombres = "Nuevo", Email = "nuevo@correo.com" };
            _userRepositoryMock.Setup(repo => repo.CreateAsync(user)).ReturnsAsync(3);

            // Act
            var result = await _userRepositoryMock.Object.CreateAsync(user);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Nombres = "Actualizado" };
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).ReturnsAsync(true);

            // Act
            var result = await _userRepositoryMock.Object.UpdateAsync(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _userRepositoryMock.Object.DeleteAsync(99);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.EmailExistsAsync("existe@correo.com", null)).ReturnsAsync(true);

            // Act
            var result = await _userRepositoryMock.Object.EmailExistsAsync("existe@correo.com");

            // Assert
            Assert.True(result);
        }
    }
}
