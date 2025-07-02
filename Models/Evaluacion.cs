using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitodietCalc.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public required int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public double PesoKg { get; set; }
        public double AlturaCm { get; set; }   
        public ActividadFisica? NivelActividad { get; set; }
        public enum ActividadFisica
        {
            Sedentario,
            Ligera,
            Moderada,
            Activa,
            Intensa
        }    

    }
}
