
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTO.HLT.MOBILE.VIP.FakeModules.Views.HelperUs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelperUsPage : ContentPage
    {
        public HelperUsPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            GopY.Text = "";
        }
    }
}