using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Manage
{
    public class ManageViewModel : ViewModelBase
    {
        private bool _isLoading;
        private ILicenseKeyService _licenseKeyService;
        private IDatabaseService _databaseService;
        private ObservableCollection<AgecyLicenseModel> _lsMyLicense;
        private IDialogService _dialogService;
        private IFacebookService _facebookService;
        private ITelegramService _telegramService;
        private IPageDialogService _pageDialogService;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<AgecyLicenseModel> LsMyLicense
        {
            get => _lsMyLicense;
            set => SetProperty(ref _lsMyLicense, value);
        }

        public ICommand AutoServiceCommand { get; private set; }
        public ManageViewModel(INavigationService navigationService, ILicenseKeyService licenseKeyService, IDatabaseService databaseService, IDialogService dialogService, IFacebookService facebookService, ITelegramService telegramService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _telegramService = telegramService;
            _facebookService = facebookService;
            _dialogService = dialogService;
            _databaseService = databaseService;
            _licenseKeyService = licenseKeyService;
            IsLoading = true;
            AutoServiceCommand = new AsyncCommand<string>(async (key) => await AutoService(key));
        }
        /// <summary>
        /// ket noi facebook
        /// </summary>
        /// <returns></returns>
        private async Task ConnectFacebook()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog));
            IsLoading = false;
        }
        private async Task AutoService(string key)
        {
            try
            {
                if (await _facebookService.CheckCookieAndToken())
                {
                    var user = await _databaseService.GetAccountUser();
                    ContentSendTelegramModel content = new ContentSendTelegramModel();
                    switch (key)
                    {
                        case "0":

                            content = new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Thả tim bài viết",
                                So_Luong = 1,
                                Id_Nguoi_Dung = user?.ID,
                                Ghi_Chu = new
                                {
                                    Ten = user?.Name,
                                    Tai_Khoan = user?.UserName,
                                    So_dien_thoai = user?.NumberPhone,
                                    Loai_Tai_Khoan = "Đại lý",
                                    So_Ban_Quyen_Da_Mua = LsMyLicense?.Count
                                },
                                Noi_Dung_Thong_Bao = new
                                {
                                    Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                    Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                }
                            };
                            
                            break;
                        case "1":
                            content = new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Gửi tin nhắn",
                                So_Luong = 1,
                                Id_Nguoi_Dung = user?.ID,
                                Ghi_Chu = new
                                {
                                    Ten = user?.Name,
                                    Tai_Khoan = user?.UserName,
                                    So_dien_thoai = user?.NumberPhone,
                                    Loai_Tai_Khoan = "Đại lý",
                                    So_Ban_Quyen_Da_Mua=LsMyLicense?.Count
                                },
                                Noi_Dung_Thong_Bao = new
                                {
                                    Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                    Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                }
                            };
                            break;
                        case "2":
                            content = new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Thả tim story",
                                So_Luong = 1,
                                Id_Nguoi_Dung = user?.ID,
                                Ghi_Chu = new
                                {
                                    Ten = user?.Name,
                                    Tai_Khoan = user?.UserName,
                                    So_dien_thoai = user?.NumberPhone,
                                    Loai_Tai_Khoan = "Đại lý",
                                    So_Ban_Quyen_Da_Mua = LsMyLicense?.Count
                                },
                                Noi_Dung_Thong_Bao = new
                                {
                                    Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                    Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                }
                            };
                            break;
                        case "3":
                            content = new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Thả tim ảnh đại diện",
                                So_Luong = 1,
                                Id_Nguoi_Dung = user?.ID,
                                Ghi_Chu = new
                                {
                                    Ten = user?.Name,
                                    Tai_Khoan = user?.UserName,
                                    So_dien_thoai = user?.NumberPhone,
                                    Loai_Tai_Khoan = "Đại lý",
                                    So_Ban_Quyen_Da_Mua = LsMyLicense?.Count
                                },
                                Noi_Dung_Thong_Bao = new
                                {
                                    Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                    Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                }
                            };
                            break;
                        default:
                            break;
                    }
                    var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                        JsonConvert.SerializeObject(content, Formatting.Indented));
                    if (send != null && send.ok)
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Cài đặt thành công", "OK");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh bạn vui lòng cài lại hoặc liên hệ admin để được hỗ trợ", "OK");
                    }
                }
                else
                {
                    if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Để sử dụng tính năng này bạn cần kết nối với tài khoản facebook của mình", "Kết nối ngay", "Thôi"))
                    {
                        await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog));
                        await AutoService(key);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var data = await _licenseKeyService.GetLicenseForAgecy();
            if (data != null && data.Code > 0 && data.Data != null)
            {
                LsMyLicense = new ObservableCollection<AgecyLicenseModel>(data.Data);
            }
            IsLoading = false;
        }
    }
}