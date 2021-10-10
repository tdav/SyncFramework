using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using System.Diagnostics;
using Application = Microsoft.Maui.Controls.Application;

namespace SyncFrameworkApp
{
    public partial class App : Application
    {
        public App()
        {
            
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
