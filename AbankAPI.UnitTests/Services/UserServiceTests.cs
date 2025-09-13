using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbankAPI.Services;
using AbankAPI.Models;
using AbankAPI.Models.DTOs;
using AbankAPI.Repositories.Interfaces;
using AbankAPI.Interfaces;
using Moq;
using Xunit;

namespace AbankAPI.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _userService = new UserService(_userRepositoryMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsMappedUsers()
        {
            // Arrange
            var users = new List<User>
                        {
                            new User { Id = 1, Nombres = "Juan", Apellidos = "Pérez", FechaNacimiento = new DateTime(1990,1,1), Direccion = "Calle 1", Telefono = "123", Email = "juan@mail.com", FechaCreacion = DateTime.UtcNow },
                            new User { Id = 2, Nombres = "Ana", Apellidos = "García", FechaNacimiento = new DateTime(1985,5,5), Direccion = "Calle 2", Telefono = "456", Email = "ana@mail.com", FechaCreacion = DateTime.UtcNow }
                        };
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Nombres == "Juan");
            Assert.Contains(result, u => u.Nombres == "Ana");
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUsers()
        {
            var user = new User { Id = 1, Nombres = "Juan", Apellidos = "Pérez", FechaNacimiento = new DateTime(1990, 1, 1), Direccion = "Calle 1", Telefono = "123", Email = "juan@mail.com", FechaCreacion = DateTime.UtcNow };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Juan", result!.Nombres);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ReturnsNull()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_EmailExists_ThrowsException()
        {
            var dto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle 1",
                Password = "password",
                Telefono = "123",
                Email = "juan@mail.com"
            };
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email,1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUserAsync(dto));
        }

        [Fact]
        public async Task CreateUserAsync_ValidData_ReturnsUserResponseDto()
        {
            var dto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle 1",
                Password = "password",
                Telefono = "123",
                Email = "juan@mail.com"
            };
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email,1)).ReturnsAsync(false);
            _passwordServiceMock.Setup(p => p.HashPassword(dto.Password)).Returns("hashed");
            _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(1);

            var result = await _userService.CreateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Juan", result.Nombres);
            Assert.Equal("juan@mail.com", result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ReturnsNull()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);
            var dto = new UpdateUserDto();

            var result = await _userService.UpdateUserAsync(1, dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserAsync_EmailExists_ThrowsException()
        {
            var user = new User { Id = 1, Email = "old@mail.com" };
            var dto = new UpdateUserDto { Email = "new@mail.com" };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email, 1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserAsync(1, dto));
        }

        [Fact]
        public async Task UpdateUserAsync_ValidData_UpdatesAndReturnsUser()
        {
            var user = new User
            {
                Id = 1,
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle 1",
                Password = "old",
                Telefono = "123",
                Email = "juan@mail.com",
                FechaCreacion = DateTime.UtcNow
            };
            var dto = new UpdateUserDto
            {
                Nombres = "Juanito",
                Password = "newpass"
            };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), 1)).ReturnsAsync(false);
            _passwordServiceMock.Setup(p => p.HashPassword("newpass")).Returns("hashedpass");
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

            var result = await _userService.UpdateUserAsync(1, dto);

            Assert.NotNull(result);
            Assert.Equal("Juanito", result!.Nombres);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsRepository()
        {
            _userRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(1);

            Assert.True(result);
        }
    }
}
