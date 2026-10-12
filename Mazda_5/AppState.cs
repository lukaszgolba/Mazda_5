using mazda_5.Services;
using mazda_5.Views;

namespace mazda_5
{
    public static class AppState
    {
        public static AppConfig Config { get; set; } = new AppConfig();

        public static string SelectedPort { get; set; }
        public static int SelectedBaud { get; set; }
        public static VehicleDiagnosticClient Diag;
        public static ObdService Obd;
     
        public static StnCommands Stn;
        public static VlinkerFsDetector Vlinker;
        public static VlinkerFsInfo VlinkerInfo;
        public static string LastSpeedTestResults = "";
        public static AdapterInfoWindow CurrentAdapterWindow = null;
       
        public static bool IsConnected { get; set; } = false;

        public static bool IsSimulation { get; set; } = false;
        


    }
}