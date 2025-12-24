namespace appWeb_Admin.Models
{
    public class SubcategoriaModel
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; }
        public bool EstaActivo { get; set; }
    }
}
