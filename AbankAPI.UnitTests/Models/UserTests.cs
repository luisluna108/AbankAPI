using System;
using AbankAPI.Models;
using Xunit;

namespace AbankAPI.Domain.Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(0, user.Id);
            Assert.Equal(string.Empty, user.Nombres);
            Assert.Equal(string.Empty, user.Apellidos);
            Assert.Equal(default(DateTime), user.FechaNacimiento);
            Assert.Equal(string.Empty, user.Direccion);
            Assert.Equal(string.Empty, user.Password);
            Assert.Equal(string.Empty, user.Telefono);
            Assert.Equal(string.Empty, user.Email);
            Assert.Equal(default(DateTime), user.FechaCreacion);
            Assert.Null(user.FechaModificacion);
        }

        [Fact]
        public void User_SetProperties_AssignsValuesCorrectly()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var user = new User
            {
                Id = 1,
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle Falsa 123",
                Password = "secreto",
                Telefono = "123456789",
                Email = "juan@correo.com",
                FechaCreacion = now,
                FechaModificacion = now
            };

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("Juan", user.Nombres);
            Assert.Equal("Pérez", user.Apellidos);
            Assert.Equal(new DateTime(1990, 1, 1), user.FechaNacimiento);
            Assert.Equal("Calle Falsa 123", user.Direccion);
            Assert.Equal("secreto", user.Password);
            Assert.Equal("123456789", user.Telefono);
            Assert.Equal("juan@correo.com", user.Email);
            Assert.Equal(now, user.FechaCreacion);
            Assert.Equal(now, user.FechaModificacion);
        }
    }
}
