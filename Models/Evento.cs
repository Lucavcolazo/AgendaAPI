using System;
using System.Collections.Generic;

namespace AgendaAPI.Models
{
    public class Evento
    {
    public int Id { get; set; }
    public string Titulo { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Duracion { get; set; }
    public string Lugar { get; set; }
    public List<Contacto> Participantes { get; set; } = new List<Contacto>();
    //Ver si usamos linq para que se linkee derecho con la lista de contactos :D
    }
}
