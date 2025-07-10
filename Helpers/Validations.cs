using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitodietCalc.Helpers
{
    public static class Validations
    {
        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static bool ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            var pattern = @"^[\d\+\-\(\)\s]{7,15}$";
            return Regex.IsMatch(telefono, pattern);
        }
        public static bool ValidarFechaNacimiento(DateTime fechaNacimiento)
        {
            // Verifica que la fecha de nacimiento no sea futura y que tenga al menos 5 años
            return fechaNacimiento <= DateTime.Now && (DateTime.Now.Year - fechaNacimiento.Year) >= 5;
        }


    }
}
