using AgendaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaAPI.Services
{
    public class ContactoService
    {
        private readonly ApplicationDbContext _context;

        public ContactoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contacto>> GetContactosAsync()
        {
            return await _context.Contactos.ToListAsync();
        }

        public async Task<Contacto> GetContactoByIdAsync(int id)
        {
            return await _context.Contactos.FindAsync(id);
        }

        public async Task<Contacto> CreateContactoAsync(Contacto nuevoContacto)
        {
            _context.Contactos.Add(nuevoContacto);
            await _context.SaveChangesAsync();
            return nuevoContacto;
        }

        public async Task<bool> UpdateContactoAsync(int id, Contacto contactoActualizado)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return false;
            }

            contacto.Nombre = contactoActualizado.Nombre;
            contacto.Apellido = contactoActualizado.Apellido;
            contacto.Email = contactoActualizado.Email;
            contacto.Telefono = contactoActualizado.Telefono;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContactoAsync(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return false;
            }

            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}