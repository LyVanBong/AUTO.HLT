using AUTOHLT.MOBILE.ViewModels.Home;
using AUTOHLT.MOBILE.ViewModels.Login;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Login;
using Prism;
using Prism.Ioc;
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
            InitializeComponent();

            await NavigationService.NavigateAsync(nameof(LoginPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Register Singleton

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            #endregion

            #region Register For Navigation

            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            #endregion
        }
    }
}
