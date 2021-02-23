using AUTO.HLT.MOBILE.VIP.ViewModels;
using AUTO.HLT.MOBILE.VIP.ViewModels.Home;
using AUTO.HLT.MOBILE.VIP.ViewModels.Login;
using AUTO.HLT.MOBILE.VIP.Views;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using AUTO.HLT.MOBILE.VIP.Views.Login;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
        }
    }
}
