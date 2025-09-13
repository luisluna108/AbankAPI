using System;
using AbankAPI.Models.DTOs;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.DTOs
{
    public class LoginResponseDtoTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesWithDefaults()
        {
            // Act
            var dto = new LoginResponseDto();

            // Assert
            Assert.Equal(string.Empty, dto.Token);
            Assert.Equal(default(DateTime), dto.ExpiresAt);
            Assert.NotNull(dto.User);
            Assert.IsType<UserResponseDto>(dto.User);
        }

        [Fact]
        public void Properties_SetAndGetValues_Correctly()
        {
            // Arrange
            var user = new UserResponseDto
            {
                Id = 1,
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                Email = "juan@correo.com",
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow
            };
            var token = "abc123";
            var expiresAt = DateTime.UtcNow.AddHours(1);

            // Act
            var dto = new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = user
            };

            // Assert
            Assert.Equal(token, dto.Token);
            Assert.Equal(expiresAt, dto.ExpiresAt);
            Assert.Equal(user, dto.User);
        }
    }
}
