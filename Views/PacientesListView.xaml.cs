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
    /// Interaction logic for PacientesListView.xaml
    /// </summary>
    public partial class PacientesListView : UserControl
    {
        public PacientesListView()
        {
            InitializeComponent();

            // Asociar eventos aquí si no están en XAML
            NombreBusquedaTextBox.TextChanged += NombreBusquedaTextBox_TextChanged;
            CargarDatosButton.Click += CargarDatosButton_Click;
            PacientesDataGrid.MouseDoubleClick += PacientesDataGrid_MouseDoubleClick;
        }

        private void NombreBusquedaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Aquí irá la lógica para filtrar la lista en tiempo real
        }

        private void CargarDatosButton_Click(object sender, RoutedEventArgs e)
        {
            // Aquí la lógica para cargar los datos del paciente seleccionado
        }

        private void PacientesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Aquí la lógica para cargar los datos al hacer doble clic en un paciente
        }
    }
}

