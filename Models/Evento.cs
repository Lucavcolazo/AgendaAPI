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
    public List<int> ParticipantesIds { get; set; } = new List<int>();
    }
}
