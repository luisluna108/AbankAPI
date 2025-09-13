using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AbankAPI.Models.DTOs;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.DTOs
{
    public class CreateUserDtoTests
    {
        [Fact]
        public void CreateUserDto_ValidData_PassesValidation()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle Falsa 123",
                Password = "Password123",
                Telefono = "123456789",
                Email = "juan.perez@email.com"
            };

            // Act
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void CreateUserDto_MissingRequiredFields_FailsValidation()
        {
            // Arrange
            var dto = new CreateUserDto();

            // Act
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void CreateUserDto_InvalidEmail_FailsValidation()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle Falsa 123",
                Password = "Password123",
                Telefono = "123456789",
                Email = "no-es-un-email"
            };

            // Act
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Formato de email inválido"));
        }
    }
}
