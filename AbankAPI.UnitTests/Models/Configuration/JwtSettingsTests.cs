using AbankAPI.Configuration;
using Xunit;

namespace AbankAPI.Domain.Tests.Models.Configuration
{
    public class JwtSettingsTests
    {
        [Fact]
        public void JwtSettings_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var settings = new JwtSettings();

            // Assert
            Assert.Equal(string.Empty, settings.SecretKey);
            Assert.Equal(string.Empty, settings.Issuer);
            Assert.Equal(string.Empty, settings.Audience);
            Assert.Equal(24, settings.ExpirationInHours);
        }

        [Fact]
        public void JwtSettings_SetProperties_ValuesAreSet()
        {
            // Arrange
            var settings = new JwtSettings
            {
                SecretKey = "clave",
                Issuer = "emisor",
                Audience = "audiencia",
                ExpirationInHours = 12
            };

            // Assert
            Assert.Equal("clave", settings.SecretKey);
            Assert.Equal("emisor", settings.Issuer);
            Assert.Equal("audiencia", settings.Audience);
            Assert.Equal(12, settings.ExpirationInHours);
        }
    }
}
