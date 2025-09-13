using System;
using System.Threading.Tasks;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using Moq;
using Xunit;

namespace AbankAPI.Application.Tests.Services
{
    public class IAuthServiceTests
    {
        [Fact]
        public async Task LoginAsync_ReturnsLoginResponseDto_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "test@correo.com",
                Password = "Password123!"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "fake-jwt-token",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = new UserResponseDto
                {
                    Id = 1,
                    Nombres = "Juan",
                    Apellidos = "Pérez",
                    FechaNacimiento = new DateTime(1990, 1, 1),
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    Email = "test@correo.com",
                    FechaCreacion = DateTime.UtcNow,
                    FechaModificacion = null
                }
            };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await authServiceMock.Object.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Token, result.Token);
            Assert.Equal(expectedResponse.User.Email, result.User.Email);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "wrong@correo.com",
                Password = "wrongpassword"
            };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync((LoginResponseDto?)null);

            // Act
            var result = await authServiceMock.Object.LoginAsync(loginRequest);

            // Assert
            Assert.Null(result);
        }
    }
}
