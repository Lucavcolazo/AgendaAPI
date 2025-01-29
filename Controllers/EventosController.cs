using AgendaAPI.Models;
using AgendaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly EventoService _eventoService;

        public EventosController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var eventos = await _eventoService.GetEventosAsync();
            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        [HttpPost]
        public async Task<ActionResult<Evento>> CreateEvento([FromBody] EventoDto nuevoEventoDto)
        {
            var evento = await _eventoService.CreateEventoAsync(nuevoEventoDto);
            if (evento == null)
            {
                return Conflict("El nuevo evento se superpone con un evento existente.");
            }

            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] Evento eventoActualizado)
        {
            var result = await _eventoService.UpdateEventoAsync(id, eventoActualizado);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var result = await _eventoService.DeleteEventoAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("dia/{fecha}")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorDia(DateTime fecha)
        {
            var eventos = await _eventoService.GetEventosPorDiaAsync(fecha);
            return Ok(eventos);
        }

        [HttpGet("semana/{fecha}")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorSemana(DateTime fecha)
        {
            var eventos = await _eventoService.GetEventosPorSemanaAsync(fecha);
            return Ok(eventos);
        }

        [HttpGet("mes/{fecha}")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventosPorMes(DateTime fecha)
        {
            var eventos = await _eventoService.GetEventosPorMesAsync(fecha);
            return Ok(eventos);
        }
    }
}
