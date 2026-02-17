using Microsoft.EntityFrameworkCore;
using OrganizarTerritorios.Models;

namespace OrganizarTerritorios.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Pessoa> Pessoas => Set<Pessoa>(); 
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Territorio> Territorios => Set<Territorio>();

    }
}
