using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{
    [Table("perfiles_artista")]
    public class PerfilArtista
    {
        [Column("id_perfil_artista")]
        public int IdPerfilArtista { get; set; }

        [Column("nombre_artistico")]
        public string NombreArtistico { get; set; }

        [Column("biografia_artista")]
        public string BiografiaArtista { get; set; }

        [Column("genero_musical")]
        public string GeneroMusical { get; set; }

        [Column("sitio_web")]
        public string? SitioWeb { get; set; }

        [Column("facebook_url")]
        public string? FacebookUrl { get; set; }

        [Column("instagram_url")]
        public string? InstagramUrl { get; set; }

        [Column("tiktok_url")]
        public string? TiktokUrl { get; set; }

        [Column("fecha_creacion")]
        public DateOnly FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        public DateOnly FechaActualizacion { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }
    }
}
