using Meevent_API.src.Features.Paises;

public class CiudadDTO
{
    public int IdCiudad { get; set; }
    public string NombreCiudad { get; set; }
    public int IdPais { get; set; }

    public PaisDTO? jsonPais { get; set; }

}