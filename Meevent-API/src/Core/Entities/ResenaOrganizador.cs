namespace Meevent_API.src.Core.Entities
{
    public class ResenaOrganizador
    {
        public int IdResenaOrganizador { get; set; }
        public int CalificacionResena { get; set; }
        public string ComentarioResena { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int ContadorUtilidad { get; set; }
        public bool CompradorVerificado { get; set; }
        public int PerfilOrganizadorId { get; set; }
        public int UsuarioId { get; set; }

        // Datos adicionales del usuario (para listar)
        public string NombreCompleto { get; set; }
        public string ImagenPerfilUrl { get; set; }
    }
}
