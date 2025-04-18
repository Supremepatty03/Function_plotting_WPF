using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

namespace Lab3_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Lab3_WPF.Properties.Settings.Default.ShowWelcomeScreen)
            {
                var welcome = new MainWindow(); // приветствие
                welcome.Show();
            }
            else
            {
                var navWindow = new NavigationWindow();
                navWindow.Source = new Uri("Page2.xaml", UriKind.Relative);
                navWindow.Show();
            }
        }
    }

}
