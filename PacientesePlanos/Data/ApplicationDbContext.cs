using Microsoft.EntityFrameworkCore;
using PacientesePlanos.Model;
using System.Collections.Generic;


namespace PacientesePlanos.Data;

public class ApplicationDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Plano> Planos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         {
        modelBuilder.Entity<Paciente>()
            .HasMany(p => p.Planos)
            .WithMany(p => p.Pacientes)
            .UsingEntity<Dictionary<string, object>>(
                "PacientePlano",  // Nome da tabela de junção
                j => j
                    .HasOne<Plano>()
                    .WithMany()
                    .HasForeignKey("PlanoId"),  // Chave estrangeira
                j => j
                    .HasOne<Paciente>()
                    .WithMany()
                    .HasForeignKey("PacienteId")  // Chave estrangeira
            );
    }
    }
    
    }
