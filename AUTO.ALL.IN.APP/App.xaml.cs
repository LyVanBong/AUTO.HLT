using AUTO.ALL.IN.APP.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace AUTO.ALL.IN.APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDM2OTI3QDMxMzkyZTMxMmUzMFEyNW45ZER4VUtmVlBwSnpMcTBDbEZjV3ArM0dwSVFkbEZyTmZzNDdOdjg9");
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
