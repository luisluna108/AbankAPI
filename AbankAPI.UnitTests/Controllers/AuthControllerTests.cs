using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using AbankAPI.Controllers;

namespace UnitTest.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var loginRequest = new LoginRequestDto { Email = "test@email.com", Password = "password" };
            var loginResponse = new LoginResponseDto
            {
                Token = "fake-jwt-token",
                ExpiresAt = System.DateTime.UtcNow.AddHours(1),
                User = new UserResponseDto
                {
                    Id = 1,
                    Nombres = "Juan",
                    Apellidos = "Pérez",
                    FechaNacimiento = new System.DateTime(1990, 1, 1),
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    Email = "test@email.com",
                    FechaCreacion = System.DateTime.UtcNow,
                    FechaModificacion = null
                }
            };
            mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(loginResponse);
            var controller = new AuthController(mockAuthService.Object);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.Equal(loginResponse.Token, returnedValue.Token);
            Assert.Equal(loginResponse.User.Email, returnedValue.User.Email);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var loginRequest = new LoginRequestDto { Email = "wrong@email.com", Password = "wrongpass" };
            mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync((LoginResponseDto?)null);
            var controller = new AuthController(mockAuthService.Object);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Contains("Credenciales inválidas", unauthorizedResult.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var controller = new AuthController(mockAuthService.Object);
            controller.ModelState.AddModelError("Email", "El email es requerido");
            var loginRequest = new LoginRequestDto { Email = "", Password = "password" };

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}
