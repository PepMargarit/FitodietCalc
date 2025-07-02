using FitodietCalc.Data;
using FitodietCalc.Models;
using FitodietCalc.Services;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for PacienteFormView.xaml
    /// </summary>
    public partial class PacienteFormView : UserControl
    {
        private readonly AppDbContext _dbContext;
        private readonly PacienteService _pacienteService;
        private readonly EvaluacionService _evaluacionService;

        public PacienteFormView()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _pacienteService = new PacienteService(_dbContext);
            _evaluacionService = new EvaluacionService(_dbContext);
        }

        private void EvaluacionExpander_Expanded(object sender, RoutedEventArgs e)
        {
            var paciente = ObtenerPacienteDesdeFormulario();
            if (paciente != null)
            {

                BtnCalcular.Visibility = Visibility.Visible;
                DPFechaEval.SelectedDate = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Por favor, complete los datos del paciente antes de agregar una evaluación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                EvaluacionExpander.IsExpanded = false; // Cierra el expander si no hay datos del paciente
                return;
            }
        }

        private void EvaluacionExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            BtnCalcular.Visibility = Visibility.Collapsed;
            DPFechaEval.SelectedDate = null; 
            TBPeso.Text = string.Empty;
            TBAltura.Text = string.Empty;            
            CBActividad.SelectedIndex = -1; 
        }

        private Paciente? ObtenerPacienteDesdeFormulario()
        {
            if (string.IsNullOrWhiteSpace(TBNombre.Text) ||                
                string.IsNullOrWhiteSpace(TBEmail.Text) ||
                string.IsNullOrWhiteSpace(TBTelefono.Text) ||
                DPFechaNacimiento.SelectedDate == null ||
                CBSexo.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios. *", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (!Helpers.Validations.ValidarTelefono(TBTelefono.Text))
            {
                MessageBox.Show("El teléfono ingresado no es válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            if (!Helpers.Validations.ValidarEmail(TBEmail.Text))
            {
                MessageBox.Show("El email ingresado no es válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            return new Paciente
            {
                Nombre = TBNombre.Text,
                Apellido1 = TBApellido1.Text,
                Apellido2 = TBApellido2.Text,
                Email = TBEmail.Text,
                Telefono = TBTelefono.Text,
                FechaNacimiento = DPFechaNacimiento.SelectedDate ?? DateTime.MinValue,
                Sexo = (CBSexo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty
            };
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {          
            
            var paciente = ObtenerPacienteDesdeFormulario();
            if (paciente == null)
            {
                return; // Si no se pudo obtener el paciente, salimos del método
            }
            bool yaExiste = await _pacienteService.PacienteYaExisteAsync(paciente);
            if (yaExiste && !EvaluacionExpander.IsExpanded)
            {
                MessageBox.Show("Este paciente ya existe en la base de datos.", "Duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {    
                if (EvaluacionExpander.IsExpanded)
                {
                    if (!yaExiste)
                    {
                        await _pacienteService.CrearPacienteAsync(paciente);
                        paciente = await _pacienteService.ObtenerPacientePorEmailAsync(paciente.Email);
                    }

                    double.TryParse(TBPeso.Text, out double peso);
                    double.TryParse(TBAltura.Text, out double altura);


                    var evaluacion = new Evaluacion
                    {
                        PacienteId = paciente.Id,
                        Fecha = (DateTime)DPFechaEval.SelectedDate,
                        PesoKg = peso,
                        AlturaCm = altura,
                    };

                    if (CBActividad.SelectedItem is ComboBoxItem item)
                    {
                        var actividadTexto = item.Content.ToString();
                        if (Enum.TryParse<Evaluacion.ActividadFisica>(actividadTexto, out var actividadParsed))
                            evaluacion.NivelActividad = actividadParsed;
                    }
                    await _evaluacionService.CrearEvaluacionAsync(evaluacion);
                    MessageBox.Show("Paciente y Evaluacion guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (!yaExiste)
                {
                    await _pacienteService.CrearPacienteAsync(paciente);
                    MessageBox.Show("Paciente guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el paciente:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ContentArea.Content = new PacienteFormView();
 
            }
            else
            {
                MessageBox.Show("Error al limpiar el formulario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
