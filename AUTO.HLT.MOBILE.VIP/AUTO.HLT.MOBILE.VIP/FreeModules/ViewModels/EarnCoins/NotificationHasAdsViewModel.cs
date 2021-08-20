using System;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.EarnCoins
{
    public class NotificationHasAdsViewModel : BindableBase, IDialogAware
    {
        private string _cancel;
        private string _approve;
        private string _notification;
        private bool _isExcuteCommand;
        private string _bannerId;
        private bool _isLoadingButton;
        public event Action<IDialogParameters> RequestClose;

        public string BannerId
        {
            get => _bannerId;
            set => SetProperty(ref _bannerId, value);
        }

        public string Cancel
        {
            get => _cancel;
            set => SetProperty(ref _cancel, value);
        }

        public string Approve
        {
            get => _approve;
            set => SetProperty(ref _approve, value);
        }

        public string Notification
        {
            get => _notification;
            set => SetProperty(ref _notification, value);
        }

        public ICommand ApproveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool IsExcuteCommand
        {
            get => _isExcuteCommand;
            set => SetProperty(ref _isExcuteCommand, value);
        }

        public bool IsLoadingButton
        {
            get => _isLoadingButton;
            set => SetProperty(ref _isLoadingButton, value);
        }

        public NotificationHasAdsViewModel()
        {
            BannerId = AppConstants.BannerAdmodId;
            ApproveCommand = new Command(() =>
           {
               if (IsExcuteCommand) return;
               IsExcuteCommand = true;
               if (RequestClose != null)
               {
                   var para = new DialogParameters();
                   para.Add(AppConstants.ResultOfAds, true);
                   RequestClose(para);
               }

               IsExcuteCommand = false;
           });
            CancelCommand = new Command(() =>
            {
                if (IsExcuteCommand) return;
                IsExcuteCommand = true;
                if (RequestClose != null)
                {
                    var para = new DialogParameters();
                    para.Add(AppConstants.ResultOfAds, false);
                    RequestClose(para);
                }

                IsExcuteCommand = false;
            });
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            IsExcuteCommand = false;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                Notification = parameters.ContainsKey(AppConstants.Notification)
                    ? parameters.GetValue<string>(AppConstants.Notification)
                    : "Bạn muỗn tiếp tục";
                Cancel = parameters.ContainsKey(AppConstants.Cancel)
                    ? parameters.GetValue<string>(AppConstants.Cancel)
                    : "Cancel";
                Approve = parameters.ContainsKey(AppConstants.Approve)
                    ? parameters.GetValue<string>(AppConstants.Approve)
                    : "OK";
                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    IsLoadingButton = true;
                    return false;
                });
            }
        }
    }
}