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
        public DbSet<DesignerUser> DesignerUsers { get; set; }
        public DbSet<ProjectFilePath> ProjectFilePaths { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BlogPost>()
            .HasOne(b => b.Competition)
            .WithMany(c => c.BlogPosts)
            .HasForeignKey(b => b.CompetitionId)
            .OnDelete(DeleteBehavior.NoAction); // Kısıtlamayı ekleyin

            modelBuilder.Entity<DesignerUser>()
            .HasOne(b => b.Competition)
            .WithMany(c => c.DesignerUsers)
            .HasForeignKey(b => b.CompetitionId)
            .OnDelete(DeleteBehavior.NoAction); // Kısıtlamayı ekleyin

            //modelBuilder.Entity<ContestEntry>()
            //.HasOne(e => e.Competition)
            //.WithMany(c => c.ContestEntries)
            //.HasForeignKey(e => e.CompetitionId)
            //.OnDelete(DeleteBehavior.Cascade); // Yarışma silindiğinde katılımcılar da silinecek



            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



    }
}
