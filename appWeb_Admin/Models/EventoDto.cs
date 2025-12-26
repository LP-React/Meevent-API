namespace appWeb_Admin.Models
{
    public class EventoDto
    {
        public int IdEvento { get; set; }
        public string NombreEvento { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
    }
}
