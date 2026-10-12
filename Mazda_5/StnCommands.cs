using System;
using mazda_5.Services;

namespace mazda_5.Services
{
    public class StnCommands
    {
        private readonly VehicleDiagnosticClient _diag;

        public StnCommands(VehicleDiagnosticClient diag)
        {
            _diag = diag;
        }

        // Informacje o urządzeniu (pełna nazwa)
        public string GetDeviceInfo()
        {
            return _diag.Send("STDI");
        }

        // Wersja firmware
        public string GetFirmwareVersion()
        {
            return _diag.Send("STVER");
        }

        // Informacje o producencie
        public string GetManufacturerInfo()
        {
            return _diag.Send("STMFR");
        }

        // Możliwości adaptera (HS/MS CAN, ISO15765, auto-switch)
        public string GetAdapterCapabilities()
        {
            return _diag.Send("STMA");
        }

        // Informacje o interfejsie (ELM, bufory, UART)
        public string GetInterfaceInfo()
        {
            return _diag.Send("STI");
        }

        // Przełączenie na HS-CAN
        public string SwitchHsCan()
        {
            return _diag.Send("STCSWH");
        }

        // Przełączenie na MS-CAN
        public string SwitchMsCan()
        {
            return _diag.Send("STCSWM");
        }

        // Odczyt aktualnej prędkości CAN
        public string GetCanSpeed()
        {
            return _diag.Send("STSLCS");
        }
    }
}
