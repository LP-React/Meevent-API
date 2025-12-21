using System.ComponentModel.DataAnnotations.Schema;

namespace Meevent_API.src.Core.Entities
{
    public class Pais
    {
        [Column("id_pais")]
        public int IdPais { get; set; }

        [Column("nombre_pais")]
        public string NombrePais { get; set; }
        public string CodigoISO { get; set; }
    }
}
