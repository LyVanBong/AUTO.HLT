using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace AUTOHLT.MOBILE.ViewModels.SuportCustumer
{
    public class SuportCustomerViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IDatabaseService _databaseService;
        private ITelegramService _telegramService;
        private string _content;
        private bool _isEnabledButton;
        private IPageDialogService _pageDialogService;

        public ICommand SuportCustomerCommand { get; private set; }
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        public ICommand UnfocusedCommand { get; private set; }
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand CallAdminCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public SuportCustomerViewModel(INavigationService navigationService, IDatabaseService databaseService, ITelegramService telegramService,IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _telegramService = telegramService;
            CallAdminCommand = new Command(CallAdmin);
            UnfocusedCommand = new Command(Unfocused);
            SuportCustomerCommand = new Command(SuportCustomer);
        }

        private async void SuportCustomer()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Content))
                {
                    var user = await _databaseService.GetAccountUser();
                    if (user != null)
                    {
                        var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                        {
                            Ten_Thong_Bao = "Hỗ trợ khách hàng",
                            So_Luong = 1,
                            Id_Nguoi_Dung = user?.ID,
                            Noi_Dung_Thong_Bao = new
                            {
                                Noi_Dung=Content,
                            },
                            Ghi_Chu = new
                            {
                                Ten = user?.Name,
                                Tai_Khoan = user?.UserName,
                                So_dien_thoai = user?.NumberPhone
                            }
                        }, Formatting.Indented);
                        var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, content);
                        if (send != null && send.ok && send.result != null)
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000094, "OK");
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                        }
                    }
                    Content = String.Empty;
                    IsEnabledButton = false;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void Unfocused()
        {
            if (IsLoading) return;
            IsLoading = true;

            IsEnabledButton = !string.IsNullOrWhiteSpace(Content);

            IsLoading = false;
        }

        private void CallAdmin()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                PhoneDialer.Open(AppConstants.NumberPhoneAdmin);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }
    }
}