﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitodietCalc.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; } = string.Empty; // Default value to avoid null
        public string Apellido2 { get; set; } = string.Empty; // Default value to avoid null
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }

        // Relación 1 a muchos
        public List<Evaluacion> Evaluaciones { get; set; } = new();

        // Constructor por defecto para EF
        public Paciente() { }

        public Paciente(string nombre, string apellido1, string apellido2, string email, string telefone, DateTime fechaNacimento, string sexo)
        {
            Nombre = nombre;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            Email = email;
            Telefono = telefone;
            FechaNacimiento = fechaNacimento;
            Sexo = sexo;            
            
        }

        
    }
}
