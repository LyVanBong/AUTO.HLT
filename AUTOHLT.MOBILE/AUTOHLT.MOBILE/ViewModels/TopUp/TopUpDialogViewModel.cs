using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Guide;
using Microsoft.AppCenter.Crashes;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.TopUp
{
    public class TopUpDialogViewModel : BindableBase, IDialogAware
    {
        private IGuideService _guideService;
        private string _userName;
        private IDatabaseService _databaseService;

        public ICommand HDSDCommand { get; private set; }
        public ICommand ClosePopupCommand { get; set; }
        public bool CanCloseDialog() => true;
        public event Action<IDialogParameters> RequestClose;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public TopUpDialogViewModel(IGuideService guideService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _guideService = guideService;
            ClosePopupCommand = new Command(ClosePopup);
            HDSDCommand = new Command(HDSDApp);
        }

        private async void HDSDApp()
        {
            try
            {
                var data = await _guideService.GetGuide(4);
                await Browser.OpenAsync(data?.Url);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ClosePopup()
        {
            if (RequestClose != null)
            {
                RequestClose(null);
            }
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var data = await _databaseService.GetAccountUser();
            UserName = $"Nội dung chuyển khoản :  {data?.UserName} autohlt";
        }
    }
}