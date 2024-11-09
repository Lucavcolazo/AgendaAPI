using System.Collections.Generic;

namespace AgendaAPI.Models
{
    public static class DataStore
    {
        public static List<Contacto> Contactos { get; set; } = new List<Contacto> // Lista de contactos de prueba
        {
            new Contacto { Id = 1, Nombre = "Juan", Apellido = "Perez", Email = "juan.perez@example.com", Telefono = "123456789" },
            new Contacto { Id = 2, Nombre = "Maria", Apellido = "Gomez", Email = "maria.gomez@example.com", Telefono = "987654321" },
            new Contacto { Id = 3, Nombre = "Pedro", Apellido = "Gonzalez", Email = "Pedro.Gonzales@example.com", Telefono = "194353450" },
            new Contacto { Id = 4, Nombre = "Ana", Apellido = "Martinez", Email = "Ana.Martinez@example.com", Telefono = "075415781" },
            new Contacto { Id = 5, Nombre = "Lucia", Apellido = "Rodriguez", Email = "Lucia.Rodriguez@example.com", Telefono = "197437653" }
        };
    }
}