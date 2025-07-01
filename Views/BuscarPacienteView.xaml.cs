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
    /// Interaction logic for BuscarPacienteView.xaml
    /// </summary>
    public partial class BuscarPacienteView : UserControl
    {
        public BuscarPacienteView()
        {
            InitializeComponent();
            BtnAnteriorEval.Click += BtnAnteriorEval_Click;
            BtnSiguienteEval.Click += BtnSiguienteEval_Click;
            BtnCargar.Click += BtnCargar_Click;
            BtnModificarEval.Click += BtnModificarEval_Click;
        }

        private void BtnAnteriorEval_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para cargar la evaluación anterior
        }

        private void BtnSiguienteEval_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para cargar la evaluación siguiente
        }

        private void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para cargar los datos en otra vista
        }

        private void BtnModificarEval_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para guardar cambios en la evaluación actual
        }

        private void EvaluacionExpander_Expanded(object sender, RoutedEventArgs e)
        {
            BtnModificarEval.Visibility = Visibility.Visible;
        }

        private void EvaluacionExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            BtnModificarEval.Visibility = Visibility.Collapsed;
        }
    }
}
