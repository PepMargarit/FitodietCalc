using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitodietCalc.Helpers
{
    public static class Operations
    {
        public static double CalculateIMC(double pesoKg, double alturaCm)
        {
            if (alturaCm <= 0) throw new ArgumentException("La altura debe ser mayor que cero.");
            return Math.Round(pesoKg / Math.Pow(alturaCm / 100, 2), 2);
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
    }
}
