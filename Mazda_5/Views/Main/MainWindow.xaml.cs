using mazda_5.Services;
using mazda_5.Views.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


namespace mazda_5.Views.Main
{
    public partial class MainWindow : Window
    {

       
        public MainWindow()
        {
            InitializeComponent();
            RefreshMenu();
           
        }
        private void Menu_Connect(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.ConnectionView();
        }

        private void Menu_VIN(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.VinView();
        }

        private void Menu_DTC(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Diagnostics.DtcView(AppState.Obd);
        }

        private void Menu_Live(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.LiveDataView();
        }

        private void Menu_Modules(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.Main.ModulesView(AppState.Obd);
        }

        private void Menu_Settings(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.SettingsView();
        }


        public void RefreshMenu()
        {
            BtnConnect.IsEnabled = true;
            BtnVIN.IsEnabled = AppState.IsConnected;
            BtnDTC.IsEnabled = AppState.IsConnected;
            BtnLive.IsEnabled = AppState.IsConnected;
            BtnModules.IsEnabled = AppState.IsConnected;
            BtnSettings.IsEnabled = true;
        }
        
    }
}
