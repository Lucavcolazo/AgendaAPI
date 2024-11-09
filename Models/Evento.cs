namespace AgendaAPI.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
        public Contacto Contacto { get; set; }
    }
}

// En esta parte esta lo que se pide cuando cargamos o editamos un evento