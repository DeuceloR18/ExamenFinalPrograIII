using Microsoft.EntityFrameworkCore;
using ApiNetguardgt.Models;

namespace ApiNetguardgt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Sitio> Sitios { get; set; }
        public DbSet<Incidente> Incidentes { get; set; }
        public DbSet<HistorialIncidente> HistorialIncidentes { get; set; }
    }
}