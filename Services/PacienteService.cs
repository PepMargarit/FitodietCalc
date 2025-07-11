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
        public async Task<bool> PacienteIdYaExisteAsync(int id)
        {
            using var context = new AppDbContext();
            return await context.Pacientes.AnyAsync(p => p.Id == id);
        }
        public async Task ActualizarPacienteAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task<Paciente?> ObtenerPacientePorNombreYApellidosAsync(string nombre, string apellido1, string apellido2)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Nombre == nombre &&
                                          p.Apellido1 == apellido1 &&
                                          p.Apellido2 == apellido2);
        }
        public async Task<Paciente?> ObtenerPacientePorEmailAsync(string email)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Email == email);
        }
        public async Task<Paciente?> ObtenerPacientePorTelefonoAsync(string telefono)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Telefono == telefono);
        }
        public async Task<Paciente?> ObtenerPacientePorIdAsync(int id)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes
                .OrderBy(p => p.Nombre)
                .ThenBy(p => p.Apellido1)
                .ThenBy(p => p.Apellido2)
                .ToListAsync();
        }
        public async Task EliminarPacienteConEvaluacionesAsync(int pacienteId)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Evaluaciones)
                .FirstOrDefaultAsync(p => p.Id == pacienteId);
            if (paciente != null)
            {
                _context.Evaluaciones.RemoveRange(paciente.Evaluaciones); // Elimina evaluaciones primero
                _context.Pacientes.Remove(paciente);                      // Luego el paciente
                await _context.SaveChangesAsync();
            }
        }
    }
}
