using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgendaAPI.Models;


namespace AgendaAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Evento> Eventos { get; set; }
    }
}
