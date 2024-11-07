using AgendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private static List<Contacto> contactos = DataStore.Contactos;
        private static int nextId = 3;

        [HttpGet]
        public ActionResult<IEnumerable<Contacto>> GetContactos() => Ok(contactos);

        [HttpGet("{id}")]
        public ActionResult<Contacto> GetContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null) return NotFound();
            return Ok(contacto);
        }

        [HttpPost]
        public ActionResult<Contacto> CreateContacto(Contacto nuevoContacto)
        {
            nuevoContacto.Id = nextId++;
            contactos.Add(nuevoContacto);
            return CreatedAtAction(nameof(GetContacto), new { id = nuevoContacto.Id }, nuevoContacto);
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public IActionResult DeleteContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null) return NotFound();

            contactos.Remove(contacto);
            return NoContent();
        }
    }
}