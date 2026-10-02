using Microsoft.EntityFrameworkCore;

namespace InvitacionDigital.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Invitado> Invitados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación de eliminación en cascada: Si se elimina la familia/usuario, se borran sus invitados asociados
            modelBuilder.Entity<Usuario>()
             .HasIndex(u => u.Username)
             .IsUnique();

            // 2. Relación y eliminación en cascada (Usuario -> Invitados)
            modelBuilder.Entity<Invitado>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Invitados)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
