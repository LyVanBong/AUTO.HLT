using System;
using System.Windows.Input;
using Acr.UserDialogs;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using Microsoft.AppCenter.Crashes;
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
                PhoneDialer.Open("0829.726.888");
            }
            catch (Exception e)
            {
                var messager = "Bạn vui lòng liên hệ trực tiếp để được hố trợ thêm!";
                UserDialogs.Instance.Toast(new ToastConfig(messager)
                {

                    BackgroundColor = System.Drawing.Color.WhiteSmoke,
                    Message = messager,
                    Position = ToastPosition.Top,
                    MessageTextColor = Color.DarkGray,
                });
                Crashes.TrackError(e);
            }
            finally
            {
                var user = await _databaseService.GetAccountUser();
                var message = $"{Resource._1000081}\n" +
                              $"Nội dung: Khách hàng chọn vào liên hệ với admin mong được tư vẫn\n" +
                              $"Id người dùng dịch vụ: {user.ID}\n" +
                              $"Số điện thoại: {user.NumberPhone}\n" +
                              $"Tên: {user.Name}\n" +
                              $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, message);
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
                        var message = $"{Resource._1000081}\n" +
                                      $"Nội dung: {Content}\n" +
                                      $"Id người dùng dịch vụ: {user.ID}\n" +
                                      $"Số điện thoại: {user.NumberPhone}\n" +
                                      $"Tên: {user.Name}\n" +
                                      $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                        var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, message);
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