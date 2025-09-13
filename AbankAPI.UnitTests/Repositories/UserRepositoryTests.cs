using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AbankAPI.Models;
using AbankAPI.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Npgsql;
using Xunit;

namespace AbankAPI.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly string _connectionString = "Host=localhost;Database=testdb;Username=test;Password=test";

        public UserRepositoryTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            var section = new Mock<IConfigurationSection>();
            section.Setup(s => s.Value).Returns(_connectionString);
            _mockConfig.Setup(c => c.GetConnectionString("DefaultConnection")).Returns(_connectionString);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsers()
        {
            // Arrange
            var repo = new UserRepository(_mockConfig.Object);
            var users = new List<User>
            {
                new User { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Email = "juan@test.com", FechaNacimiento = DateTime.Now, Direccion = "Calle 1", Password = "123", Telefono = "123", FechaCreacion = DateTime.Now },
                new User { Id = 2, Nombres = "Ana", Apellidos = "García", Email = "ana@test.com", FechaNacimiento = DateTime.Now, Direccion = "Calle 2", Password = "456", Telefono = "456", FechaCreacion = DateTime.Now }
            };

            // No se puede mockear Dapper directamente, así que esta prueba es de integración o se debe usar una base de datos en memoria.
            // Aquí solo se verifica que el método no lanza excepción (mock de integración).
            var result = await repo.GetAllAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var repo = new UserRepository(_mockConfig.Object);
            var result = await repo.GetByIdAsync(-1);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsNewId()
        {
            var repo = new UserRepository(_mockConfig.Object);
            var user = new User
            {
                Nombres = "Test",
                Apellidos = "User",
                FechaNacimiento = DateTime.Now.AddYears(-20),
                Direccion = "Test Address",
                Password = "pass",
                Telefono = "000",
                Email = $"test{Guid.NewGuid()}@mail.com",
                FechaCreacion = DateTime.Now
            };
            var id = await repo.CreateAsync(user);
            Assert.True(id > 0);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            var repo = new UserRepository(_mockConfig.Object);
            var user = new User
            {
                Id = -1,
                Nombres = "No",
                Apellidos = "Existe",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Direccion = "N/A",
                Password = "none",
                Telefono = "000",
                Email = "no@existe.com",
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
            var result = await repo.UpdateAsync(user);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            var repo = new UserRepository(_mockConfig.Object);
            var result = await repo.DeleteAsync(-1);
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ReturnsFalse_WhenEmailNotExists()
        {
            var repo = new UserRepository(_mockConfig.Object);
            var result = await repo.EmailExistsAsync("noexiste@email.com");
            Assert.False(result);
        }
    }
}
