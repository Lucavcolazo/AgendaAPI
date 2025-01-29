using AgendaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaAPI.Services
{
    public class EventoService
    {
        private readonly ApplicationDbContext _context;

        public EventoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> GetEventosAsync()
        {
            return await _context.Eventos.ToListAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int id)
        {
            return await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evento> CreateEventoAsync(EventoDto nuevoEventoDto)
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

            var nuevoEventoInicio = nuevoEvento.Fecha.Add(nuevoEvento.Hora);
            var nuevoEventoFin = nuevoEventoInicio.AddHours(nuevoEvento.Duracion);

            var eventosExistentes = await _context.Eventos.ToListAsync();

            var eventoSuperpuesto = eventosExistentes.Any(e =>
                nuevoEventoInicio < e.Fecha.Add(e.Hora).AddHours(e.Duracion) &&
                nuevoEventoFin > e.Fecha.Add(e.Hora)
            );

            if (eventoSuperpuesto)
            {
                return null;
            }

            _context.Eventos.Add(nuevoEvento);
            await _context.SaveChangesAsync();
            return nuevoEvento;
        }

        public async Task<bool> UpdateEventoAsync(int id, Evento eventoActualizado)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null)
            {
                return false;
            }

            evento.Titulo = eventoActualizado.Titulo;
            evento.Fecha = eventoActualizado.Fecha;
            evento.Hora = eventoActualizado.Hora;
            evento.Duracion = eventoActualizado.Duracion;
            evento.Lugar = eventoActualizado.Lugar;
            evento.ContactoId = eventoActualizado.ContactoId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventoAsync(int id)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null)
            {
                return false;
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Evento>> GetEventosPorDiaAsync(DateTime fecha)
        {
            return await _context.Eventos
                .Where(e => e.Fecha.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evento>> GetEventosPorSemanaAsync(DateTime fecha)
        {
            var inicioSemana = fecha.Date.AddDays(-(int)fecha.DayOfWeek);
            var finSemana = inicioSemana.AddDays(7);
            return await _context.Eventos
                .Where(e => e.Fecha >= inicioSemana && e.Fecha < finSemana)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evento>> GetEventosPorMesAsync(DateTime fecha)
        {
            var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
            var finMes = inicioMes.AddMonths(1);
            return await _context.Eventos
                .Where(e => e.Fecha >= inicioMes && e.Fecha < finMes)
                .ToListAsync();
        }
    }
}