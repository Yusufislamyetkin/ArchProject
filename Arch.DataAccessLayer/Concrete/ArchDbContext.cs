using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Arch.EntityLayer.Entities.Auth.Authorization;


namespace Arch.DataAccessLayer.Concrete.Repositories
{
    public class ArchDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ArchDbContext(DbContextOptions options) : base(options)
        {
        }



        public DbSet<Competition> Competitions { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BlogPost>()
            .HasOne(b => b.Competition)
            .WithMany(c => c.BlogPosts)
            .HasForeignKey(b => b.CompetitionId)
            .OnDelete(DeleteBehavior.NoAction); // Kısıtlamayı ekleyin

            //modelBuilder.Entity<Competition>()
            //    .HasOne(c => c.Designers)
            //    .WithMany()
            //    .HasForeignKey(c => c.DesignerId)
            //    .OnDelete(DeleteBehavior.Restrict); // Kısıtlamayı ekleyin

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



    }
}
