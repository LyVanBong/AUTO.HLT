using AUTO.HLT.ADMIN.Views.Main;
using Prism.Ioc;
using System.Windows;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Services.RequestProvider;
using AUTO.HLT.ADMIN.Services.RestSharp;
using AUTO.HLT.ADMIN.Views.Login;
using AUTOHLT.MOBILE.Services.RestSharp;

namespace AUTO.HLT.ADMIN
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
            containerRegistry.Register<IFacebookService, FacebookeService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
        }
    }
}
