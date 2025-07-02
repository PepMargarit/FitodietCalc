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
    public class EvaluacionService
    {
        private readonly AppDbContext _context;
        public EvaluacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CrearEvaluacionAsync(Evaluacion evaluacion)
        {
            await _context.Evaluaciones.AddAsync(evaluacion);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Evaluacion>> ObtenerEvaluacionesPorPacienteAsync(int pacienteId)
        {
            return await _context.Evaluaciones
                .Where(e => e.PacienteId == pacienteId)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
        }
        public async Task<Evaluacion> ObtenerUltimaEvaluacionAsync(int pacienteId)
        {
            return await _context.Evaluaciones
                .Where(e => e.PacienteId == pacienteId)
                .OrderByDescending(e => e.Fecha)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> EvaluacionYaExisteAsync(int pacienteId, DateTime fecha)
        {
            return await _context.Evaluaciones
                .AnyAsync(e => e.PacienteId == pacienteId && e.Fecha.Date == fecha.Date);
        }
        
    }
}
