using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AbankAPI.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_NoException_CallsNext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var wasCalled = false;
        RequestDelegate next = ctx =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(wasCalled);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        RequestDelegate next = ctx => throw new InvalidOperationException("Operación inválida");
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("Operación inválida", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WithArgumentNullException_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        RequestDelegate next = ctx => throw new ArgumentNullException("param");
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("Parámetro requerido faltante", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WithUnauthorizedAccessException_ReturnsUnauthorized()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        RequestDelegate next = ctx => throw new UnauthorizedAccessException();
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        // Assert
        Assert.Equal((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
        Assert.Equal("No autorizado", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WithGenericException_ReturnsInternalServerError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        RequestDelegate next = ctx => throw new Exception("Error genérico");
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(response);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("Ha ocurrido un error interno del servidor", json.GetProperty("message").GetString());
        Assert.Equal("Error genérico", json.GetProperty("details").GetString());
    }
}
