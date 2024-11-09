namespace AgendaAPI.Models
{
    public class Contacto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}

// En esta parte esta lo que se pide cuando cargamos o editamos un contacto