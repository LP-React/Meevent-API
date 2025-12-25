using System.ComponentModel.DataAnnotations;

namespace appWeb_Admin.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }

        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Teléfono")]
        public string NumeroTelefono { get; set; }

        [Display(Name = "Estado")]
        public bool CuentaActiva { get; set; }

        [Display(Name = "Tipo de usuario")]
        public string TipoUsuario { get; set; }
    }
}
