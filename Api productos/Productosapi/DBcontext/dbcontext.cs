using Microsoft.EntityFrameworkCore;
using Productosapi.Modelos;

namespace Productosapi.DBcontext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
    }
}