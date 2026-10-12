using mazda_5.Services;
using mazda_5.Views.Main;
using System.Windows;
using System.Windows.Controls;


namespace mazda_5.Views
{
    public partial class ModuleDetailView : UserControl
    {
        private readonly ObdService _obd;
        private readonly string _addr;

        public ModuleDetailView(ObdService obd, string addr)
        {
            InitializeComponent();
            _obd = obd;
            _addr = addr;

            ModuleTitle.Text = ModulesView.MapModule(_addr);
            
        }

        private void ResetModule_Click(object sender, RoutedEventArgs e)
        {
            _obd.Send($"ATCRA{_addr}"); // ustawiamy filtr
            _obd.Send("ATSH" + _addr);  // ustawiamy header

            _obd.Send("04"); // reset modułu (UDS: ECU Reset)

            string resp = _obd.Read();
            Output.Text = resp;
        }

        private void ReadParams_Click(object sender, RoutedEventArgs e)
        {
            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);

            _obd.Send("22F190"); // przykład: identyfikacja modułu

            string resp = _obd.Read();
            Output.Text = resp;
        }

        private void SelfTest_Click(object sender, RoutedEventArgs e)
        {
            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);

            _obd.Send("31 01"); // UDS RoutineControl

            string resp = _obd.Read();
            Output.Text = resp;
        }

        private void ActiveTest1_Click(object sender, RoutedEventArgs e)
        {
            RunRoutine("01"); // ABS pump test / PCM fan test / IPC sweep
        }

        private void ActiveTest2_Click(object sender, RoutedEventArgs e)
        {
            RunRoutine("02");
        }

        private void ActiveTest3_Click(object sender, RoutedEventArgs e)
        {
            RunRoutine("03");
        }

        private void RunRoutine(string routineId)
        {
            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);

            _obd.Send($"31 {routineId}");

            string resp = _obd.Read();
            Output.Text = resp;
        }

        private void WriteConfig_Click(object sender, RoutedEventArgs e)
        {
            string did = DidBox.Text.Trim();
            string val = ValueBox.Text.Trim();

            if (string.IsNullOrEmpty(did) || string.IsNullOrEmpty(val))
            {
                Output.Text = "Podaj DID i wartość.";
                return;
            }

            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);

            _obd.Send($"2E {did} {val}");

            string resp = _obd.Read();
            Output.Text = resp;
        }

        private void ReadConfig_Click(object sender, RoutedEventArgs e)
        {
            string did = DidBox.Text.Trim();

            if (string.IsNullOrEmpty(did))
            {
                Output.Text = "Podaj DID.";
                return;
            }

            _obd.Send($"ATCRA{_addr}");
            _obd.Send("ATSH" + _addr);

            _obd.Send($"22 {did}");

            string resp = _obd.Read();
            Output.Text = resp;
        }
        

        
        


    }
}
