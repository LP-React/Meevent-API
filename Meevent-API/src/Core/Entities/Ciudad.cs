using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{

    [Table("ciudades")]
    public class Ciudad
    {
        [Column("id_ciudad")]
        public int IdCiudad { get; set; }
        [Column("nombre_ciudad")]
        public string NombreCiudad { get; set; }
        [Column("id_pais")]
        public int IdPais { get; set; }

    }
}
