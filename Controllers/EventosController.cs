using AgendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private static List<Evento> eventos = new List<Evento>();
        private static List<Contacto> contactos = DataStore.Contactos; // Referencia a la lista de contactos del DataStore
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<Evento>> GetEventos() => Ok(eventos);

        [HttpGet("{id}")]
        public ActionResult<Evento> GetEvento(int id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPost]
        public ActionResult<Evento> CreateEvento([FromBody] EventoDto nuevoEventoDto)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == nuevoEventoDto.ContactoId);

            var nuevoEvento = new Evento
            {
                Id = nextId++,
                Titulo = nuevoEventoDto.Titulo,
                Fecha = nuevoEventoDto.Fecha,
                Duracion = nuevoEventoDto.Duracion,
                Contacto = contacto // Asocia el contacto al evento, puede ser null
            };

            eventos.Add(nuevoEvento);

            return CreatedAtAction(nameof(GetEvento), new { id = nuevoEvento.Id }, nuevoEvento);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvento(int id, Evento eventoActualizado)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null) return NotFound();

            evento.Titulo = eventoActualizado.Titulo;
            evento.Fecha = eventoActualizado.Fecha;
            evento.Duracion = eventoActualizado.Duracion;
            evento.Contacto = eventoActualizado.Contacto;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvento(int id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null) return NotFound();

            eventos.Remove(evento);
            return NoContent();
        }
    }

    public class EventoDto
    {
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
        public int ContactoId { get; set; } // Solo el ID del contacto
    }
}