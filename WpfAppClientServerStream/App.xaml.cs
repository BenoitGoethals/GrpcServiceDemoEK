using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using WpfAppClientServerStream.Services;
using WpfAppClientServerStream.Views;

namespace WpfAppClientServerStream
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App :  PrismApplication
    {

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzY0NDIzQDMxMzgyZTMzMmUzMGp3Q1JidFFMK0crbUZzSGYvSUlyalliUTE2K0xranVMdkduUEc3Zll6K2s9");
            
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITempatureService,TempatureService>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }
    }
}
