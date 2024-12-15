using AgendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor que recibe ApplicationDbContext
        public ContactosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/contactos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            // Obtener todos los contactos de la base de datos
            return Ok(await _context.Contactos.ToListAsync());
        }

        // GET: api/contactos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacto>> GetContacto(int id)
        {
            // Buscar el contacto por su id
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null)
            {
                return NotFound(); // Si no se encuentra, devolver 404
            }

            return Ok(contacto); // Devolver el contacto encontrado
        }

        // POST: api/contactos
        [HttpPost]
        public async Task<ActionResult<Contacto>> CreateContacto([FromBody] Contacto nuevoContacto)
        {
            if (nuevoContacto == null)
            {
                return BadRequest(); // Si el contacto no es válido, devolver BadRequest
            }

            // Agregar el nuevo contacto al DbContext
            _context.Contactos.Add(nuevoContacto);
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

            return CreatedAtAction(nameof(GetContacto), new { id = nuevoContacto.Id }, nuevoContacto); // Devolver el contacto creado
        }

        // PUT: api/contactos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContacto(int id, Contacto contactoActualizado)
        {
            // Buscar el contacto en la base de datos
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return NotFound(); // Si no se encuentra, devolver 404
            }

            // Actualizar los datos del contacto
            contacto.Nombre = contactoActualizado.Nombre;
            contacto.Apellido = contactoActualizado.Apellido;
            contacto.Email = contactoActualizado.Email;
            contacto.Telefono = contactoActualizado.Telefono;

            await _context.SaveChangesAsync(); // Guardar los cambios

            return NoContent(); // Devolver 204 No Content cuando la actualización sea exitosa
        }

        // DELETE: api/contactos/{id}
        [Authorize] 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacto(int id)
        {
            // Buscar el contacto en la base de datos
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null)
            {
                return NotFound(); // Si no se encuentra, devolver 404
            }

            // Eliminar el contacto
            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync(); // Guardar los cambios

            return NoContent(); // Devolver 204 No Content cuando la eliminación sea exitosa
        }
    }
}
