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
        public ActionResult<Evento> CreateEvento(Evento nuevoEvento)
        {
            nuevoEvento.Id = nextId++;
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
            evento.Lugar = eventoActualizado.Lugar;

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
}
