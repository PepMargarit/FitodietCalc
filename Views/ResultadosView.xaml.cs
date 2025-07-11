using FitodietCalc.Helpers;
using FitodietCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitodietCalc.Views
{
    /// <summary>
    /// Interaction logic for ResultadosView.xaml
    /// </summary>
    public partial class ResultadosView : UserControl
    {
        private readonly Paciente _paciente;
        private readonly Evaluacion _evaluacion;
        public ResultadosView(Paciente paciente, Evaluacion evaluacion)
        {
            InitializeComponent();
            _paciente = paciente;
            _evaluacion = evaluacion;

            MostrarResultados();
        }

        
        private void MostrarResultados()
        {
            if (_paciente == null || _evaluacion == null) return;

            // Datos básicos
            LblNombre.Text = $"{_paciente.Nombre} {_paciente.Apellido1} {_paciente.Apellido2}";
            LblSexo.Text = _paciente.Sexo;
            LblFechaNacimiento.Text = _paciente.FechaNacimiento.ToShortDateString();
            LblAltura.Text = $"{_evaluacion.AlturaCm} cm";
            LblPeso.Text = $"{_evaluacion.PesoKg} kg";
            LblActividad.Text = _evaluacion.NivelActividad.ToString();

            // Cálculos base
            int edad = Operations.CalcularEdad(_paciente.FechaNacimiento);
            double imc = Operations.CalculateIMC(_evaluacion.PesoKg, _evaluacion.AlturaCm);
            string clasificacion = Operations.GetIMCClassification(imc);
            double grasaCorporal = Operations.CalculateGrasaCorporal(imc, edad, _paciente.Sexo);
            double masaMuscular = Operations.CalculateMasaMuscular(_evaluacion.PesoKg, _evaluacion.AlturaCm, grasaCorporal);

            // TMB por Mifflin-St Jeor
            string formulaMifflin = Operations.ObtenerFormulaMifflin(_evaluacion.PesoKg, _evaluacion.AlturaCm, edad, _paciente.Sexo);
            double tmbMifflin = Operations.CalculateMifflin(_evaluacion.PesoKg, _evaluacion.AlturaCm, edad, _paciente.Sexo);
            double getMifflin = Operations.CalcularGET(tmbMifflin, _evaluacion.NivelActividad);

            // TMB por Harris-Benedict
            string formulaHarris = Operations.ObtenerFormulaHarrisBenedict(_evaluacion.PesoKg, _evaluacion.AlturaCm, edad, _paciente.Sexo);
            double tmbHarris = Operations.CalcularHarrisBenedict(_evaluacion.PesoKg, _evaluacion.AlturaCm, edad, _paciente.Sexo);
            double getHarris = Operations.CalcularGET(tmbHarris, _evaluacion.NivelActividad);

            // Mostrar resultados
            LblIMC.Text = imc.ToString("F2");
            LblClasificacionIMC.Text = clasificacion;
            LblGrasa.Text = $"{grasaCorporal:F2} %";
            LblMasaMuscular.Text = $"{masaMuscular:F2} kg";

            LblFormulaHarris.Text = formulaHarris;
            LblHarris.Text = $"{tmbHarris} kcal";
            LblHarrisGET.Text = $"{getHarris} kcal";

            LblFormulaMifflin.Text = formulaMifflin;
            LblMifflin.Text = $"{tmbMifflin} kcal";
            LblMifflinGET.Text = $"{getMifflin} kcal";
        }
    
    }
}
