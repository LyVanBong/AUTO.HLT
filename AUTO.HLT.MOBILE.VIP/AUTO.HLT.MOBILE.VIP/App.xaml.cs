using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.AboutUs;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.HelperUs;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.Home;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.InviteFriend;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.Main;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.MyBooking;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.MyProfile;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.Notification;
using AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.TermsCondition;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.AboutUs;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.HelperUs;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.Home;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.InviteFriend;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.Main;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.MyBooking;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.Notification;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.Profile;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.TermsCondition;
using AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels;
using AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.BuffAPost;
using AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.Home;
using AUTO.HLT.MOBILE.VIP.FreeModules.Views;
using AUTO.HLT.MOBILE.VIP.FreeModules.Views.BuffAPost;
using AUTO.HLT.MOBILE.VIP.FreeModules.Views.Home;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using AUTO.HLT.MOBILE.VIP.Services.RestSharp;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.User;
using AUTO.HLT.MOBILE.VIP.Services.VersionApp;
using AUTO.HLT.MOBILE.VIP.ViewModels.ChangePassword;
using AUTO.HLT.MOBILE.VIP.ViewModels.Feature;
using AUTO.HLT.MOBILE.VIP.ViewModels.FilterFriend;
using AUTO.HLT.MOBILE.VIP.ViewModels.HappyBirthday;
using AUTO.HLT.MOBILE.VIP.ViewModels.Home;
using AUTO.HLT.MOBILE.VIP.ViewModels.KeyGeneration;
using AUTO.HLT.MOBILE.VIP.ViewModels.Login;
using AUTO.HLT.MOBILE.VIP.ViewModels.Manage;
using AUTO.HLT.MOBILE.VIP.ViewModels.Pokes;
using AUTO.HLT.MOBILE.VIP.ViewModels.SuportCustumer;
using AUTO.HLT.MOBILE.VIP.Views.ChangePassword;
using AUTO.HLT.MOBILE.VIP.Views.Feature;
using AUTO.HLT.MOBILE.VIP.Views.FilterFriend;
using AUTO.HLT.MOBILE.VIP.Views.HappyBirthday;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using AUTO.HLT.MOBILE.VIP.Views.KeyGeneration;
using AUTO.HLT.MOBILE.VIP.Views.Login;
using AUTO.HLT.MOBILE.VIP.Views.Manage;
using AUTO.HLT.MOBILE.VIP.Views.Pokes;
using AUTO.HLT.MOBILE.VIP.Views.SuportCustumer;
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppSettings.LICENSE_KEY_SYNCFUSION);

            InitializeComponent();

            await NavigationService.NavigateAsync("/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Module Free

            containerRegistry.RegisterForNavigation<BuffAPostPage, BuffAPostViewModel>();
            containerRegistry.RegisterForNavigation<FreeHomePage, FreeHomeViewModel>();

            #endregion

            #region Phần đăng ký View - ViewModel Fake

            containerRegistry.RegisterForNavigation<NotificationPage, NotificationViewModel>();
            containerRegistry.RegisterForNavigation<MyBookingPage, MyBookingViewModel>();
            containerRegistry.RegisterForNavigation<FAboutUsPage, AboutUsViewModel>();
            containerRegistry.RegisterForNavigation<MyProfilePage, MyProfileViewModel>();
            containerRegistry.RegisterForNavigation<FMainPage, FMainViewModel>();
            containerRegistry.RegisterForNavigation<FHomePage, FHomeViewModel>();
            containerRegistry.RegisterForNavigation<FMainPageFlyout, FMainPageFlyoutViewModel>();
            containerRegistry.RegisterForNavigation<HelperUsPage, HelperUsViewModel>();
            containerRegistry.RegisterForNavigation<InviteFriendPage, InviteFriendViewModel>();
            containerRegistry.RegisterForNavigation<TermsConditionPage, TermsConditionViewModel>();

            #endregion

            containerRegistry.RegisterDialog<ConnectFacebookDialog, ConnectFacebookDialogViewModel>();

            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IVersionAppService, VersionAppService>();
            containerRegistry.Register<ITelegramService, TelegramService>();
            containerRegistry.Register<IFacebookService, FacebookeService>();
            containerRegistry.Register<ILicenseKeyService, LicenseKeyService>();
            containerRegistry.Register<IDatabaseService, DatabaseService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordViewModel>();
            containerRegistry.RegisterForNavigation<KeyGenerationPage, KeyGenerationViewModel>();
            containerRegistry.RegisterForNavigation<ManagePage, ManageViewModel>();
            containerRegistry.RegisterForNavigation<HappyBirthdayPage, HappyBirthdayViewModel>();
            containerRegistry.RegisterForNavigation<SuportCustumerPage, SuportCustomerViewModel>();
            containerRegistry.RegisterForNavigation<FilterFriendPage, FilterFriendViewModel>();
            containerRegistry.RegisterForNavigation<PokesPage, PokesViewModel>();
            containerRegistry.RegisterForNavigation<FeaturePage, FeatureViewModel>();
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
