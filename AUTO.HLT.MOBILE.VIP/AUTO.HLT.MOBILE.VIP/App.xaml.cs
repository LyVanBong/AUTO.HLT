using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using AUTO.HLT.MOBILE.VIP.Services.RestSharp;
using AUTO.HLT.MOBILE.VIP.ViewModels.Home;
using AUTO.HLT.MOBILE.VIP.ViewModels.Login;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using AUTO.HLT.MOBILE.VIP.Views.Login;
using AUTOHLT.MOBILE.Services.RestSharp;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP
{
    public partial class App
    {
        public static LoginModel UserLogin { get; set; }

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

            containerRegistry.Register<ILicenseKeyService, LicenseKeyService>();
            containerRegistry.Register<IDatabaseService, DatabaseService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            AppCenter.Start("android=bf67e994-451d-4e1b-8248-b269811cf72e;" +
                            "ios=94df858b-344a-4ca3-a03e-54296844bfd6;",
                typeof(Analytics), typeof(Crashes));
        }
    }
}
