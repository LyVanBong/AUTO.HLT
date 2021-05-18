using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTO.HLT.MOBILE.VIP.FakeModules.Views.InviteFriend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteFriendPage : ContentPage
    {
        public InviteFriendPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Sodt.Text = "";
        }
    }
}