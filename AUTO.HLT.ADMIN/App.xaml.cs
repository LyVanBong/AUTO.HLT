using AUTO.HLT.ADMIN.Services.AutoLikeCommentAvatar;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Services.RequestProvider;
using AUTO.HLT.ADMIN.Services.RestSharp;
using AUTO.HLT.ADMIN.Services.Telegram;
using AUTO.HLT.ADMIN.Views.Main;
using AUTOHLT.MOBILE.Services.RestSharp;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using System.Windows;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppCenter.Start("31b8b4ef-97ea-42e5-a160-ec0bd1d3e307",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IAutoAvatarService, AutoAvatarService>();
            containerRegistry.Register<IFacebookService, FacebookeService>();
            containerRegistry.Register<ITelegramService, TelegramService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
        }
    }
}