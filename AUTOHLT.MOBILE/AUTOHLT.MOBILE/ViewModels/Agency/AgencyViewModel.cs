using System;
using System.Windows.Input;
using Acr.UserDialogs;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Agency
{
    public class AgencyViewModel : ViewModelBase
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

        public AgencyViewModel(INavigationService navigationService, IDatabaseService databaseService, ITelegramService telegramService, IPageDialogService pageDialogService) : base(navigationService)
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
                        var message = $"Đăng ký đại lý\n" +
                                      $"Nội dung: {Content}\n" +
                                      $"Id người dùng dịch vụ: {user.ID}\n" +
                                      $"Số điện thoại: {user.NumberPhone}\n" +
                                      $"Tên: {user.Name}\n" +
                                      $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                        var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, message);
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