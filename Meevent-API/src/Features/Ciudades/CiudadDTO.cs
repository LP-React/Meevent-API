using Meevent_API.src.Features.Paises;

public class CiudadDTO
{
    public int IdCiudad { get; set; }
    public string NombreCiudad { get; set; } = string.Empty;
}

public class CiudadListResponseDTO
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int TotalCiudades { get; set; }
    public List<CiudadDTO> Ciudades { get; set; } = new List<CiudadDTO>();
}