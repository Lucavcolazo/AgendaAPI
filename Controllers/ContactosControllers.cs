using AgendaAPI.Models;
using AgendaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly ContactoService _contactoService;

        public ContactosController(ContactoService contactoService)
        {
            _contactoService = contactoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            var contactos = await _contactoService.GetContactosAsync();
            return Ok(contactos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contacto>> GetContacto(int id)
        {
            var contacto = await _contactoService.GetContactoByIdAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }

            return Ok(contacto);
        }

        [HttpPost]
        public async Task<ActionResult<Contacto>> CreateContacto([FromBody] Contacto nuevoContacto)
        {
            var contacto = await _contactoService.CreateContactoAsync(nuevoContacto);
            return CreatedAtAction(nameof(GetContacto), new { id = contacto.Id }, contacto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContacto(int id, [FromBody] Contacto contactoActualizado)
        {
            var result = await _contactoService.UpdateContactoAsync(id, contactoActualizado);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacto(int id)
        {
            var result = await _contactoService.DeleteContactoAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
