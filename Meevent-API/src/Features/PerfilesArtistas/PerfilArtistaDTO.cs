namespace Meevent_API.src.Features.PerfilesArtistas
{
    public class PerfilArtistaDTO
    {
        public int id_perfil_artista { get; set; }
        public string nombre_artistico { get; set; }
        public string biografia_artista { get; set; }
        public string genero_musical { get; set; }
        public string? sitio_web { get; set; }
        public string? facebook_url { get; set; }
        public string? instagram_url { get; set; }
        public string? tiktok_url { get; set; }
        public DateOnly fecha_creacion { get; set; }
        public DateOnly fecha_actualizacion { get; set; }
        public int usuario_id { get; set; }
    }

}
