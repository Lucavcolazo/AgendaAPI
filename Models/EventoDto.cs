using System;
public class EventoDto
    {
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; } // Duración en minutos
        public int? ContactoId { get; set; } // Id del contacto asociado al evento
    }