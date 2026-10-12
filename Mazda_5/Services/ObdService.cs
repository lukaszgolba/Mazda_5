

namespace mazda_5.Services
{
    public class ObdService
    {
        private readonly VehicleDiagnosticClient _diag;

        public List<string> Modules { get; private set; } = new();

        public ObdService(VehicleDiagnosticClient diag)
        {
            _diag = diag;
        }

        public string GetVinRaw() => _diag.Send("0902");
        public string GetDtcRaw() => _diag.Send("03");
        public string ClearDtcRaw() => _diag.Send("04");

        public string Read()
        {
            return _diag.Read();
        }
        public string Send(string command)
        {
            
            return _diag.Send(command);
        }
        public List<string> ScanModules()
        {
            // STMA – skan magistrali CAN (STN / VLinker FS)
            _diag.Send("STMA");

            string response = _diag.Read();   

            var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var modules = new List<string>();

            foreach (var line in lines)
            {
                string addr = line.Trim().Replace(" ", "");

                // adresy ECU mają 3 znaki (np. 7E0, 7D0, 7B0)
                if (addr.Length == 3)
                    modules.Add(addr);
            }

            Modules = modules;

            return modules;
        }
        
    }


}
