using Microsoft.EntityFrameworkCore;
using PruebaTecnicaMultiMoney.Models;

namespace PruebaTecnicaMultiMoney.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cuota>()
                .HasOne(c => c.Prestamo)
                .WithMany(p => p.Cuotas)
                .HasForeignKey(c => c.PrestamoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}