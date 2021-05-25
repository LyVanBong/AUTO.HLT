using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.FakeModules.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.FakeModules.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FMain2Page : TabbedPage
    {
        public FMain2Page()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var vm = BindingContext as FMainViewModel;
            if (vm != null)
            {
                vm.GoBackHomeCommand.Execute(null);
            }
        }
    }
}