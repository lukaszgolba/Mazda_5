using System.Windows;
using System.Windows.Controls;
using mazda_5.Services;




namespace mazda_5.Views.Main
{
    public partial class ModulesView : UserControl
    {
        private readonly ObdService _obd;

        public ModulesView(ObdService obd)
        {
            InitializeComponent();
            _obd = obd;

            ModulesList.ItemsSource = _obd.Modules;
        }

        public static string MapModule(string addr)
        {
            return addr switch
            {
                "7E0" => "PCM",
                "7E8" => "PCM",
                "7D0" => "ABS",
                "7D8" => "ABS",
                "7B0" => "SRS",
                "7A0" => "BCM",
                "720" => "IPC",
                "7C0" => "TCM",
                "7F0" => "GATEWAY",
                _ => $"ECU {addr}"
            };
        }
        private void ScanModules_Click(object sender, RoutedEventArgs e)
        {
            var addrs = _obd.ScanModules();

            ModulesList.Items.Clear();

            foreach (var addr in addrs)
            {
                string module = MapModule(addr);

                ModulesList.Items.Add(new ListBoxItem
                {
                    Content = $"{module} ({addr})"
                });
            }
        }
        private void ModulesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModulesList.SelectedItem is ListBoxItem item)
            {
                string text = item.Content.ToString();

                // wyciągamy adres ECU z "(7E0)"
                string addr = text.Split('(')[1].Replace(")", "");

                var main = (MainWindow)Application.Current.MainWindow;
                main.MainContent.Content = new ModuleDetailView(_obd, addr);
            }
        }
    }
}
