using System;
using System.Windows;
using System.Windows.Controls;
using mazda_5.Services;

namespace mazda_5.Views
{
    public partial class LiveDataView : UserControl
    {
        public LiveDataView()
        {
            InitializeComponent();
        }

        private void ReadLiveData_Click(object sender, RoutedEventArgs e)
        {
            var diag = AppState.Diag;

            if (diag == null)
            {
                OutputBox.Text = "BŁĄD: Brak połączenia z adapterem.";
                return;
            }

            try
            {
                string rpm = diag.Send("010C");   // RPM
                string speed = diag.Send("010D"); // Speed
                string temp = diag.Send("0105");  // Coolant temp

                OutputBox.Text =
                    $"RPM RAW: {rpm}\n" +
                    $"Speed RAW: {speed}\n" +
                    $"Temp RAW: {temp}\n";
            }
            catch (Exception ex)
            {
                OutputBox.Text = $"BŁĄD odczytu danych Live: {ex.Message}";
            }
        }
    }
}
