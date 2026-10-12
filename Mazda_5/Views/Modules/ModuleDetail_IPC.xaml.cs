using mazda_5.Services;
using System.Windows;
using System.Windows.Controls;

namespace mazda_5.Views.Modules
{
    public partial class ModuleDetail_IPC : UserControl
    {
        private readonly ObdService _obd;
        private readonly string _addr;

        public ModuleDetail_IPC(ObdService obd, string addr)
        {
            InitializeComponent();
            _obd = obd;
            _addr = addr;
        }

        private void GaugeSweep_Click(object sender, RoutedEventArgs e)
        {
            RunRoutine("03");
        }

        private void ReadParams_Click(object sender, RoutedEventArgs e)
        {
            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);
            _obd.Send("22 F101"); // język
            Output.Text = _obd.Read();
        }

        private void RunRoutine(string id)
        {
            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);
            _obd.Send($"31 {id}");
            Output.Text = _obd.Read();
        }

        
        private void WriteConfig_Click(object sender, RoutedEventArgs e)
        {
            // 1. Walidacja pól
            if (string.IsNullOrWhiteSpace(DidBox.Text) || string.IsNullOrWhiteSpace(ValueBox.Text))
            {
                MessageBox.Show("Uzupełnij pola DID oraz Wartość.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Potwierdzenie operacji
            var confirm = MessageBox.Show(
                $"Czy na pewno chcesz zapisać konfigurację?\n\n" +
                $"Adres modułu: {_addr}\n" +
                $"DID: {DidBox.Text}\n" +
                $"Wartość: {ValueBox.Text}\n\n" +
                $"Komenda: 2E {DidBox.Text} {ValueBox.Text}",
                "Potwierdzenie zapisu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                Output.Text = "Zapis anulowany przez użytkownika.";
                return;
            }

            try
            {
                // 3. Wysyłanie komend
                _obd.Send($"ATCRA{_addr}");
                _obd.Send("ATSH" + _addr);
                _obd.Send($"2E {DidBox.Text} {ValueBox.Text}");

                // 4. Odczyt odpowiedzi
                var response = _obd.Read();
                Output.Text = response;

                // 5. Informacja o sukcesie
                MessageBox.Show(
                    $"Konfiguracja została wysłana.\n\nOdpowiedź ECU:\n{response}",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // 6. Obsługa błędów
                MessageBox.Show(
                    $"Wystąpił błąd podczas zapisu:\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Output.Text = $"BŁĄD: {ex.Message}";
            }
        }

    }

}
