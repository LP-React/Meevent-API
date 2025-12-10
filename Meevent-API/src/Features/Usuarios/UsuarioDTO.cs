
using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.Usuarios
{
    public class UsuarioDTO
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; }
        public string correo_electronico { get; set; }
        public string? numero_telefono { get; set; }
        public string? imagen_perfil_url { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public string tipo_usuario { get; set; }
    }

    public class UsuarioListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalUsuarios { get; set; }
        public IEnumerable<UsuarioDTO> Usuarios { get; set; }
    }

    public class UsuarioRegistroDTO
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string nombre_completo { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string correo_electronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "La contraseña debe tener al menos 1 mayúscula y 1 número")]
        public string contrasena { get; set; }

        public string? numero_telefono { get; set; }
        public string? imagen_perfil_url { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string correo_electronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string contrasena { get; set; }
    }

    public class LoginResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }

    public class PasswordHasher
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }

    public class UsuarioEditarDTO
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        public int id_usuario { get; set; }

        public string? contrasena { get; set; }  

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string nombre_completo { get; set; }

        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? numero_telefono { get; set; }

        [Url(ErrorMessage = "La URL de la imagen no es válida")]
        [StringLength(300, ErrorMessage = "La URL no puede exceder 300 caracteres")]
        public string? imagen_perfil_url { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido")]
        public DateTime? fecha_nacimiento { get; set; }

        public bool? email_verificado { get; set; }

        public bool? cuenta_activa { get; set; }

        [RegularExpression("^(normal|artista|organizador)$",
            ErrorMessage = "El tipo de usuario debe ser: normal, artista u organizador")]
        public string? tipo_usuario { get; set; }
    }

    public class UsuarioEditarResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public UsuarioDTO UsuarioActualizado { get; set; }
    }

}