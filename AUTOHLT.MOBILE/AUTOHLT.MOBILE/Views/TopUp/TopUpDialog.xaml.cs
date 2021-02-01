using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.TopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopUpDialog
    {
        public TopUpDialog()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open("0824726888");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}