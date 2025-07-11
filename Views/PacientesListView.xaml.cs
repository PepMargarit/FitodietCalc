using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FitodietCalc.Data;
using FitodietCalc.Models;
using FitodietCalc.Services;
using Microsoft.EntityFrameworkCore;
using static FitodietCalc.Views.PacienteFormView;

namespace FitodietCalc.Views
{
    public partial class PacientesListView : UserControl
    {
        private readonly AppDbContext _dbContext;
        private readonly PacienteService _pacienteService;
        private List<Paciente> _todosLosPacientes;

        public PacientesListView()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _pacienteService = new PacienteService(_dbContext);
            CargarPacientes();
            NombreBusquedaTextBox.TextChanged += NombreBusquedaTextBox_TextChanged;
            CargarDatosButton.Click += CargarDatosButton_Click;
        }

        private async void CargarPacientes()
        {
            _todosLosPacientes = await _pacienteService.ObtenerPacientesAsync();
            PacientesDataGrid.ItemsSource = _todosLosPacientes;
        }
        private void NombreBusquedaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = NombreBusquedaTextBox.Text.Trim().ToLower();

            var filtrados = _todosLosPacientes
                .Where(p => p.Nombre.ToLower().Contains(filtro))
                .ToList();

            PacientesDataGrid.ItemsSource = filtrados;
        }
        private void PacientesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PacientesDataGrid.SelectedItem is Paciente seleccionado)
                NavegarAPacienteForm(seleccionado);
        }
        private void CargarDatosButton_Click(object sender, RoutedEventArgs e)
        {
            if (PacientesDataGrid.SelectedItem is Paciente seleccionado)
                NavegarAPacienteForm(seleccionado);
            else
                MessageBox.Show("Seleccione un paciente para continuar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void NavegarAPacienteForm(Paciente paciente)
        {
            var vista = new PacienteFormView(FormMode.Buscar, paciente);
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ContentArea.Content = vista;
            }            
        }

        private async void EliminarPacienteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PacientesDataGrid.SelectedItem is Paciente paciente)
            {
                var resultado = MessageBox.Show(
                    $"¿Seguro que quieres eliminar al paciente {paciente.Nombre} {paciente.Apellido1} {paciente.Apellido2}? Esto tambien eliminara sus evaluaciones.",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _pacienteService.EliminarPacienteConEvaluacionesAsync(paciente.Id);
                        MessageBox.Show("Paciente eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarPacientes(); // Refresca la tabla
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar paciente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un paciente para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}