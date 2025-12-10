namespace Meevent_API.src.Core.Entities
{
    // Meevent_API.src.Core.Entities/Usuario.cs
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Meevent_API.src.Core.Entities
    {
        public class Usuario
        {
            [Column("id_usuario")]
            public int IdUsuario { get; set; }

            [Column("nombre_completo")]
            public string NombreCompleto { get; set; }

            [Column("correo_electronico")]
            public string CorreoElectronico { get; set; }

            [Column("contrasena_hash")]
            public string ContrasenaHash { get; set; }

            [Column("numero_telefono")]
            public string? NumeroTelefono { get; set; }

            [Column("imagen_perfil_url")]
            public string? ImagenPerfilUrl { get; set; }

            [Column("fecha_nacimiento")]
            public DateTime? FechaNacimiento { get; set; }

            [Column("fecha_creacion")]
            public DateTime FechaCreacion { get; set; }

            [Column("fecha_actualizacion")]
            public DateTime FechaActualizacion { get; set; }

            [Column("email_verificado")]
            public bool EmailVerificado { get; set; }

            [Column("cuenta_activa")]
            public bool CuentaActiva { get; set; }

            [Column("tipo_usuario")]
            public string TipoUsuario { get; set; } = "normal";
        }
    }
}