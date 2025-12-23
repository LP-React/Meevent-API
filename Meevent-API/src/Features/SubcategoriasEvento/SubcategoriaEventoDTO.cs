using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.SubcategoriasEvento
{
    public class SubcategoriaEventoDTO
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }
        public int CategoriaEventoId { get; set; }
        public bool Estado { get; set; } 
    }

    public class SubcategoriaEventoListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalSubCategoriasEvento { get; set; }
        public IEnumerable<SubcategoriaEventoDTO> SubcategoriaEventos { get; set; }
    }

    public class SubcategoriaEventoCrearDTO
    {
        [Required(ErrorMessage = "El nombre de la subcategoría es obligatorio")]
        public string NombreSubcategoria { get; set; }

        [Required(ErrorMessage = "El slug es obligatorio")]
        public string SlugSubcategoria { get; set; }

        [Required(ErrorMessage = "El ID de la categoría padre es obligatorio")]
        public int CategoriaEventoId { get; set; }
    }

    public class SubcategoriaEventoEditarDTO
    {
        public string? NombreSubcategoria { get; set; }
        public string? SlugSubcategoria { get; set; }
        public int? CategoriaEventoId { get; set; }
    }

    public class SubcategoriaCambiarEstadoDTO
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool Estado { get; set; }
    }

    public class SubcategoriaCambiarEstadoResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public bool Estado { get; set; }
    }

    public class SubcategoriaEventoDetalleDTO
    {
        public int IdSubcategoriaEvento { get; set; }
        public string NombreSubcategoria { get; set; }
        public string SlugSubcategoria { get; set; }
        public int CategoriaEventoId { get; set; }
        public bool Estado { get; set; }
    }

    public class SubcategoriaEventoOperacionResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public SubcategoriaEventoDetalleDTO? Subcategoria { get; set; }
    }
}