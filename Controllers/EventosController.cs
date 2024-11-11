using AgendaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class EventosController : ControllerBase
    {
        private static List<Evento> eventos = new List<Evento>(); // Lista de eventos de prueba
        private static List<Contacto> contactos = DataStore.Contactos; // Referencia a la lista de contactos del DataStore

        [HttpGet] // Obtiene todos los eventos
        public ActionResult<IEnumerable<Evento>> GetEventos() => Ok(eventos);

        [HttpGet("{id}")] // Obtiene un evento por su id
        public ActionResult<Evento> GetEvento(int id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPost] // Crea un nuevo evento
        public ActionResult<Evento> CreateEvento([FromBody] EventoDto nuevoEventoDto)
        {
            var nuevoEvento = new Evento
            {
                Id = eventos.Count + 1,
                Titulo = nuevoEventoDto.Titulo,
                Fecha = nuevoEventoDto.Fecha,
                Hora = nuevoEventoDto.Hora,
                Duracion = nuevoEventoDto.Duracion,
                Lugar = nuevoEventoDto.Lugar,
                ContactoId = nuevoEventoDto.ContactoId
            };

            // Controlar la superposición de eventos
            var nuevoEventoInicio = nuevoEvento.Fecha.Add(nuevoEvento.Hora);
            var nuevoEventoFin = nuevoEventoInicio.AddHours(nuevoEvento.Duracion);

            var eventoSuperpuesto = eventos.Any(e =>
            {
                var eventoInicio = e.Fecha.Add(e.Hora);
                var eventoFin = eventoInicio.AddHours(e.Duracion);
                return nuevoEventoInicio < eventoFin && nuevoEventoFin > eventoInicio;
            });

            if (eventoSuperpuesto)
            {
                return Conflict("El nuevo evento se superpone con un evento existente.");
            }

            eventos.Add(nuevoEvento);

            return CreatedAtAction(nameof(GetEvento), new { id = nuevoEvento.Id }, nuevoEvento);
        }

        [HttpPut("{id}")] // Actualiza un evento por su id
        public IActionResult UpdateEvento(int id, Evento eventoActualizado)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id); // Busca el evento por su id
            if (evento == null) return NotFound();

            evento.Titulo = eventoActualizado.Titulo;
            evento.Fecha = eventoActualizado.Fecha;
            evento.Hora = eventoActualizado.Hora;
            evento.Duracion = eventoActualizado.Duracion;
            evento.Lugar = eventoActualizado.Lugar;
            evento.ContactoId = eventoActualizado.ContactoId;

            return NoContent();
        }

        [Authorize] // Solo los usuarios autenticados pueden eliminar eventos
        [HttpDelete("{id}")] // Elimina un evento por su id
        public IActionResult DeleteEvento(int id)
        {
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null) return NotFound();

            eventos.Remove(evento);
            return NoContent();
        }

        // Desde aca estan las rutas para buscar por dia/semanas/mes

        [HttpGet("dia/{fecha}")] // Obtiene los eventos de un día específico (formato YYYY-MM-DD)
        public ActionResult<IEnumerable<Evento>> GetEventosPorDia(DateTime fecha)
        {
            var eventosPorDia = eventos.Where(e => e.Fecha.Date == fecha.Date).ToList();
            return Ok(eventosPorDia);
        }

        [HttpGet("semana/{fecha}")] // Obtiene los eventos de la semana de una fecha específica (formato YYYY-MM-DD)
        public ActionResult<IEnumerable<Evento>> GetEventosPorSemana(DateTime fecha)
        {
            var inicioSemana = fecha.Date.AddDays(-(int)fecha.DayOfWeek);
            var finSemana = inicioSemana.AddDays(7);
            var eventosPorSemana = eventos.Where(e => e.Fecha >= inicioSemana && e.Fecha < finSemana).ToList();
            return Ok(eventosPorSemana);
        }

        [HttpGet("mes/{fecha}")]  // Obtiene los eventos de un mes específico (formato YYYY-MM)
        public ActionResult<IEnumerable<Evento>> GetEventosPorMes(DateTime fecha)
        {
            var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
            var finMes = inicioMes.AddMonths(1);
            var eventosPorMes = eventos.Where(e => e.Fecha >= inicioMes && e.Fecha < finMes).ToList();
            return Ok(eventosPorMes);
        }
    }
}

