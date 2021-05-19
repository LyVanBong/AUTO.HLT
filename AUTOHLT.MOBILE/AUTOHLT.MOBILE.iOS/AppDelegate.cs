using Foundation;
using Google.MobileAds;
using Prism;
using Prism.Ioc;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfDataGrid.XForms.iOS;
using Syncfusion.SfGauge.XForms.iOS;
using Syncfusion.SfSunburstChart.XForms.iOS;
using Syncfusion.SfTreeMap.XForms.iOS;
using Syncfusion.XForms.iOS.BadgeView;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.DataForm;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.ProgressBar;
using Syncfusion.XForms.iOS.TextInputLayout;
using System;
using UIKit;


namespace AUTOHLT.MOBILE.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            OtherLibraries(app, options);
            LoadApplication(new App(new iOSInitializer()));
            return base.FinishedLaunching(app, options);
        }
        
        private void OtherLibraries(UIApplication app, NSDictionary options)
        {
            MobileAds.SharedInstance.Start(CompletionHandler);
            new SfTreeMapRenderer();
            SfDataFormRenderer.Init();
            SfGaugeRenderer.Init();
            SfChartRenderer.Init();
            SfCalendarRenderer.Init();
            // Add the below line if you are using SfLinearProgressBar.
            SfLinearProgressBarRenderer.Init();
            SfSunburstChartRenderer.Init();
            // Add the below line if you are using SfCircularProgressBar.  
            SfCircularProgressBarRenderer.Init();
            SfBadgeViewRenderer.Init();
            SfMaskedEditRenderer.Init();
            SfRadioButtonRenderer.Init();
            new SfBusyIndicatorRenderer();
            SfTextInputLayoutRenderer.Init();
            SfCheckBoxRenderer.Init();
            SfGradientViewRenderer.Init();
            SfDataGridRenderer.Init();
        }

        private void CompletionHandler(InitializationStatus status)
        {
            
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
