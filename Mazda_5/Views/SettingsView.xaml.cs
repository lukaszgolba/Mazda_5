using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using mazda_5.Services;

namespace mazda_5.Views
{
    public partial class SettingsView : UserControl
    {
        private readonly int[] BaudRates = new int[]
        {
            9600, 14400, 19200, 38400, 57600, 115200, 230400, 460800, 921600
        };

        public SettingsView()
        {
            InitializeComponent();
            Loaded += SettingsView_Loaded;
        }

        private void LoadPorts()
        {
            PortList.Items.Clear();
            var ports = SerialPort.GetPortNames();

            foreach (var p in ports)
                PortList.Items.Add(p);

            if (!string.IsNullOrWhiteSpace(AppState.Config.SelectedPort))
                PortList.SelectedItem = AppState.Config.SelectedPort;
            else if (ports.Length > 0)
                PortList.SelectedIndex = 0;
        }

        private void LoadBaudRates()
        {
            BaudList.Items.Clear();
            foreach (var b in BaudRates)
                BaudList.Items.Add(b);
            if (AppState.Config != null && AppState.Config.SelectedBaud > 0)
                BaudList.SelectedItem = AppState.Config.SelectedBaud;  // ustawiamy int
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (PortList.SelectedItem == null || BaudList.SelectedItem == null)
            {
                OutputBox.Text = "Wybierz port i prędkość baud.";
                return;
            }

            AppState.SelectedPort = PortList.SelectedItem.ToString();
            AppState.SelectedBaud = int.Parse(BaudList.SelectedItem.ToString());
            OutputBox.Text = $"Ustawienia zapisane pomyślnie.\nPort: {AppState.SelectedPort}\nPrędkość: {AppState.SelectedBaud}";

            AppState.Config.SelectedPort = AppState.SelectedPort;
            AppState.Config.SelectedBaud = AppState.SelectedBaud;

            ConfigManager.Save(AppState.Config);
        }
        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPorts();
            LoadBaudRates();
            OutputBox.Text = $"Zapisane ustawienia.\nPort: {AppState.Config.SelectedPort}\nPrędkość: {AppState.Config.SelectedBaud}";
        }

    }
}
