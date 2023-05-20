using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = Arch.EntityLayer.Entities.File;


namespace Arch.DataAccessLayer.Concrete.Repositories
{
    public class ArchDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ArchDbContext(DbContextOptions<ArchDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Eğer özel modelleme gereksinimleri varsa burada yapabilirsiniz.
            // Örneğin, ilişkileri belirtebilir veya indeksler ekleyebilirsiniz.

            base.OnModelCreating(modelBuilder);
        }
    }
}