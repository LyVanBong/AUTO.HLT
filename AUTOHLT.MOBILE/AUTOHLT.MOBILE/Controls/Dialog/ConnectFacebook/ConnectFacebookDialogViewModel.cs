using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Facebook;
using Prism.Mvvm;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook
{
    public class ConnectFacebookDialogViewModel : BindableBase, IDialogAware
    {
        private bool _isVisibleConnect;
        private string _urlFacebook;

        public event Action<IDialogParameters> RequestClose;

        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private IFacebookService _facebookService;
        private IDatabaseService _databaseService;

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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ConnectFacebookDialogViewModel(IPageDialogService pageDialogService, IFacebookService facebookService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _facebookService = facebookService;
            _pageDialogService = pageDialogService;
            FuntionCommand = new AsyncCommand<string>(Funtion);
            MessagingCenter.Subscribe<App>(this, AppConstants.GetTokenDone, async (app) =>
            {
                var usr = await _databaseService.GetAccountUser();
                var face = await _facebookService.GetInfoUser("name,picture", Preferences.Get(AppConstants.TokenFaceook, ""));
                await _facebookService.UpdateUserFacebook(usr.ID, face.id, Preferences.Get(AppConstants.CookieFacebook, ""), Preferences.Get(AppConstants.TokenFaceook, ""), "APP AUTOHLT");
                ClosePopup(null);
            });
            MessagingCenter.Subscribe<App>(this, AppConstants.GetCookieDone, async (app) =>
            {
                IsVisibleConnect = true;
                if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Đăng nhập thành công !", "Kết nối",
                    "Không"))
                    await Funtion("0");
            });
        }

        private Task Funtion(string arg)
        {
            if (arg == "0")
            {
                IsLoading = true;
                UrlFacebook = AppConstants.UriGetTokenFacebook;
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
            MessagingCenter.Unsubscribe<App>(this, AppConstants.GetCookieDone);
            MessagingCenter.Unsubscribe<App>(this, AppConstants.GetTokenDone);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            IsLoading = true;
            UrlFacebook = AppConstants.UriLoginFacebook;
            IsLoading = false;
        }
    }
}