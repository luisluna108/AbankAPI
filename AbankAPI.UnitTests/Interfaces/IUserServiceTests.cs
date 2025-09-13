using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using Moq;
using Xunit;

namespace AbankAPI.Application.Tests.Services
{
    public class IUserServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock;

        public IUserServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUserList()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" },
                new UserResponseDto { Id = 2, Nombres = "Ana", Apellidos = "García", Email = "ana@correo.com" }
            };
            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userServiceMock.Object.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var user = new UserResponseDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userServiceMock.Object.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(99)).ReturnsAsync((UserResponseDto?)null);

            // Act
            var result = await _userServiceMock.Object.GetUserByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_ValidData_ReturnsCreatedUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Nombres = "Carlos",
                Apellidos = "Ramírez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle 123",
                Password = "password123",
                Telefono = "123456789",
                Email = "carlos@correo.com"
            };
            var createdUser = new UserResponseDto { Id = 3, Nombres = "Carlos", Apellidos = "Ramírez", Email = "carlos@correo.com" };
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto)).ReturnsAsync(createdUser);

            // Act
            var result = await _userServiceMock.Object.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Carlos", result.Nombres);
        }

        [Fact]
        public async Task UpdateUserAsync_UserExists_ReturnsUpdatedUser()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Nombres = "Pedro" };
            var updatedUser = new UserResponseDto { Id = 1, Nombres = "Pedro", Apellidos = "Pérez", Email = "juan@correo.com" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(1, updateUserDto)).ReturnsAsync(updatedUser);

            // Act
            var result = await _userServiceMock.Object.UpdateUserAsync(1, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pedro", result.Nombres);
        }

        [Fact]
        public async Task UpdateUserAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Nombres = "Pedro" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(99, updateUserDto)).ReturnsAsync((UserResponseDto?)null);

            // Act
            var result = await _userServiceMock.Object.UpdateUserAsync(99, updateUserDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUserAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            _userServiceMock.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _userServiceMock.Object.DeleteUserAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _userServiceMock.Setup(s => s.DeleteUserAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _userServiceMock.Object.DeleteUserAsync(99);

            // Assert
            Assert.False(result);
        }
    }
}
