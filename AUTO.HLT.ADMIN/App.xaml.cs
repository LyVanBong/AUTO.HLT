using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Services.RequestProvider;
using AUTO.HLT.ADMIN.Services.RestSharp;
using AUTO.HLT.ADMIN.Services.Telegram;
using AUTO.HLT.ADMIN.Views.AddWork;
using AUTO.HLT.ADMIN.Views.AutoHltCrm;
using AUTO.HLT.ADMIN.Views.CheckWork;
using AUTO.HLT.ADMIN.Views.Main;
using AUTOHLT.MOBILE.Services.RestSharp;
using CefSharp;
using CefSharp.Wpf;
using Prism.Ioc;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFacebookService, FacebookeService>();
            containerRegistry.Register<ITelegramService, TelegramService>();
            containerRegistry.Register<IRestSharpService, RestSharpService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();

            containerRegistry.RegisterForNavigation<AutoHltCrmView>();
            containerRegistry.RegisterForNavigation<AddWorkView>();
            containerRegistry.RegisterForNavigation<CheckWorkView>();
        }

        #region Khởi tạo cefsharp

        public App()
        {
            //Add Custom assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            InitializeCefSharp();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    assemblyName);

                return File.Exists(archSpecificPath)
                    ? Assembly.LoadFile(archSpecificPath)
                    : null;
            }

            return null;
        }

        #endregion Khởi tạo cefsharp
    }
}