using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Arch.EntityLayer.Entities.Auth.Authorization;
using File = Arch.EntityLayer.Entities.File;

namespace Arch.DataAccessLayer.Concrete.Repositories
{
    public class ArchDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ArchDbContext(DbContextOptions options) : base(options)
        {
        }



        public DbSet<Competition> Competitions { get; set; }
        public DbSet<File> Files { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



    }
}
