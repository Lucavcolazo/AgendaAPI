using AgendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private static List<Contacto> contactos = DataStore.Contactos; // Referencia a la lista de contactos del DataStore
        

        [HttpGet]
        public ActionResult<IEnumerable<Contacto>> GetContactos() => Ok(contactos);

        [HttpGet("{id}")] // Obtiene un contacto por su id - puede obtener todos
        public ActionResult<Contacto> GetContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null) return NotFound(); // Si no se encuentra el contacto se da error 404
            return Ok(contacto);
        }

        [HttpPost] // Crea un nuevo contacto
        public ActionResult<Contacto> CreateContacto([FromBody] Contacto nuevoContacto)
        {
            
            if (contactos.Any(c => c.Id == nuevoContacto.Id)) // Si el id ya existe se da error 409
            {
                return Conflict("El ID del contacto ya existe.");
            }

            contactos.Add(nuevoContacto);
            return CreatedAtAction(nameof(GetContacto), new { id = nuevoContacto.Id }, nuevoContacto); // Devuelve el contacto creado
        }

        [HttpPut("{id}")] // Actualiza un contacto por su id
        public IActionResult UpdateContacto(int id, Contacto contactoActualizado) 
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null) return NotFound();

            contacto.Nombre = contactoActualizado.Nombre;
            contacto.Apellido = contactoActualizado.Apellido;
            contacto.Email = contactoActualizado.Email;
            contacto.Telefono = contactoActualizado.Telefono;

            return NoContent();
        }

        [Authorize (Roles = "Admin")] // Solo los usuarios con rol "Admin" pueden eliminar contactos
        [HttpDelete("{id}")] // Elimina un contacto por su id
        public IActionResult DeleteContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null) return NotFound();

            contactos.Remove(contacto);
            return NoContent();
        }
    }
}