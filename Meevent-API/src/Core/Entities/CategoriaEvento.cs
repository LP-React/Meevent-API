using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{
    [Table("categorias_evento")]
    public class CategoriaEvento
    {
        [Column("id_categoria_evento")]
        public int IdCategoriaEvento { get; set; }

        [Column("nombre_categoria")]
        public string NombreCategoria { get; set; }

        [Column("slug_categoria")]
        public string SlugCategoria { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        public SubcategoriaEvento? IdSubcategoriaEventoNavigation { get; set; }
    }
}
