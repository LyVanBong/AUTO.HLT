using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
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
        public ICommand HDSDCommand { get; private set; }
        public ICommand ClosePopupCommand { get; set; }
        public bool CanCloseDialog() => true;

        public TopUpDialogViewModel(IGuideService guideService)
        {
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

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        public event Action<IDialogParameters> RequestClose;
    }
}