using System.ComponentModel.DataAnnotations;

namespace Meevent_API.src.Features.CategoriasEvento
{

    public class CategoriaEventoDTO
    {
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; }
        public string SlugCategoria { get; set; }
        public bool Estado { get; set; }
    }

    public class CategoriaEventoListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int TotalCategoriasEvento { get; set; }
        public IEnumerable<CategoriaEventoDTO> CategoriasEvento { get; set; }
    }

    public class CategoriaEventoCrearDTO
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "El slug es obligatorio")]
        public string SlugCategoria { get; set; }
    }

    public class CategoriaEventoEditarDTO
    {
        public string? NombreCategoria { get; set; }
        public string? SlugCategoria { get; set; }

        public bool? Estado { get; set; }
    }


    public class CategoriaCambiarEstadoDTO
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool Estado { get; set; }
    }

    public class CategoriaCambiarEstadoResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public bool Estado { get; set; }
    }


    public class CategoriaEventoDetalleDTO
    {
        public int IdCategoriaEvento { get; set; }
        public string NombreCategoria { get; set; }
        public string SlugCategoria { get; set; }
        public bool Estado { get; set; }
    }

 
    public class CategoriaEventoOperacionResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public CategoriaEventoDetalleDTO? Categoria { get; set; }
    }
}
