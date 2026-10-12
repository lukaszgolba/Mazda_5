using System;
using mazda_5.Services;

namespace mazda_5.Services
{
    public class VlinkerFsDetector
    {
        private readonly VehicleDiagnosticClient _diag;
        private readonly StnCommands _stn;

        public VlinkerFsDetector(VehicleDiagnosticClient diag)
        {
            _diag = diag;
            _stn = new StnCommands(diag);
        }

        public VlinkerFsInfo Detect()
        {
            var info = new VlinkerFsInfo();

            // --- ELM BASIC ---
            info.Ati = _diag.Send("ATI");
            info.FirmwareElm = _diag.Send("AT@1");
            info.SerialElm = _diag.Send("AT@2");
            info.Protocol = _diag.Send("ATDP");

            // --- STN/MIC ---
            info.Device = _stn.GetDeviceInfo();
            info.FirmwareStn = _stn.GetFirmwareVersion();
            info.Manufacturer = _stn.GetManufacturerInfo();
            info.Capabilities = _stn.GetAdapterCapabilities();
            info.Interface = _stn.GetInterfaceInfo();
            info.CanSpeed = _stn.GetCanSpeed();

            // --- HEURISTICS (FORScan-style) ---
            info.IsVlinkerFs = DetectVlinker(info);
            info.FirmwareFinal = DetectFirmware(info);

            return info;
        }

        private bool DetectVlinker(VlinkerFsInfo i)
        {
            if (i.Device.Contains("vLinker", StringComparison.OrdinalIgnoreCase)) return true;
            if (i.Ati.Contains("vLinker", StringComparison.OrdinalIgnoreCase)) return true;
            if (i.Manufacturer.Contains("Vgate", StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        private string DetectFirmware(VlinkerFsInfo i)
        {
            // STVER działa tylko częściowo → często zwraca ?
            if (!string.IsNullOrWhiteSpace(i.FirmwareStn) && i.FirmwareStn != "?")
                return i.FirmwareStn;

            // FORScan heurystyka: wyciąga wersję z ATI
            if (i.Ati.Contains("v2.3", StringComparison.OrdinalIgnoreCase))
                return "V2.3.04";

            // fallback
            return "Nieznane / ograniczone STN";
        }
    }

    public class VlinkerFsInfo
    {
        // ELM
        public string Ati;
        public string FirmwareElm;
        public string SerialElm;
        public string Protocol;

        // STN/MIC
        public string Device;
        public string FirmwareStn;
        public string Manufacturer;
        public string Capabilities;
        public string Interface;
        public string CanSpeed;

        // Heurystyka
        public bool IsVlinkerFs;
        public string FirmwareFinal;
    }
}
