using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AbankAPI.Models.DTOs;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.DTOs
{
    public class UpdateUserDtoTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void UpdateUserDto_ValidData_PassesValidation()
        {
            var dto = new UpdateUserDto
            {
                Nombres = "Juan",
                Apellidos = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Direccion = "Calle Falsa 123",
                Password = "Password123",
                Telefono = "123456789",
                Email = "juan.perez@email.com"
            };

            var results = ValidateModel(dto);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateUserDto_InvalidEmail_FailsValidation()
        {
            var dto = new UpdateUserDto
            {
                Email = "correo-invalido"
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage == "Formato de email inválido");
        }

        [Fact]
        public void UpdateUserDto_NombresExcedeLongitud_FailsValidation()
        {
            var dto = new UpdateUserDto
            {
                Nombres = new string('a', 101)
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage == "Los nombres no pueden exceder 100 caracteres");
        }

        [Fact]
        public void UpdateUserDto_PasswordMuyCorta_FailsValidation()
        {
            var dto = new UpdateUserDto
            {
                Password = "123"
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage == "La contraseña debe tener entre 6 y 120 caracteres");
        }

        [Fact]
        public void UpdateUserDto_TelefonoExcedeLongitud_FailsValidation()
        {
            var dto = new UpdateUserDto
            {
                Telefono = new string('1', 21)
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage == "El teléfono no puede exceder 20 caracteres");
        }
    }
}
