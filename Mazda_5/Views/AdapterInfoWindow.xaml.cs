using System.Windows;
using mazda_5.Services;

namespace mazda_5.Views
{
    public partial class AdapterInfoWindow : Window
    {
        public AdapterInfoWindow()
        {
            InitializeComponent();
            LoadInfo();
        }

        private void LoadInfo()
        {
            if (AppState.Diag == null)
            {
                ElmBox.Text = "Brak połączenia.";
                StnBox.Text = "Brak połączenia.";
                VlinkerBox.Text = "Brak połączenia.";
                return;
            }

            // --- 1. ELM ---
            ElmBox.Text =
                $"ATI: {AppState.Diag.Send("ATI")}\n" +
                $"Firmware: {AppState.Diag.Send("AT@1")}\n" +
                $"Serial: {AppState.Diag.Send("AT@2")}\n" +
                $"Protocol: {AppState.Diag.Send("ATDP")}\n";

            // --- 2. STN/MIC ---
            var stn = new StnCommands(AppState.Diag);
            StnBox.Text =
                $"STDI: {stn.GetDeviceInfo()}\n" +
                $"STVER: {stn.GetFirmwareVersion()}\n" +
                $"STMFR: {stn.GetManufacturerInfo()}\n" +
                $"STMA: {stn.GetAdapterCapabilities()}\n" +
                $"STI: {stn.GetInterfaceInfo()}\n" +
                $"STSLCS: {stn.GetCanSpeed()}\n";

            // --- 3. VLinker FS ---
            AppState.Vlinker = new VlinkerFsDetector(AppState.Diag);
            AppState.VlinkerInfo = AppState.Vlinker.Detect();
            var i = AppState.VlinkerInfo;

            VlinkerBox.Text =
                $"Wykryto: {(i.IsVlinkerFs ? "TAK" : "NIE")}\n\n" +
                $"Urządzenie: {i.Device}\n" +
                $"Producent: {i.Manufacturer}\n" +
                $"Firmware: {i.FirmwareFinal}\n\n" +
                $"ELM ATI: {i.Ati}\n" +
                $"ELM Firmware: {i.FirmwareElm}\n" +
                $"ELM Serial: {i.SerialElm}\n" +
                $"Protokół: {i.Protocol}\n\n" +
                $"STMA: {i.Capabilities}\n" +
                $"STI: {i.Interface}\n" +
                $"CAN Mode: {i.CanSpeed}\n";

            
        }
    }
}
