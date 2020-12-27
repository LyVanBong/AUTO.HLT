using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Controls.Dialog.UseService;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.HistoryService;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.RequestProvider;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.ViewModels.AccountInformation;
using AUTOHLT.MOBILE.ViewModels.AddFullFriend;
using AUTOHLT.MOBILE.ViewModels.BuffEyesView;
using AUTOHLT.MOBILE.ViewModels.BuffLikePage;
using AUTOHLT.MOBILE.ViewModels.BuffLikes;
using AUTOHLT.MOBILE.ViewModels.ChangePassword;
using AUTOHLT.MOBILE.ViewModels.FakeUpApp;
using AUTOHLT.MOBILE.ViewModels.FilterFriend;
using AUTOHLT.MOBILE.ViewModels.Home;
using AUTOHLT.MOBILE.ViewModels.Interactive;
using AUTOHLT.MOBILE.ViewModels.Login;
using AUTOHLT.MOBILE.ViewModels.RechargeCustomers;
using AUTOHLT.MOBILE.ViewModels.SecurityFb;
using AUTOHLT.MOBILE.ViewModels.SuportCustumer;
using AUTOHLT.MOBILE.ViewModels.Transfers;
using AUTOHLT.MOBILE.ViewModels.UnLockFb;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.AddFullFriend;
using AUTOHLT.MOBILE.Views.BuffEyesView;
using AUTOHLT.MOBILE.Views.BuffLikePage;
using AUTOHLT.MOBILE.Views.BuffLikes;
using AUTOHLT.MOBILE.Views.BuffSub;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FakeUpApp;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Interactive;
using AUTOHLT.MOBILE.Views.Login;
using AUTOHLT.MOBILE.Views.RechargeCustomers;
using AUTOHLT.MOBILE.Views.SecurityFb;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using AUTOHLT.MOBILE.Views.Transfers;
using AUTOHLT.MOBILE.Views.UnLockFb;
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
            #region Registering the Dialog service

            containerRegistry.RegisterDialog<BuffDialog, BuffDialogViewModel>();
            containerRegistry.RegisterDialog<UseServiceDialog, UseServiceDialogViewModel>();

            #endregion

            #region Register Service

            containerRegistry.Register<ITelegramService, TelegramService>();
            containerRegistry.Register<IHistoryService, HistoryService>();
            containerRegistry.Register<IProductService, ProductService>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IDatabaseService, DatabaseService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();

            #endregion

            #region Register Singleton

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            #endregion

            #region Register For Navigation

            containerRegistry.RegisterForNavigation<BuffSubPage, BuffLikePageViewModel>();
            containerRegistry.RegisterForNavigation<SuportCustumerPage, SuportCustomerViewModel>();
            containerRegistry.RegisterForNavigation<FilterFriendPage, FilterFriendViewModel>();
            containerRegistry.RegisterForNavigation<SecurityFbPage, SecurityFbViewModel>();
            containerRegistry.RegisterForNavigation<UnLockFbPage, UnLockFbViewModel>();
            containerRegistry.RegisterForNavigation<BuffLikePagePage, BuffLikePageViewModel>();
            containerRegistry.RegisterForNavigation<AddFullFriendPage, AddFullFriendViewModel>();
            containerRegistry.RegisterForNavigation<HomePageF, HomeFViewModel>();
            containerRegistry.RegisterForNavigation<InteractivePage, InteractiveViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordViewModels>();
            containerRegistry.RegisterForNavigation<AccountInformationPage, AccountInformationViewModel>();
            containerRegistry.RegisterForNavigation<RechargeCustomersPage, RechargeCustomersViewModel>();
            containerRegistry.RegisterForNavigation<BuffEyesViewPage, BuffEyesViewViewModel>();
            containerRegistry.RegisterForNavigation<BuffLikePage, BuffLikeViewModel>();
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
