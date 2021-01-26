using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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
           Xamarin.Essentials.PhoneDialer.Open("0824726888");
        }
    }
}