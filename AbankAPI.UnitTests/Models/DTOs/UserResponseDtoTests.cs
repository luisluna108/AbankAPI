using System;
using AbankAPI.Models.DTOs;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.DTOs
{
    public class UserResponseDtoTests
    {
        [Fact]
        public void UserResponseDto_Properties_SetAndGetValues()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var dto = new UserResponseDto
            {
                Id = 1,
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 5, 10),
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                Email = "juan.perez@email.com",
                FechaCreacion = now,
                FechaModificacion = now.AddMinutes(5)
            };

            // Assert
            Assert.Equal(1, dto.Id);
            Assert.Equal("Juan", dto.Nombres);
            Assert.Equal("Pérez", dto.Apellidos);
            Assert.Equal(new DateTime(1990, 5, 10), dto.FechaNacimiento);
            Assert.Equal("Calle Falsa 123", dto.Direccion);
            Assert.Equal("555-1234", dto.Telefono);
            Assert.Equal("juan.perez@email.com", dto.Email);
            Assert.Equal(now, dto.FechaCreacion);
            Assert.Equal(now.AddMinutes(5), dto.FechaModificacion);
        }

        [Fact]
        public void UserResponseDto_DefaultValues_AreCorrect()
        {
            // Act
            var dto = new UserResponseDto();

            // Assert
            Assert.Equal(0, dto.Id);
            Assert.Equal(string.Empty, dto.Nombres);
            Assert.Equal(string.Empty, dto.Apellidos);
            Assert.Equal(default(DateTime), dto.FechaNacimiento);
            Assert.Equal(string.Empty, dto.Direccion);
            Assert.Equal(string.Empty, dto.Telefono);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Null(dto.FechaCreacion);
            Assert.Null(dto.FechaModificacion);
        }
    }
}
