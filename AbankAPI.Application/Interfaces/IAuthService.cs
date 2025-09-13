using AbankAPI.Models.DTOs;

namespace AbankAPI.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);

    }
}
