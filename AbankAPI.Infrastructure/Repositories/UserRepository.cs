using AbankAPI.Models;
using AbankAPI.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AbankAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            const string sql = @"
            SELECT id, nombres, apellidos, fechanacimiento, direccion, 
                   password, telefono, email, fechacreacion, fechamodificacion
            FROM usuarios 
            ORDER BY id ASC";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT id, nombres, apellidos, fechanacimiento, direccion, 
                   password, telefono, email, fechacreacion, fechamodificacion
            FROM usuarios 
            WHERE id = @Id";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"
            SELECT id, nombres, apellidos, fechanacimiento, direccion, 
                   password, telefono, email, fechacreacion, fechamodificacion
            FROM usuarios 
            WHERE email = @Email";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateAsync(User user)
        {
            const string sql = @"
            INSERT INTO usuarios (nombres, apellidos, fechanacimiento, direccion, 
                                password, telefono, email, fechacreacion)
            VALUES (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, 
                    @Password, @Telefono, @Email, @FechaCreacion)
            RETURNING id";

            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>(sql, user);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            const string sql = @"
            UPDATE usuarios 
            SET nombres = @Nombres, apellidos = @Apellidos, 
                fechanacimiento = @FechaNacimiento, direccion = @Direccion,
                password = @Password, telefono = @Telefono, email = @Email,
                fechamodificacion = @FechaModificacion
            WHERE id = @Id";

            using var connection = new NpgsqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM usuarios WHERE id = @Id";

            using var connection = new NpgsqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var sql = "SELECT COUNT(1) FROM usuarios WHERE email = @Email";
            var parameters = new { Email = email, ExcludeUserId = 0 };

            if (excludeUserId.HasValue)
            {
                sql += " AND id != @ExcludeUserId";
                parameters = new { Email = email, ExcludeUserId = excludeUserId.Value };
            }

            using var connection = new NpgsqlConnection(_connectionString);
            var count = await connection.QuerySingleAsync<int>(sql, parameters);
            return count > 0;
        }
    }

}
