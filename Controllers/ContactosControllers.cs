using AgendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AgendaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private static List<Contacto> contactos = new List<Contacto>();
        private static int nextId = 1;

        // GET: api/Contactos
        [HttpGet]
        public ActionResult<IEnumerable<Contacto>> GetContactos()
        {
            return Ok(contactos);
        }

        // GET: api/Contactos/{id}
        [HttpGet("{id}")]
        public ActionResult<Contacto> GetContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null)
                return NotFound();

            return Ok(contacto);
        }

        // POST: api/Contactos
        [HttpPost]
        public ActionResult<Contacto> CreateContacto(Contacto contacto)
        {
            contacto.Id = nextId++;
            contactos.Add(contacto);
            return CreatedAtAction(nameof(GetContacto), new { id = contacto.Id }, contacto);
        }

        // PUT: api/Contactos/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateContacto(int id, Contacto updatedContacto)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null)
                return NotFound();

            contacto.Nombre = updatedContacto.Nombre;
            contacto.Apellido = updatedContacto.Apellido;
            contacto.Telefono = updatedContacto.Telefono;
            contacto.Email = updatedContacto.Email;

            return NoContent();
        }

        // DELETE: api/Contactos/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteContacto(int id)
        {
            var contacto = contactos.FirstOrDefault(c => c.Id == id);
            if (contacto == null)
                return NotFound();

            contactos.Remove(contacto);
            return NoContent();
        }
    }
}


