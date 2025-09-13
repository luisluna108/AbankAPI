using AbankAPI.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.DTOs
{
    public class LoginRequestDtoTests
    {
        [Fact]
        public void LoginRequestDto_ValidData_PassesValidation()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "usuario@dominio.com",
                Password = "ContraseñaSegura123"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void LoginRequestDto_MissingEmail_FailsValidation()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "",
                Password = "ContraseñaSegura123"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "El email es requerido");
        }

        [Fact]
        public void LoginRequestDto_InvalidEmailFormat_FailsValidation()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "correo-invalido",
                Password = "ContraseñaSegura123"
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Formato de email inválido");
        }

        [Fact]
        public void LoginRequestDto_MissingPassword_FailsValidation()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "usuario@dominio.com",
                Password = ""
            };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "La contraseña es requerida");
        }
    }
}
