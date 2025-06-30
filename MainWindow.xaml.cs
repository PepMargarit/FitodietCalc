using FitodietCalc.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitodietCalc;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
        VersionTextBlock.Text = $"v{version} - By PepMG 2025";
    }

    private void Salir_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            // Doble clic: alternar entre maximizado y restaurado
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        else if (e.ButtonState == MouseButtonState.Pressed)
        {
            // Arrastrar ventana
            DragMove();
        }
    }

    private void NuevoPaciente_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new PacienteFormView();
    }
    private void ListadoPacientes_Click(object sender, RoutedEventArgs e)
    {        
        ContentArea.Content = new PacientesListView();
    }

    private void BuscarPaciente_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new BuscarPacienteView();
    }


}
