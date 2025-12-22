using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{
    [Table("subcategorias_evento")]
    public class SubcategoriaEvento
    {
        [Column("id_subcategoria_evento")]
        public int IdSubcategoriaEvento { get; set; }

        [Column("nombre_subcategoria")]
        public string NombreSubcategoria { get; set; } 

        [Column("slug_subcategoria")]
        public string SlugSubcategoria { get; set; }

        [Column("categoria_evento_id")]
        public int CategoriaEventoId { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }
    }

}
