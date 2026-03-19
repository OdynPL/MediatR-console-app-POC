using Microsoft.EntityFrameworkCore;
using PersonManager.Domain;

namespace PersonManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            // One-to-Many: Person -> Address
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Address)
                .WithMany(a => a.Residents)
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-One: Person -> Company
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many: Person <-> Project
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Projects)
                .WithMany(pr => pr.Members)
                .UsingEntity(j => j.ToTable("PersonProjects"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app.db");
            }
        }
    }
}
