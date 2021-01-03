using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.DependencyServices;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook
{
    public class ConnectFacebookeViewModel : BindableBase, IDialogAware
    {
        private string _uriFacebook;
        private bool _isLoading;
        private bool _isVisibleWebview;

        public bool IsVisibleWebview
        {
            get => _isVisibleWebview;
            set => SetProperty(ref _isVisibleWebview, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand CloseCommand { get; private set; }
        public string UriFacebook
        {
            get => _uriFacebook;
            set => SetProperty(ref _uriFacebook, value);
        }

        public event Action<IDialogParameters> RequestClose;

        public ConnectFacebookeViewModel()
        {
            CloseCommand = new Command(ClosePopupConnectFacebook);
        }

        private void ClosePopupConnectFacebook()
        {
            ClosePopup();
        }

        public bool CanCloseDialog() => true;

        private void ClosePopup()
        {
            if (RequestClose != null)
            {
                RequestClose(null);
            }
        }

        public void OnDialogClosed()
        {
            try
            {
                MessagingCenter.Unsubscribe<App>(this, AppConstants.GetokenDone);
                MessagingCenter.Unsubscribe<App>(this, AppConstants.GetCookieDone);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            DependencyService.Get<IClearCookies>().ClearAllCookies();
            IsLoading = false;
            IsVisibleWebview = true;
            MessagingCenter.Subscribe<App>(this, AppConstants.GetokenDone, (message) =>
            {
                ClosePopup();
            });
            MessagingCenter.Subscribe<App>(this, AppConstants.GetCookieDone, (message) =>
            {
                IsLoading = true;
                IsVisibleWebview = false;
            });
            UriFacebook = AppConstants.UriLoginFacebook;
        }
    }
}