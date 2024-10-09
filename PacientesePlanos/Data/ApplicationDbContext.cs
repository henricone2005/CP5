using Microsoft.EntityFrameworkCore;
using PacientesePlanos.Model;

namespace PacientesePlanos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Plano> Planos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurando a relação muitos-para-muitos entre Paciente e Plano
            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.Planos)
                .WithMany(p => p.Pacientes)
                .UsingEntity<Dictionary<string, object>>(
                    "PacientePlano", // Nome da tabela de junção
                    j => j
                        .HasOne<Plano>()
                        .WithMany()
                        .HasForeignKey("PlanoId"), // Chave estrangeira
                    j => j
                        .HasOne<Paciente>()
                        .WithMany()
                        .HasForeignKey("PacienteId"), // Chave estrangeira
                    j =>
                    {
                        j.HasKey("PacienteId", "PlanoId"); // Define a chave primária
                    }
                );
        }
    }
}
