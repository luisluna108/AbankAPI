using AbankAPI.Interfaces;
using AbankAPI.Models.DTOs;
using AbankAPI.Models;
using AbankAPI.Repositories.Interfaces;

namespace AbankAPI.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToResponseDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToResponseDto(user) : null;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Verificar si el email ya existe
            if (await _userRepository.EmailExistsAsync(createUserDto.Email))
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            var user = new User
            {
                Nombres = createUserDto.Nombres,
                Apellidos = createUserDto.Apellidos,
                FechaNacimiento = createUserDto.FechaNacimiento,
                Direccion = createUserDto.Direccion,
                Password = _passwordService.HashPassword(createUserDto.Password),
                Telefono = createUserDto.Telefono,
                Email = createUserDto.Email,
                FechaCreacion = DateTime.UtcNow
            };

            var userId = await _userRepository.CreateAsync(user);
            user.Id = userId;

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            // Verificar si el email ya existe (excluyendo el usuario actual)
            if (!string.IsNullOrEmpty(updateUserDto.Email) &&
                await _userRepository.EmailExistsAsync(updateUserDto.Email, id))
            {
                throw new InvalidOperationException("El email ya está registrado por otro usuario");
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(updateUserDto.Nombres))
                existingUser.Nombres = updateUserDto.Nombres;

            if (!string.IsNullOrEmpty(updateUserDto.Apellidos))
                existingUser.Apellidos = updateUserDto.Apellidos;

            if (updateUserDto.FechaNacimiento.HasValue)
                existingUser.FechaNacimiento = updateUserDto.FechaNacimiento.Value;

            if (!string.IsNullOrEmpty(updateUserDto.Direccion))
                existingUser.Direccion = updateUserDto.Direccion;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
                existingUser.Password = _passwordService.HashPassword(updateUserDto.Password);

            if (!string.IsNullOrEmpty(updateUserDto.Telefono))
                existingUser.Telefono = updateUserDto.Telefono;

            if (!string.IsNullOrEmpty(updateUserDto.Email))
                existingUser.Email = updateUserDto.Email;

            existingUser.FechaModificacion = DateTime.UtcNow;

            var updated = await _userRepository.UpdateAsync(existingUser);
            return updated ? MapToResponseDto(existingUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        private static UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Nombres = user.Nombres,
                Apellidos = user.Apellidos,
                FechaNacimiento = user.FechaNacimiento,
                Direccion = user.Direccion,
                Telefono = user.Telefono,
                Email = user.Email,
                FechaCreacion = user.FechaCreacion,
                FechaModificacion = user.FechaModificacion
            };
        }
    }
}
