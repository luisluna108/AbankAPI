using AbankAPI.Controllers;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AbankAPI.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" }
            };
            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new UserResponseDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((UserResponseDto?)null);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreated_WhenValid()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = DateTime.Now.AddYears(-20),
                Direccion = "Calle 123",
                Password = "password123",
                Telefono = "123456789",
                Email = "juan@correo.com"
            };
            var user = new UserResponseDto { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@correo.com" };
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser(createUserDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(user, createdResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nombres", "Requerido");
            var createUserDto = new CreateUserDto();

            // Act
            var result = await _controller.CreateUser(createUserDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        }

        [Fact]
        public async Task CreateUser_ReturnsConflict_WhenServiceThrows()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = DateTime.Now.AddYears(-20),
                Direccion = "Calle 123",
                Password = "password123",
                Telefono = "123456789",
                Email = "juan@correo.com"
            };
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto))
                .ThrowsAsync(new InvalidOperationException("Usuario ya existe"));

            // Act
            var result = await _controller.CreateUser(createUserDto);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOk_WhenUserUpdated()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Nombres = "Pedro" };
            var user = new UserResponseDto { Id = 1, Nombres = "Pedro", Apellidos = "Pérez", Email = "pedro@correo.com" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(1, updateUserDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateUser(1, updateUserDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Nombres = "Pedro" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(1, updateUserDto)).ReturnsAsync((UserResponseDto?)null);

            // Act
            var result = await _controller.UpdateUser(1, updateUserDto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nombres", "Requerido");
            var updateUserDto = new UpdateUserDto();

            // Act
            var result = await _controller.UpdateUser(1, updateUserDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsConflict_WhenServiceThrows()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Nombres = "Pedro" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(1, updateUserDto))
                .ThrowsAsync(new InvalidOperationException("Error de actualización"));

            // Act
            var result = await _controller.UpdateUser(1, updateUserDto);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenDeleted()
        {
            // Arrange
            _userServiceMock.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
        }
    }
}
