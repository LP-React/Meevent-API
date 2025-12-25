namespace appWeb_Admin.Models
{
    public class UsuarioDetalleModel
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? ImagenPerfilUrl { get; set; }
        public bool EmailVerificado { get; set; }
        public bool CuentaActiva { get; set; }

        public string Ciudad { get; set; }
        public string Pais { get; set; }

        public PerfilOrganizadorModel? PerfilOrganizador { get; set; }
    }
}
