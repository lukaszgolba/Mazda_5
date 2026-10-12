using mazda_5.Services;
using Mazda_5.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace mazda_5.Views
{
    public partial class VinView : UserControl
    {
        public VinView()
        {
            InitializeComponent();
           
        }

        private void ReadVin_Click(object sender, RoutedEventArgs e)
        {
            var diag = AppState.Diag;

            if (diag == null)
            {
                OutputBox.Text = "BŁĄD: Brak połączenia z adapterem.";
                return;
            }

            try
            {
                // VIN PID: 0902
                string raw = diag.Send("0902");
                string vin = ExtractVin(raw);

                var info = VinDecoder.Decode(vin);

                if (info.Error != null)
                {
                    OutputBox.Text = info.Error;
                    return;
                }

                OutputBox.Text =
                    $"VIN: {info.VIN}\n" +
                    $"Producent: {info.Manufacturer}\n" +
                    $"Model: {info.Model}\n" +
                    $"Model Year: {info.ModelYear}\n" +
                    $"VDS: {info.VehicleDescriptor}\n" +
                    $"VIS: {info.VehicleIdentifier}";

                
            }
            catch (Exception ex)
            {
                OutputBox.Text = $"BŁĄD odczytu VIN: {ex.Message}";
            }
        }

         private static string ExtractVin(string raw)
         {
             // typowa odpowiedź:
             // 0: 49 02 01 4A 4D 31 5F 46 31 32 33 34 35 36 37 38 39
             // VIN = JM1_F123456789

             raw = raw.Replace(" ", "").Replace("\r", "").Replace("\n", "");

             // usuń nagłówki 490201
             int idx = raw.IndexOf("490201");
             if (idx >= 0)
                 raw = raw[(idx + 6)..];

             // konwersja hex → ASCII
             string vin = "";
             for (int i = 0; i < raw.Length; i += 2)
             {
                 string hex = raw.Substring(i, 2);
                 int val = Convert.ToInt32(hex, 16);
                 vin += (char)val;
             }

             return vin.Trim();
         }
        
       /* private static string ExtractVin(string raw)
        {
            // usuń znaki sterujące
            raw = raw.Replace("\r", "").Replace("\n", "").Trim();

            // rozbij na linie
            var lines = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> vinBytes = new();

            // przejście przez wszystkie bajty
            for (int i = 0; i < lines.Length; i++)
            {
                // szukamy nagłówków 49 02 xx
                if (lines[i] == "49" && lines[i + 1] == "02")
                {
                    // numer bloku (01,02,03,04)
                    int block = Convert.ToInt32(lines[i + 2], 16);

                    // dane zaczynają się od i+3
                    int start = i + 3;

                    // każdy blok ma max 7 bajtów VIN
                    for (int j = start; j < start + 7 && j < lines.Length; j++)
                    {
                        vinBytes.Add(lines[j]);
                    }
                }
            }

            // konwersja HEX -> ASCII
            string vin = string.Concat(vinBytes.Select(b => (char)Convert.ToInt32(b, 16)));

            return vin.Trim();
        }
       */



    }
}
