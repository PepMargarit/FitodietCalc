using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitodietCalc.Data;
using FitodietCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace FitodietCalc.Services
{
    public class PacienteService
    {
        private readonly AppDbContext _context;

        public PacienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CrearPacienteAsync(Paciente paciente)
        {
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();            
        }

        public async Task<bool> PacienteYaExisteAsync(Paciente paciente)
        {
            using var context = new AppDbContext();

            return await context.Pacientes.AnyAsync(p =>
                (p.Nombre == paciente.Nombre &&
                p.Apellido1 == paciente.Apellido1 &&
                p.Apellido2 == paciente.Apellido2 &&
                p.FechaNacimiento.Date == paciente.FechaNacimiento.Date) 
                ||
                p.Telefono == paciente.Telefono
                ||
                p.Email == paciente.Email);
        }
    }
}
