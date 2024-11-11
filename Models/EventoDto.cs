using System;

public class EventoDto
{
    public int Id { get; set; } // Id del evento
    public string Titulo { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; } // Hora del evento
    public int Duracion { get; set; } // Duración en horas
    public string Lugar { get; set; } // Lugar del evento
    public int? ContactoId { get; set; } // Id del contacto asociado al evento - puede ser null
}


// En esta parte esta lo que se pide cuando cargamos o editamos un evento
//Dto hace referencia a Data Transfer Object a diferencia del archivo evento que es el modelo
// el signo ? indica que el campo puede ser null