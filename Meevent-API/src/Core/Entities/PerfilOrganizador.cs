using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{
    [Table("perfiles_organizador")]
    public class PerfilOrganizador
    {

        [Column("id_perfil_organizador")]
        public int IdPerfilOrganizador { get; set; }

        [Column("nombre_organizador")]
        public string NombreOrganizador { get; set; }

        [Column("descripcion_organizador")]
        public string DescripcionOrganizador { get; set; }

        [Column("sitio_web")]
        public string? SitioWeb { get; set; }

        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("facebook_url")]
        public string? FacebookUrl { get; set; }

        [Column("instagram_url")]
        public string? InstagramUrl { get; set; }

        [Column("tiktok_url")]
        public string? TiktokUrl { get; set; }
        [Column("twitter_url")]
        public string? TwitterUrl { get; set; }
        [Column("direccion_organizador")]
        public string? DireccionOrganizador { get; set; }
        [Column("telefono_contacto")]
        public string? TelefonoContacto { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }
    }

    }
