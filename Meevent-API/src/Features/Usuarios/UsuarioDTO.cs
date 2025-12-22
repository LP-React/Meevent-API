using Meevent_API.src.Features.Paises;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Meevent_API.src.Features.Usuarios
{
    public class UsuarioDTO
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; }
        public string correo_electronico { get; set; }
        public string? numero_telefono { get; set; }
        public string? imagen_perfil_url { get; set; }
        public DateOnly? fecha_nacimiento { get; set; }
        public DateOnly fecha_creacion { get; set; }
        public DateOnly fecha_actualizacion { get; set; }
        public string tipo_usuario { get; set; }
        public object? perfil { get; set; }
        public bool cuenta_activa { get; set; }
        public CiudadDTO? jsonCiudad { get; set; }

    }

    public class UsuarioListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalUsuarios { get; set; }
        public IEnumerable<UsuarioDTO> Usuarios { get; set; }
    }

    // DTO para el registro de usuarios
    public class UsuarioRegistroDTO
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string nombre_completo { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(150)]
        public string correo_electronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe tener al menos una mayúscula y un número")]
        public string contrasenia { get; set; }

        [RegularExpression(@"^\+?\d+$", ErrorMessage = "El número de teléfono solo debe contener números")]
        [StringLength(20)]
        public string? numero_telefono { get; set; }

        public string? imagen_perfil_url { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        [RegularExpression("^(normal|artista|organizador)$",
        ErrorMessage = "El tipo de usuario debe ser: normal, artista u organizador")]
        public string tipo_usuario { get; set; } = "normal";

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public int id_ciudad { get; set; } = 1;

        // Campos para Artista (Opcionales)
        public string? nombre_artistico { get; set; }
        public string? biografia_artista { get; set; }
        public string? genero_musical { get; set; }

        // Campos para Organizador (Opcionales)
        public string? nombre_organizador { get; set; }
        public string? descripcion_organizador { get; set; }
        public string? telefono_contacto { get; set; }
    }

    // DTO para la actualización de usuarios
    public class UsuarioUpdateDTO
    {
        public int id_usuario { get; set; }
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string? nombre_completo { get; set; }
        public int? id_ciudad { get; set; }

        [RegularExpression(@"^\+?\d+$", ErrorMessage = "El número de teléfono solo debe contener números")]
        [StringLength(20)]
        public string? numero_telefono { get; set; }
        public string? imagen_perfil_url { get; set; }

        // Campos de Perfiles
        public string? nombre_artistico { get; set; }
        public string? biografia_artista { get; set; }
        public string? genero_musical { get; set; }
        public string? nombre_organizador { get; set; }
        public string? descripcion_organizador { get; set; }
        public string? telefono_contacto { get; set; }
    }

    public class UsuarioCambiarPasswordDTO
    {
        public string contrasenia;
    }

    public class UsuarioUpdateResponseDTO
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; } = null!;
        public string correo_electronico { get; set; } = null!;
        public string tipo_usuario { get; set; } = null!;
        public string imagen_perfil_url { get; set; } = null!;
        public bool cuenta_activa { get; set; }

        // El Hash se queda en el DAO/Service, no se envía al cliente
        [JsonIgnore]
        public string contrasena_hash { get; set; } = null!;

        // Perfil Artista
        public string? nombre_artistico { get; set; }
        public string? genero_musical { get; set; }

        // Perfil Organizador
        public string? nombre_organizador { get; set; }
        public string? descripcion_organizador { get; set; }
        public PerfilArtistaDTO? PerfilArtista { get; set; }
        public PerfilOrganizadorDTO? PerfilOrganizador { get; set; }
    }

    public class UpdateResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public UsuarioDetalleDTO UsuarioActualizado { get; set; }
    }

    // DTO para el login de usuarios
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string correo_electronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string contrasenia { get; set; }
    }

    // DTO para el perfil de artista
    public class PerfilArtistaDTO
    {
        public int id_perfil_artista { get; set; }
        public string nombre_artistico { get; set; } = null!;
        public string? biografia_artista { get; set; }
        public string? genero_musical { get; set; }
        public string? sitio_web { get; set; }
        public string? facebook_url { get; set; }
        public string? instagram_url { get; set; }
        public string? tiktok_url { get; set; }
    }

    // DTO para el perfil de organizador
    public class PerfilOrganizadorDTO
    {
        public int id_perfil_organizador { get; set; }
        public string nombre_organizador { get; set; } = null!;
        public string? descripcion_organizador { get; set; }
        public string? direccion_organizador { get; set; }
        public string? telefono_contacto { get; set; }
        public string? sitio_web { get; set; }
        public string? logo_url { get; set; }
        public string? facebook_url { get; set; }
        public string? instagram_url { get; set; }
        public string? tiktok_url { get; set; }
        public string? twitter_url { get; set; }
    }

    // DTO para la respuesta del login
    public class UsuarioLoginResponseDTO
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; } = null!;
        public string correo_electronico { get; set; } = null!;
        public string tipo_usuario { get; set; } = null!;
        public string imagen_perfil_url { get; set; } = null!;
        public bool cuenta_activa { get; set; }

        // El Hash se queda en el DAO/Service, no se envía al cliente
        [JsonIgnore]
        public string contrasena_hash { get; set; } = null!;

        // Perfil Artista
        public string? nombre_artistico { get; set; }
        public string? genero_musical { get; set; }

        // Perfil Organizador
        public string? nombre_organizador { get; set; }
        public string? descripcion_organizador { get; set; }
        public PerfilArtistaDTO? PerfilArtista { get; set; }
        public PerfilOrganizadorDTO? PerfilOrganizador { get; set; }
    }

    public class LoginResponseDTOE
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = null!;
        public UsuarioLoginResponseDTO? Usuario { get; set; }
    }

    public class UsuarioActivarCuentaDTO
    {
        [Required(ErrorMessage = "El estado de la cuenta es obligatorio")]
        public bool cuenta_activa { get; set; }

    }

    public class UsuarioActivarCuentaResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public bool CuentaActiva { get; set; }
    }

    // DTO para la ubicación (Ciudad y País)
    public class UbicacionDTO
    {
        public int id_ciudad { get; set; }
        public string nombre_ciudad { get; set; } = null!;
        public int id_pais { get; set; }
        public string nombre_pais { get; set; } = null!;
        public string codigo_iso { get; set; } = null!;
    }

    // DTO para detalles completos del usuario 
    public class UsuarioDetalleDTO
    {
        // Datos básicos del usuario
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; } = null!;
        public string tipo_usuario { get; set; } = null!;
        public string correo_electronico { get; set; } = null!;
        public string? numero_telefono { get; set; }
        public string? imagen_perfil_url { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public bool email_verificado { get; set; }

        // Objeto Anidado para Ciudad y País
        public UbicacionDTO Ubicacion { get; set; } = null!;

        // Perfiles condicionales
        public PerfilArtistaDTO? PerfilArtista { get; set; }
        public PerfilOrganizadorDTO? PerfilOrganizador { get; set; }
    }

    public class UsuarioDetalleResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public UsuarioDetalleDTO Usuario { get; set; }
    }

    public class UsuariosListaResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public IEnumerable<UsuarioDetalleDTO> Usuarios { get; set; }
    }
}
