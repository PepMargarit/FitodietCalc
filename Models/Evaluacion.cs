using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitodietCalc.Models
{
    class Evaluacion
    {
        public int Id { get; set; }
        public required string PacienteId { get; set; }
        public DateTime Fecha { get; set; }
        public double PesoKg { get; set; }
        public double AlturaCm { get; set; }
        public double IMC { get; set; }
        public double GrasaCorporal { get; set; } = 0;
        public double MasaMuscular { get; set; } = 0;
        
        public enum ActividadFisica
        {
            Sedentario,
            Moderado,
            Activo,
            MuyActivo

        }

        private double CalcularIMC()
        {
            return PesoKg / (AlturaCm * AlturaCm);
        }

    }
}
