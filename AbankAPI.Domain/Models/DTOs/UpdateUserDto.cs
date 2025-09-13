using System.ComponentModel.DataAnnotations;

namespace AbankAPI.Models.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(100, ErrorMessage = "Los nombres no pueden exceder 100 caracteres")]
        public string? Nombres { get; set; }

        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        public string? Apellidos { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(255, ErrorMessage = "La dirección no puede exceder 255 caracteres")]
        public string? Direccion { get; set; }

        [StringLength(120, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 120 caracteres")]
        public string? Password { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? Email { get; set; }
    }
}
