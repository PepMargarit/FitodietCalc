using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitodietCalc.Models;

namespace FitodietCalc.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=FitodietCalc.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>()
                        .HasMany<Evaluacion>(p => p.Evaluaciones)
                        .WithOne(e => e.Paciente)
                        .HasForeignKey(e => e.PacienteId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
