using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Controls.Dialog.UseService;
using AUTOHLT.MOBILE.Controls.Dialog.VerifyOtp;
using AUTOHLT.MOBILE.FakeModules.ViewModels;
using AUTOHLT.MOBILE.FakeModules.Views;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Facebook;
using AUTOHLT.MOBILE.Services.Guide;
using AUTOHLT.MOBILE.Services.HistoryService;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.RequestProvider;
using AUTOHLT.MOBILE.Services.RestSharp;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.Services.VersionAppService;
using AUTOHLT.MOBILE.ViewModels.AccountInformation;
using AUTOHLT.MOBILE.ViewModels.AddFullFriend;
using AUTOHLT.MOBILE.ViewModels.Agency;
using AUTOHLT.MOBILE.ViewModels.BuffEyesView;
using AUTOHLT.MOBILE.ViewModels.BuffLikePage;
using AUTOHLT.MOBILE.ViewModels.BuffLikes;
using AUTOHLT.MOBILE.ViewModels.ChangePassword;
using AUTOHLT.MOBILE.ViewModels.FilterFriend;
using AUTOHLT.MOBILE.ViewModels.HappyBirthday;
using AUTOHLT.MOBILE.ViewModels.Home;
using AUTOHLT.MOBILE.ViewModels.Interactive;
using AUTOHLT.MOBILE.ViewModels.Login;
using AUTOHLT.MOBILE.ViewModels.Pokes;
using AUTOHLT.MOBILE.ViewModels.RechargeCustomers;
using AUTOHLT.MOBILE.ViewModels.SecurityFb;
using AUTOHLT.MOBILE.ViewModels.SuportCustumer;
using AUTOHLT.MOBILE.ViewModels.TopUp;
using AUTOHLT.MOBILE.ViewModels.Transfers;
using AUTOHLT.MOBILE.ViewModels.UnLockFb;
using AUTOHLT.MOBILE.ViewModels.VipInteraction;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.AddFullFriend;
using AUTOHLT.MOBILE.Views.Agency;
using AUTOHLT.MOBILE.Views.BuffEyesView;
using AUTOHLT.MOBILE.Views.BuffLikePage;
using AUTOHLT.MOBILE.Views.BuffLikes;
using AUTOHLT.MOBILE.Views.BuffSub;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.HappyBirthday;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Interactive;
using AUTOHLT.MOBILE.Views.Login;
using AUTOHLT.MOBILE.Views.Pokes;
using AUTOHLT.MOBILE.Views.RechargeCustomers;
using AUTOHLT.MOBILE.Views.SecurityFb;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using AUTOHLT.MOBILE.Views.TopUp;
using AUTOHLT.MOBILE.Views.Transfers;
using AUTOHLT.MOBILE.Views.UnLockFb;
using AUTOHLT.MOBILE.Views.VipInteraction;
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
            #region App fake

            containerRegistry.RegisterForNavigation<FIntroducePage, FIntroduceViewModel>();
            containerRegistry.RegisterForNavigation<FMain2Page, FMainViewModel>();
            containerRegistry.RegisterForNavigation<FHomePage, FHomeViewModel>();
            containerRegistry.RegisterForNavigation<FProfilePage, FProfileViewModel>();

            #endregion
            #region Registering the Dialog service

            containerRegistry.RegisterDialog<TopUpDialog, TopUpDialogViewModel>();
            containerRegistry.RegisterDialog<OtpDialog, OtpViewModel>();
            containerRegistry.RegisterDialog<ConnectFacebookDialog, ConnectFacebookDialogViewModel>();
            containerRegistry.RegisterDialog<BuffDialog, BuffDialogViewModel>();
            containerRegistry.RegisterDialog<UseServiceDialog, UseServiceDialogViewModel>();

            #endregion

            #region Register Service

            containerRegistry.Register<IVersionAppService, VersionAppService>();
            containerRegistry.Register<IGuideService, GuideService>();
            containerRegistry.Register<IFacebookService, FacebookeService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
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

            containerRegistry.RegisterForNavigation<TopUpPage, TopUpViewModel>();
            containerRegistry.RegisterForNavigation<HappyBirthdayPage, HappyBirthdayViewModel>();
            containerRegistry.RegisterForNavigation<AgencyPage, AgencyViewModel>();
            containerRegistry.RegisterForNavigation<VipInteractionPage, VipInteractionViewModel>();
            containerRegistry.RegisterForNavigation<PokesPage, PokesViewModel>();
            containerRegistry.RegisterForNavigation<BuffSubPage, BuffLikePageViewModel>();
            containerRegistry.RegisterForNavigation<SuportCustumerPage, SuportCustomerViewModel>();
            containerRegistry.RegisterForNavigation<FilterFriendPage, FilterFriendViewModel>();
            containerRegistry.RegisterForNavigation<SecurityFbPage, SecurityFbViewModel>();
            containerRegistry.RegisterForNavigation<UnLockFbPage, UnLockFbViewModel>();
            containerRegistry.RegisterForNavigation<BuffLikePagePage, BuffLikePageViewModel>();
            containerRegistry.RegisterForNavigation<AddFullFriendPage, AddFullFriendViewModel>();
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
            AppCenter.Start("android=56d7dd9c-0c31-4519-b53c-2dd43107075b;" +
                            "ios=0fc2b766-98ac-48ee-95dc-b310b43e1331;",
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
