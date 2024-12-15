using AgendaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // DbContext para interactuar con la base de datos

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] // Obtiene todos los eventos
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var eventos = await _context.Eventos.ToListAsync();
            return Ok(eventos);
        }

        [HttpGet("{id}")] // Obtiene un evento por su id
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPost] // Crea un nuevo evento
        public async Task<ActionResult<Evento>> CreateEvento([FromBody] EventoDto nuevoEventoDto)
        {
            var nuevoEvento = new Evento
            {
                Titulo = nuevoEventoDto.Titulo,
                Fecha = nuevoEventoDto.Fecha,
                Hora = nuevoEventoDto.Hora,
                Duracion = nuevoEventoDto.Duracion,
                Lugar = nuevoEventoDto.Lugar,
                ContactoId = nuevoEventoDto.ContactoId
            };

            // Calcular el inicio y fin del nuevo evento
            var nuevoEventoInicio = nuevoEvento.Fecha.Add(nuevoEvento.Hora);
            var nuevoEventoFin = nuevoEventoInicio.AddHours(nuevoEvento.Duracion);

            // Obtener todos los eventos desde la base de datos
            var eventosExistentes = await _context.Eventos.ToListAsync();

            // Verificar si el nuevo evento se superpone con algún evento existente
            var eventoSuperpuesto = eventosExistentes.Any(e =>
                nuevoEventoInicio < e.Fecha.Add(e.Hora).AddHours(e.Duracion) && 
                nuevoEventoFin > e.Fecha.Add(e.Hora)
            );

            if (eventoSuperpuesto)
            {
                return Conflict("El nuevo evento se superpone con un evento existente.");
            }

            // Guardar el nuevo evento en la base de datos
            _context.Eventos.Add(nuevoEvento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = nuevoEvento.Id }, nuevoEvento);
        }


        [HttpPut("{id}")] // Actualiza un evento por su id
        public async Task<IActionResult> UpdateEvento(int id, Evento eventoActualizado)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null) return NotFound();

            evento.Titulo = eventoActualizado.Titulo;
            evento.Fecha = eventoActualizado.Fecha;
            evento.Hora = eventoActualizado.Hora;
            evento.Duracion = eventoActualizado.Duracion;
            evento.Lugar = eventoActualizado.Lugar;
            evento.ContactoId = eventoActualizado.ContactoId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize] // Solo los usuarios autenticados pueden eliminar eventos
        [HttpDelete("{id}")] // Elimina un evento por su id
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null) return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Desde aca estan las rutas para buscar por dia/semanas/mes

        [HttpGet("dia/{fecha}")] // Obtiene los eventos de un día específico (formato YYYY-MM-DD)
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorDia(DateTime fecha)
        {
            var eventosPorDia = await _context.Eventos
                .Where(e => e.Fecha.Date == fecha.Date)
                .ToListAsync();
            return Ok(eventosPorDia);
        }

        [HttpGet("semana/{fecha}")] // Obtiene los eventos de la semana de una fecha específica (formato YYYY-MM-DD)
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorSemana(DateTime fecha)
        {
            var inicioSemana = fecha.Date.AddDays(-(int)fecha.DayOfWeek);
            var finSemana = inicioSemana.AddDays(7);
            var eventosPorSemana = await _context.Eventos
                .Where(e => e.Fecha >= inicioSemana && e.Fecha < finSemana)
                .ToListAsync();
            return Ok(eventosPorSemana);
        }

        [HttpGet("mes/{fecha}")]  // Obtiene los eventos de un mes específico (formato YYYY-MM)
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorMes(DateTime fecha)
        {
            var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
            var finMes = inicioMes.AddMonths(1);
            var eventosPorMes = await _context.Eventos
                .Where(e => e.Fecha >= inicioMes && e.Fecha < finMes)
                .ToListAsync();
            return Ok(eventosPorMes);
        }
    }
}



