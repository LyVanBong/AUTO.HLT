using AUTO.HLT.SELENIUM.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace AUTO.HLT.SELENIUM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
