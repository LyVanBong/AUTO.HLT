using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook
{
    public class ConnectFacebookDialogViewModel : BindableBase, IDialogAware
    {
        private bool _isVisibleConnect;
        private string _urlFacebook;
        public event Action<IDialogParameters> RequestClose;

        public ICommand FuntionCommand { get; private set; }

        public string UrlFacebook
        {
            get => _urlFacebook;
            set => SetProperty(ref _urlFacebook, value);
        }

        public bool IsVisibleConnect
        {
            get => _isVisibleConnect;
            set => SetProperty(ref _isVisibleConnect, value);
        }

        public ConnectFacebookDialogViewModel()
        {
            FuntionCommand = new AsyncCommand<string>(Funtion);
        }

        private Task Funtion(string arg)
        {
            if (arg == "0")
            {

            }
            else
            {
                ClosePopup(null);
            }
            return Task.FromResult(0);
        }

        private void ClosePopup(DialogParameters para)
        {
            if (RequestClose != null)
                RequestClose(para);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            UrlFacebook = AppConstants.UriLoginFacebook;
        }
    }
}