using AbankAPI.Configuration;
using AbankAPI.Extensions;
using AbankAPI.Interfaces;
using AbankAPI.Repositories;
using AbankAPI.Repositories.Interfaces;
using AbankAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace AbankAPI.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        
        [Fact]
        public void AddJwtAuthentication_RegistersAuthentication()
        {
            // Arrange
            var services = new ServiceCollection();
            var jwtSettings = new Dictionary<string, string>
            {
                {"JwtSettings:SecretKey", "supersecretkey1234567890"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(jwtSettings)
                .Build();

            // Act
            services.AddJwtAuthentication(configuration);
            var provider = services.BuildServiceProvider();

            // Assert
            var options = provider.GetService<IOptions<JwtSettings>>();
            Assert.NotNull(options);
            var authSchemeProvider = provider.GetService<IAuthenticationSchemeProvider>();
            Assert.NotNull(authSchemeProvider);
        }

       
    }
}
