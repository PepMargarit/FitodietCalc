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
        public PacienteFormView()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _pacienteService = new PacienteService(_dbContext);
        }

        private void EvaluacionExpander_Expanded(object sender, RoutedEventArgs e)
        {
            BtnCalcular.Visibility = Visibility.Visible;
            DPFechaEval.SelectedDate = DateTime.Now; 
        }

        private void EvaluacionExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            BtnCalcular.Visibility = Visibility.Collapsed;
            DPFechaEval.SelectedDate = null; 
            TBPeso.Text = string.Empty;
            TBAltura.Text = string.Empty;
            TBGrasa.Text = string.Empty;
            TBMasaMuscular.Text = string.Empty;
            CBActividad.SelectedIndex = -1; 
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBNombre.Text) ||                
                string.IsNullOrWhiteSpace(TBEmail.Text) ||
                string.IsNullOrWhiteSpace(TBTelefono.Text) ||
                DPFechaNacimiento.SelectedDate == null ||
                CBSexo.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios. *", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Helpers.Validations.ValidarTelefono(TBTelefono.Text))
            {
                MessageBox.Show("El teléfono ingresado no es válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Helpers.Validations.ValidarEmail(TBEmail.Text))
            {
                MessageBox.Show("El email ingresado no es válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var paciente = new Paciente(
                    nombre: TBNombre.Text,
                    apellido1: TBApellido1.Text,
                    apellido2: TBApellido2.Text,
                    email: TBEmail.Text,
                    telefone: TBTelefono.Text,
                    fechaNacimento: DPFechaNacimiento.SelectedDate ?? DateTime.MinValue,
                    sexo: (CBSexo.SelectedItem as ComboBoxItem)?.Content.ToString()
                );
            try
            {
                bool yaExiste = await _pacienteService.PacienteYaExisteAsync(paciente);
                if (yaExiste && !EvaluacionExpander.IsExpanded)
                {
                    MessageBox.Show("Este paciente ya existe en la base de datos.", "Duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (EvaluacionExpander.IsExpanded)
                {
                    // Aquí puedes agregar la lógica para manejar la evaluación si es necesario
                    // Por ejemplo, podrías crear una nueva evaluación y asociarla al paciente
                }

                await _pacienteService.CrearPacienteAsync(paciente);

                MessageBox.Show("Paciente guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el paciente:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
