using mazda_5.Services;
using mazda_5.Views.Main;
using mazda_5;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mazda_5.Views
{
    public partial class ConnectionView : UserControl
    {
        private ObdService _obd;

        public ConnectionView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                ConnectButton.IsEnabled = AppState.Diag == null;
                DisconnectButton.IsEnabled = AppState.Diag != null;
                RefreshConnectionStatus();
            };

        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AppState.SelectedPort))
                {
                    return;
                }

                int baud = AppState.SelectedBaud == 0 ? 115200 : AppState.SelectedBaud;

                AppState.Diag = new VehicleDiagnosticClient(AppState.SelectedPort, baud);
                AppState.Diag.Open();

                _obd = new ObdService(AppState.Diag);
                AppState.Obd = _obd;

                AppState.Stn = new StnCommands(AppState.Diag);

                // --- PANEL STATUSU ---
                Status_Port.Text = $"Port: {AppState.SelectedPort}";
                Status_Baud.Text = $"Prędkość: {baud}";

                string adapterName = AppState.Diag.Send("STDI");
                Status_Adapter.Text = $"Adapter: {adapterName}";


                Status_State.Text = "Stan: Połączony";
                Status_State.Foreground = Brushes.Green;

                ConnectButton.IsEnabled = false;
                DisconnectButton.IsEnabled = true;
                AppState.IsConnected = true;
                ((MainWindow)Application.Current.MainWindow).RefreshMenu();

            }
            catch (Exception ex)
            {
                Status_State.Text = $"Błąd: {ex.Message}";
                Status_State.Foreground = Brushes.Red;
            }
                  
               
            
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppState.Diag != null)
                {
                    AppState.Diag.Dispose();
                    AppState.Diag = null;
                    AppState.Obd = null;

                    Status_Port.Text = "Port: -";
                    Status_Baud.Text = "Prędkość: -";
                    Status_Adapter.Text = "Adapter: -";
                    
                    Status_State.Text = "Stan: Rozłączony";
                    Status_State.Foreground = Brushes.Red;

                    ConnectButton.IsEnabled = true;
                    DisconnectButton.IsEnabled = false;
                    AppState.IsConnected = false;
                    ((MainWindow)Application.Current.MainWindow).RefreshMenu();
                }

            }
            catch (Exception)
            {

            }
            
        }
        
        private void OpenAdapterInfo_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentAdapterWindow != null)
            {
                AppState.CurrentAdapterWindow.Activate();
                return;
            }
            

            var win = new AdapterInfoWindow();
            AppState.CurrentAdapterWindow = win;

            win.Closed += (s, args) =>
            {
                AppState.CurrentAdapterWindow = null;
            };

            win.Show();
        }

        private void RefreshConnectionStatus()
        {
            bool connected = AppState.Diag != null;

            Status_Port.Text = connected ? $"Port: {AppState.SelectedPort}" : "Port: -";
            Status_Baud.Text = connected ? $"Prędkość: {AppState.SelectedBaud}" : "Prędkość: -";
            Status_Adapter.Text = connected ? "Adapter: Połączono" : "Adapter: -";
            Status_State.Text = connected ? "Stan: Połączony" : "Stan: Rozłączony";
            Status_State.Foreground = connected ? Brushes.Green : Brushes.Red;

            ConnectButton.IsEnabled = !connected;
            DisconnectButton.IsEnabled = connected;
        }



    }
}
