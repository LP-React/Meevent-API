namespace Meevent_API.src.Features.Locales
{
    public class LocalDTO
    {
        public int IdLocal { get; set; }
        public string NombreLocal { get; set; }
        public int CapacidadLocal { get; set; }
        public string DireccionLocal { get; set; }
        public int CiudadId { get; set; }
        public string SlugLocal { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

    public class LocalListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int TotalLocales { get; set; }
        public List<LocalDTO> Locales { get; set; } = new List<LocalDTO>();
    }
}
