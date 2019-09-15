using LisasTours.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LisasTours.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Company { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<BusinessLine> BusinessLines { get; set; }
        public DbSet<CompanyBusinessLine> CompanyBusinessLines { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactType>(entity => entity.HasIndex(e => e.Name).IsUnique());
            modelBuilder.Entity<Region>(entity => entity.HasIndex(e => e.Name).IsUnique());
            modelBuilder.Entity<BusinessLine>(entity => entity.HasIndex(e => e.Name).IsUnique());

            base.OnModelCreating(modelBuilder);
        }
    }
}
