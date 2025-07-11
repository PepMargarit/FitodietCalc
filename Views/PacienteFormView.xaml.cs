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
        public enum FormMode
        {
            Crear,
            Buscar
        }

        private readonly FormMode _modo;
        private readonly AppDbContext _dbContext;
        private readonly PacienteService _pacienteService;
        private readonly EvaluacionService _evaluacionService;
        private Paciente _pacienteCargado;
        private List<Evaluacion> evaluacions;
        private int evaluacionActualIndex = 0;

        public PacienteFormView(FormMode mode = FormMode.Crear, Paciente ? pacienteCargado = null)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _pacienteService = new PacienteService(_dbContext);
            _evaluacionService = new EvaluacionService(_dbContext);
            _modo = mode;
            _pacienteCargado = pacienteCargado;
            ConfigurarVistaPorModo();
        }

        private void ConfigurarVistaPorModo()
        {
            if (_modo == FormMode.Crear)
            {
                
                BtnGuardar.Visibility = Visibility.Visible;
                BtnCalcular.Visibility = Visibility.Collapsed;
            }
            else if (_modo == FormMode.Buscar)
            {
                BtnCargar.Visibility = Visibility.Visible;
                BtnModificar.Visibility = Visibility.Visible;
                if (_pacienteCargado != null)
                {
                    CargarDatosPaciente(_pacienteCargado);
                    BtnCalcular.Visibility = Visibility.Visible; 
                }
                else
                {
                    EvaluacionExpander.IsExpanded = false; // Ocultar el expander de evaluación si no hay paciente cargado
                    BtnCalcular.Visibility = Visibility.Collapsed;
                }

            }
            
        }

        private void EvaluacionExpander_Expanded(object sender, RoutedEventArgs e)
        {
            var paciente = ObtenerPacienteDesdeFormulario();
            if (paciente != null)
            {
                if (_modo != FormMode.Crear)
                {
                    BtnSiguiente.Visibility = Visibility.Visible;
                    BtnAnterior.Visibility = Visibility.Visible;
                    BtnCalcular.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnCalcular.Visibility = Visibility.Visible;
                    DPFechaEval.SelectedDate = DateTime.Now;
                }                    
            }
            else
            {
                //MessageBox.Show("Por favor, complete los datos del paciente antes de agregar una evaluación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                EvaluacionExpander.IsExpanded = false; // Cierra el expander si no hay datos del paciente
                return;
            }
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (evaluacionActualIndex > 0)
                MostrarEvaluacion(evaluacionActualIndex - 1);
        }
        private void BtnAnterior_Click(object sender, RoutedEventArgs e) 
        {
            if (evaluacionActualIndex < evaluacions.Count - 1)
                MostrarEvaluacion(evaluacionActualIndex + 1);
        }
        private void EvaluacionExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            
            BtnSiguiente.Visibility = Visibility.Collapsed;
            BtnAnterior.Visibility = Visibility.Collapsed;            
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
            if (!Helpers.Validations.ValidarFechaNacimiento(DPFechaNacimiento.SelectedDate.Value))
            {
                MessageBox.Show("La fecha de nacimiento debe ser una fecha pasada y el paciente debe tener al menos 5 años.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            if (_pacienteCargado != null && _modo == FormMode.Buscar)
            {
                _pacienteCargado.Nombre = TBNombre.Text;
                _pacienteCargado.Apellido1 = TBApellido1.Text;
                _pacienteCargado.Apellido2 = TBApellido2.Text;
                _pacienteCargado.Email = TBEmail.Text;
                _pacienteCargado.Telefono = TBTelefono.Text;
                _pacienteCargado.FechaNacimiento = DPFechaNacimiento.SelectedDate ?? DateTime.MinValue;
                _pacienteCargado.Sexo = (CBSexo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;
                return _pacienteCargado;
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

        private Evaluacion? OptenerEvaluacionDesdeFormulario(Paciente paciente)
        {
            double.TryParse(TBPeso.Text, out double peso);
            double.TryParse(TBAltura.Text, out double altura);

            if (peso == 0 && altura == 0)
            {
                MessageBox.Show("Por favor, ingrese algun campo aguardar.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
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
            return evaluacion;
        }

        /// <summary>
        /// BOTONES
        /// </summary>
        
        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ContentArea.Content = new PacienteFormView(_modo);
            }
            else
            {
                MessageBox.Show("Error al limpiar el formulario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TBNombre.Text) && !string.IsNullOrWhiteSpace(TBApellido1.Text) && !string.IsNullOrWhiteSpace(TBApellido2.Text))
            {
                try
                {
                    _pacienteCargado = await _pacienteService.ObtenerPacientePorNombreYApellidosAsync(TBNombre.Text, TBApellido1.Text, TBApellido2.Text);
                    if (_pacienteCargado != null)
                    {
                        CargarDatosPaciente(_pacienteCargado);                        
                    }
                    else
                    {
                        MessageBox.Show("No se encontró ningún paciente con los datos proporcionados.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar el paciente:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!string.IsNullOrWhiteSpace(TBTelefono.Text) && Helpers.Validations.ValidarTelefono(TBTelefono.Text))
            {
                _pacienteCargado = await _pacienteService.ObtenerPacientePorTelefonoAsync(TBTelefono.Text);                
                if (_pacienteCargado != null)
                {
                    CargarDatosPaciente(_pacienteCargado);                    
                }
                else
                {
                    MessageBox.Show("No se encontró ningún paciente con el teléfono proporcionado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (!string.IsNullOrWhiteSpace(TBEmail.Text) && Helpers.Validations.ValidarEmail(TBEmail.Text))
            {
                _pacienteCargado = await _pacienteService.ObtenerPacientePorEmailAsync(TBEmail.Text);
                if (_pacienteCargado != null)
                {
                    CargarDatosPaciente(_pacienteCargado);                    
                }
                else
                {
                    MessageBox.Show("No se encontró ningún paciente con el email proporcionado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, complete al menos uno de los campos de búsqueda (Nombre y Apellidos, Teléfono o Email).", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarDatosPaciente(Paciente paciente)
        {
            TBNombre.Text = paciente.Nombre;
            TBApellido1.Text = paciente.Apellido1;
            TBApellido2.Text = paciente.Apellido2;
            TBEmail.Text = paciente.Email;
            TBTelefono.Text = paciente.Telefono;
            CBSexo.SelectedItem = CBSexo.Items
            .Cast<ComboBoxItem>()
            .FirstOrDefault(item => item.Content.ToString() == paciente.Sexo);
            DPFechaNacimiento.SelectedDate = paciente.FechaNacimiento;
            EvaluacionExpander.IsExpanded = true;
            // Cargar evaluaciones si existen
            evaluacions = await _evaluacionService.ObtenerEvaluacionesPorPacienteAsync(paciente.Id);
            MostrarEvaluacion(0); // Mostrar la primera evaluación si existe
        }

        private void MostrarEvaluacion(int index)
        {
            if (evaluacions == null || !evaluacions.Any() || index < 0 || index >= evaluacions.Count)
                return;

            var evaluacion = evaluacions[index];
            evaluacionActualIndex = index;

            DPFechaEval.SelectedDate = evaluacion.Fecha;
            TBPeso.Text = evaluacion.PesoKg.ToString();
            TBAltura.Text = evaluacion.AlturaCm.ToString();
            CBActividad.SelectedItem = CBActividad.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == evaluacion.NivelActividad.ToString());
        }


        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var paciente = ObtenerPacienteDesdeFormulario();
                if (paciente == null) return; // Si no se pudo obtener el paciente, salimos del método
                bool yaExiste = await _pacienteService.PacienteYaExisteAsync(paciente);
                if (yaExiste && !EvaluacionExpander.IsExpanded)
                {
                    MessageBox.Show("Este paciente ya existe en la base de datos.", "Duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }               
                if (EvaluacionExpander.IsExpanded)
                {
                    if (!yaExiste)
                    {
                        await _pacienteService.CrearPacienteAsync(paciente);
                        paciente = await _pacienteService.ObtenerPacientePorEmailAsync(paciente.Email);
                    }
                    else
                    {
                        paciente = await _pacienteService.ObtenerPacientePorEmailAsync(paciente.Email);
                    }

                    var evaluacion = OptenerEvaluacionDesdeFormulario(paciente);
                    if (evaluacion == null) return;
                    if (!await _evaluacionService.EvaluacionYaExisteAsync(paciente.Id, evaluacion.Fecha))
                    {
                        await _evaluacionService.CrearEvaluacionAsync(evaluacion);
                        MessageBox.Show("Paciente y Evaluacion guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        evaluacion = await _evaluacionService.ObtenerEvaluacion(paciente.Id, evaluacion.Fecha);
                        await _evaluacionService.ActualizarEvaluacionAsync(evaluacion);
                        MessageBox.Show("Evaluación actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }                    
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

        private async void BtnModificar_Click(object sender, RoutedEventArgs e)
        {            
            if (_pacienteCargado == null) return; // Si no se pudo obtener el paciente, salimos del método          
            _pacienteCargado = ObtenerPacienteDesdeFormulario();

            if (EvaluacionExpander.IsExpanded)
            {
                var evaluacion = OptenerEvaluacionDesdeFormulario(_pacienteCargado);
                if (!await _evaluacionService.EvaluacionYaExisteAsync(_pacienteCargado.Id, evaluacion.Fecha)) 
                {
                    await _pacienteService.ActualizarPacienteAsync(_pacienteCargado);
                    await _evaluacionService.CrearEvaluacionAsync(evaluacion);                    
                }
                else
                {
                    evaluacion = await _evaluacionService.ObtenerEvaluacion(_pacienteCargado.Id, evaluacion.Fecha);
                    evaluacion.PesoKg = evaluacion.PesoKg == 0 ? double.Parse(TBPeso.Text) : evaluacion.PesoKg;
                    evaluacion.AlturaCm = evaluacion.AlturaCm == 0 ? double.Parse(TBAltura.Text) : evaluacion.AlturaCm;
                    evaluacion.NivelActividad = CBActividad.SelectedItem is ComboBoxItem item ?
                        (Evaluacion.ActividadFisica)Enum.Parse(typeof(Evaluacion.ActividadFisica), item.Content.ToString()) :
                        Evaluacion.ActividadFisica.Sedentario;
                    await _pacienteService.ActualizarPacienteAsync(_pacienteCargado);
                    await _evaluacionService.ActualizarEvaluacionAsync(evaluacion);                    
                }
                MessageBox.Show("Paciente y Evaluacion modificados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                await _pacienteService.ActualizarPacienteAsync(_pacienteCargado);
                MessageBox.Show("Paciente modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private async void BtnCalcular_Click(object sender, RoutedEventArgs e)
        {
            var paciente = ObtenerPacienteDesdeFormulario();
            if (paciente == null)
            {
                return; // Si no se pudo obtener el paciente, salimos del método
            }
            bool yaExiste = await _pacienteService.PacienteYaExisteAsync(paciente);
            if (!yaExiste) {
                MessageBox.Show("Por favor, guarde datos antes realizar calculos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            paciente = await _pacienteService.ObtenerPacientePorEmailAsync(paciente.Email);
            var evaluacion = await _evaluacionService.ObtenerEvaluacion(paciente.Id, (DateTime)DPFechaEval.SelectedDate);
            if (!await _evaluacionService.EvaluacionYaExisteAsync(paciente.Id, (DateTime)DPFechaEval.SelectedDate))
            {
                MessageBox.Show("Por favor, guarde datos antes realizar calculos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DPFechaEval.SelectedDate == null || string.IsNullOrWhiteSpace(TBPeso.Text) || string.IsNullOrWhiteSpace(TBAltura.Text) || CBActividad.SelectedItem==null)
            {
                MessageBox.Show("Por favor, complete todos los campos de la evaluación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // ChatGPT Aqui cargar vista Calculos con los datos necesarios Paciente y Evaluacion para realizarlos.
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ContentArea.Content = new ResultadosView(paciente, evaluacion);
            }
        }

        

    }
}
