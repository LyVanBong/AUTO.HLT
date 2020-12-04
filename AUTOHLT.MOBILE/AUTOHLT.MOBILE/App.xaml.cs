using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Services.RequestProvider;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.ViewModels.Home;
using AUTOHLT.MOBILE.ViewModels.Login;
using AUTOHLT.MOBILE.ViewModels.Transfers;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Login;
using AUTOHLT.MOBILE.Views.Transfers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            //Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense(AppSettings.SyncfusionLicense);
            InitializeComponent();

            await NavigationService.NavigateAsync(nameof(LoginPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Register Service

            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IDatabaseService, DatabaseService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();

            #endregion

            #region Register Singleton

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            #endregion

            #region Register For Navigation

            containerRegistry.RegisterForNavigation<TransferPage, TransferViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            #endregion
        }

        protected override void OnStart()
        {
            base.OnStart();
            AppCenter.Start("android=35b692dd-cd98-46b3-8d02-539662fc9d1a;" +
                            "ios=96a350f3-9856-4888-aad8-db6bcc10590f;",
                typeof(Analytics), typeof(Crashes));
            AppCenter.LogLevel = LogLevel.Verbose;
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
