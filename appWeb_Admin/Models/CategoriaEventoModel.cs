namespace appWeb_Admin.Models
{
    public class CategoriaEventoModel
    {
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; }
        public bool EstaActivo { get; set; }

        public string EstadoTexto => EstaActivo ? "Activo" : "Inactivo";
    }
}
