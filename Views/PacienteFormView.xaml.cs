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
        public PacienteFormView()
        {
            InitializeComponent();

        }

        private void EvaluacionExpander_Expanded(object sender, RoutedEventArgs e)
        {
            BtnCalcular.Visibility = Visibility.Visible;
        }

        private void EvaluacionExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            BtnCalcular.Visibility = Visibility.Collapsed;
        }


    }
}
