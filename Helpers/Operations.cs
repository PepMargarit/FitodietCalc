using FitodietCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitodietCalc.Helpers
{
    public static class Operations
    {
        public static int CalcularEdad(DateTime nacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - nacimiento.Year;
            if (nacimiento > hoy.AddYears(-edad)) edad--;
            return edad;
        }
        public static double CalculateIMC(double pesoKg, double alturaCm)
        {
            if (alturaCm <= 0) throw new ArgumentException("La altura debe ser mayor que cero.");
            return Math.Round(pesoKg / Math.Pow(alturaCm / 100, 2), 2);
        }
        public static string GetIMCClassification(double imc)
        {
            if (imc < 18.5) return "Bajo peso";
            else if (imc >= 18.5 && imc < 24.9) return "Peso normal";
            else if (imc >= 25 && imc < 29.9) return "Sobrepeso";
            else return "Obesidad";
        }
        public static double CalculateGrasaCorporal(double imc, int edad, string sexo)
        {
            if (sexo == "Hombre")
            {
                return Math.Round((1.20 * imc) + (0.23 * edad) - 16.2, 2);
            }
            else if (sexo == "Mujer")
            {
                return Math.Round((1.20 * imc) + (0.23 * edad) - 5.4, 2);
            }
            else
            {
                throw new ArgumentException("Sexo no válido. Debe ser 'Hombre' o 'Mujer'.");
            }
        }
        public static double CalculateMasaMuscular(double pesoKg, double alturaCm, double grasaCorporal)
        {            
            return Math.Round(pesoKg * (1 - (grasaCorporal / 100)), 2);
        }
        public static double CalculateTMB(double pesoKg, double alturaCm, int edad, string sexo)
        {
            if (sexo == "Hombre")
            {
                return Math.Round(10 * pesoKg + 6.25 * alturaCm - 5 * edad + 5, 2);
            }
            else if (sexo == "Mujer")
            {
                return Math.Round(10 * pesoKg + 6.25 * alturaCm - 5 * edad - 161, 2);
            }
            else
            {
                throw new ArgumentException("Sexo no válido. Debe ser 'Hombre' o 'Mujer'.");
            }
        }
        public static double CalcularHarrisBenedict(double pesoKg, double alturaCm, int edad, string sexo)
        {
            if (sexo == "Hombre")
            {
                // GEB = 66.5 + (13.75 × peso) + (5.003 × altura) − (6.755 × edad)
                return Math.Round(66.5 + (13.75 * pesoKg) + (5.003 * alturaCm) - (6.755 * edad), 2);
            }
            else if (sexo == "Mujer")
            {
                // GEB = 655.1 + (9.563 × peso) + (1.850 × altura) − (4.676 × edad)
                return Math.Round(655.1 + (9.563 * pesoKg) + (1.850 * alturaCm) - (4.676 * edad), 2);
            }
            else
            {
                throw new ArgumentException("Sexo no válido. Debe ser 'Hombre' o 'Mujer'.");
            }
        }

        public static string ObtenerFormulaHarrisBenedict(double pesoKg, double alturaCm, int edad, string sexo)
        {
            if (sexo == "Hombre")
            {
                return $"GEB = 66.5 + (13.75 × {pesoKg}) + (5.003 × {alturaCm}) − (6.755 × {edad})";
            }
            else if (sexo == "Mujer")
            {
                return $"GEB = 655.1 + (9.563 × {pesoKg}) + (1.850 × {alturaCm}) − (4.676 × {edad})";
            }
            else
            {
                throw new ArgumentException("Sexo no válido. Debe ser 'Hombre' o 'Mujer'.");
            }
        }
        public static double GetFactorActividadFisica(Evaluacion.ActividadFisica? nivel)
        {
            return nivel switch
            {
                Evaluacion.ActividadFisica.Sedentario => 1.2,
                Evaluacion.ActividadFisica.Ligera => 1.375,
                Evaluacion.ActividadFisica.Moderada => 1.55,
                Evaluacion.ActividadFisica.Activa => 1.725,
                Evaluacion.ActividadFisica.Intensa => 1.9,
                _ => 1.0 // Si no se especifica, usamos un valor neutro (evita errores)
            };
        }

        public static double CalcularGET(double tmb, Evaluacion.ActividadFisica? nivel)
        {
            double faf = GetFactorActividadFisica(nivel);
            return Math.Round(tmb * faf, 2);
        }

        public static string ObtenerFormulaMifflin(double pesoKg, double alturaCm, int edad, string sexo)
        {
            string sexoPart = sexo == "Hombre" ? "+ 5" : "− 161";
            return $"GEB = (10 × {pesoKg}) + (6.25 × {alturaCm}) − (5 × {edad}) {sexoPart}";
        }
    }
}
