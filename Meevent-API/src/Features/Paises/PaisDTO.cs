namespace Meevent_API.src.Features.Paises
{
    // DTO para listar países (solo datos necesarios)
    public class PaisDTO
    {
        public int IdPais { get; set; }
        public string NombrePais { get; set; }


    }

    // DTO para respuestas con metadata adicional
    public class PaisListResponseDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int Total_Paises { get; set; }
        public IEnumerable<PaisDTO> Paises { get; set; }
    }
}
