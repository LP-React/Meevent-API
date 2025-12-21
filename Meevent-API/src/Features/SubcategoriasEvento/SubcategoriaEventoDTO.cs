namespace Meevent_API.src.Features.SubcategoriasEvento
{
    public class SubcategoriaEventoDTO
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }
        public int CategoriaEventoId { get; set; }
        public string? NombreCategoria { get; set; }
    }
}
