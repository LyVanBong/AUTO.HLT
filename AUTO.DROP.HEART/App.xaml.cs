using AUTO.DROP.HEART.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace AUTO.DROP.HEART
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
