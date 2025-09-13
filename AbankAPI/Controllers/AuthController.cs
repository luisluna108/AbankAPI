using Microsoft.AspNetCore.Mvc;
using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using AbankAPI.Services;

namespace AbankAPI.Controllers
{
    [ApiController]
    [Route("api/V1/auth")]
    public class AuthController : ControllerBase
    { 
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica un usuario y retorna un token JWT
        /// </summary>
        /// <param name="loginRequest">Credenciales de usuario</param>
        /// <returns>Token JWT y información del usuario</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginRequest);

            if (result == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            return Ok(result);
        }
    }
}
