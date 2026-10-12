using mazda_5.Services;
using Mazda_5.Services;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mazda_5.Views.Diagnostics
{
    public partial class DtcView : UserControl
    {
        private readonly ObdService _obd;
        private readonly DtcService _dtcService;

        public DtcView(ObdService obd)
        {
            InitializeComponent();
            _obd = obd;
            _dtcService = new DtcService(_obd);
        }

        private void ReadDtc_Click(object sender, RoutedEventArgs e)
        {
            var rawList = _dtcService.ReadDtc();
            var table = new List<DtcRow>();

            foreach (var entry in rawList)
            {
                var parts = entry.Split(" - ");

                if (parts.Length < 2)
                {
                    table.Add(new DtcRow
                    {
                        Code = entry,
                        Description = "",
                        Module = ""
                    });
                    continue;
                }

                string code = parts[0];
                string desc = parts[1];
                string module = GetModule(code);

                table.Add(new DtcRow
                {
                    Code = AddIcon(code),
                    Description = desc,
                    Module = module,
                    Color = GetColor(code)
                });
            }

            DtcList.ItemsSource = table;
        }

        private void ClearDtc_Click(object sender, RoutedEventArgs e)
        {
            bool ok = _dtcService.ClearDtc();

            DtcList.ItemsSource = new List<DtcRow>
            {
                new() {
                    Code = "",
                    Description = ok ? "Błędy zostały skasowane." : "Nie udało się skasować błędów.",
                    Module = "",
                    Color = Brushes.White
                }
            };
        }
        private Brush GetColor(string text)
        {
            if (text.StartsWith("P")) return Brushes.Red;        // PCM
            if (text.StartsWith("C")) return Brushes.Orange;     // ABS
            if (text.StartsWith("B")) return Brushes.Yellow;     // SRS/BCM/IPC
            if (text.StartsWith("U")) return Brushes.DeepSkyBlue;// NETWORK

            return Brushes.White;
        }
        private string AddIcon(string text)
        {
            if (text.StartsWith("P")) return $"🔧 {text}";
            if (text.StartsWith("C")) return $"🚗 {text}";
            if (text.StartsWith("B")) return $"🛡️ {text}";
            if (text.StartsWith("U")) return $"🌐 {text}";

            return text;
        }
        private string GetModule(string code)
        {
            if (code.StartsWith("P")) return "PCM";
            if (code.StartsWith("C")) return "ABS";
            if (code.StartsWith("B")) return "SRS / BCM / IPC";
            if (code.StartsWith("U")) return "NETWORK";

            return "UNKNOWN";
        }
        public class DtcRow
        {
            public string Code { get; set; }
            public string Description { get; set; }
            public string Module { get; set; }
            public Brush Color { get; set; }
        }

    }
}
