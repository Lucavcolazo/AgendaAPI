using System.Collections.Generic;

namespace AgendaAPI.Models
{
    public static class DataStore
    {
        public static List<Contacto> Contactos { get; set; } = new List<Contacto>
        {
            new Contacto { Id = 1, Nombre = "Juan", Apellido = "Perez", Email = "juan.perez@example.com", Telefono = "123456789" },
            new Contacto { Id = 2, Nombre = "Maria", Apellido = "Gomez", Email = "maria.gomez@example.com", Telefono = "987654321" }
        };
    }
}