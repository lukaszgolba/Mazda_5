using mazda_5.Services;
using System.Windows;

namespace mazda_5
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Wczytaj config.json
            AppState.Config = ConfigManager.Load();

           
            var main = new Views.Main.MainWindow();
            main.Show();

            // Przepisz ustawienia do AppState
            if (!string.IsNullOrWhiteSpace(AppState.Config.SelectedPort))
                AppState.SelectedPort = AppState.Config.SelectedPort;

            if (AppState.Config.SelectedBaud > 0)
                AppState.SelectedBaud = AppState.Config.SelectedBaud;
            
           
        }
    }
}
