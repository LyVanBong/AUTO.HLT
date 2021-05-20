using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace AUTOHLT.MOBILE.ViewModels.UnLockFb
{
    public class UnLockFbViewModel : ViewModelBase
    {
        private string _content;
        private bool _isLoading;
        private bool _isEnabledButton;
        private ITelegramService _telegramService;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private IProductService _productService;


        public ICommand CallAdminCommand { get; private set; }
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        public ICommand UnlockFacebookCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand UnfocusedCommand { get; private set; }
        public UnLockFbViewModel(INavigationService navigationService, ITelegramService telegramService, IDatabaseService databaseService, IPageDialogService pageDialogService, IProductService productService) : base(navigationService)
        {
            _productService = productService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _telegramService = telegramService;
            UnfocusedCommand = new Command(Unfocused);
            UnlockFacebookCommand = new Command(UnlockFacebook);
            CallAdminCommand = new Command(CallAdmin);
        }

        private async void CallAdmin()
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
                var user = await _databaseService.GetAccountUser();
                var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                {
                    Ten_Thong_Bao = "Mở khóa facebook",
                    So_Luong = 1,
                    Id_Nguoi_Dung = user?.ID,
                    Noi_Dung_Thong_Bao = new
                    {
                        Noi_Dung = "Khách hàng chọn vào liên hệ với admin mong được tư vẫn",
                    },
                    Ghi_Chu = new
                    {
                        Ten = user?.Name,
                        Tai_Khoan = user?.UserName,
                        So_dien_thoai = user?.NumberPhone
                    }
                }, Formatting.Indented);
                var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, content);
                IsLoading = false;
            }
        }

        private async void UnlockFacebook()
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
                            Ten_Thong_Bao = "Mở khóa facebook",
                            So_Luong = 1,
                            Id_Nguoi_Dung = user?.ID,
                            Noi_Dung_Thong_Bao = new
                            {
                                Noi_Dung = Content,
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
                            await _productService.AddHistoryUseService("57814f66-909d-4295-bd37-6ee4647d5bb7", Resource._1000081, user.ID, "1", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss"));
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
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

        private void Unfocused()
        {
            if (IsLoading) return;
            IsLoading = true;

            IsEnabledButton = !string.IsNullOrWhiteSpace(Content);

            IsLoading = false;
        }
    }
}