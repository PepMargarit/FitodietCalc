using FitodietCalc.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FitodietCalc;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
        }

        // Aquí puedes iniciar la ventana principal
    }
}

