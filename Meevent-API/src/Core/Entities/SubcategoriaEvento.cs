namespace Meevent_API.src.Core.Entities
{
    public class SubcategoriaEvento
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }

        public int CategoriaEventoId { get; set; }
    }

}
