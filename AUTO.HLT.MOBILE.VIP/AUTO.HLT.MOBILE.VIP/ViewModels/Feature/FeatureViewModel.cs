using AUTO.HLT.MOBILE.VIP.Models.Home;
using AUTO.HLT.MOBILE.VIP.Models.UseService;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Feature
{
    public class FeatureViewModel : ViewModelBase
    {
        private string _maxNumber;
        private ItemMenuModel _itemMenu;
        private string _number;
        private string _content;
        private IPageDialogService _pageDialogService;
        private ILicenseKeyService _licenseKeyService;
        private ITelegramService _telegramService;
        private IDatabaseService _databaseService;
        private LoginModel _user;
        private bool _isLoading;

        public ICommand RunFeatureCommand { get; set; }

        public string MaxNumber
        {
            get => _maxNumber;
            set => SetProperty(ref _maxNumber, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public FeatureViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ILicenseKeyService licenseKeyService, ITelegramService telegramService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _telegramService = telegramService;
            _licenseKeyService = licenseKeyService;
            _pageDialogService = pageDialogService;
            RunFeatureCommand = new AsyncCommand<string>(RunFeature);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            if (parameters != null)
            {
                _itemMenu = parameters.GetValue<ItemMenuModel>("TypeFeature");
                if (_itemMenu != null)
                {
                    Title = _itemMenu.TitleItem;
                    await CheckAcountUseService();
                }
            }

            IsLoading = false;
        }

        private async Task CheckAcountUseService()
        {
            var license = await _licenseKeyService.CheckLicenseForUser();
            if (_itemMenu.Id == 1)
            {
                if (license == null || string.IsNullOrEmpty(license.HistoryUseProduct))
                    MaxNumber = "100";
                else
                {
                    var history = JsonConvert.DeserializeObject<UserServiceModel>(license.HistoryUseProduct);
                    if (history.DateUse.Date == DateTime.Now.Date)
                    {
                        MaxNumber = (100 - history.NumberLike) + "";
                    }
                    else
                    {
                        MaxNumber = "100";
                    }
                }
            }
            else if (_itemMenu.Id == 2)
            {
                if (license == null || license.HistoryUseProduct == null)
                    MaxNumber = "2000";
                else
                {
                    var history = JsonConvert.DeserializeObject<UserServiceModel>(license.HistoryUseProduct);
                    if (history != null)
                    {
                        if (history.Follow > 1999)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo",
                                "Bạn đã sử dụng hết số lượt của chức năng này", "OK");
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            MaxNumber = (2000 - history.Follow) + "";
                        }
                    }
                    else
                    {
                        MaxNumber = "0";
                    }
                }
            }
        }

        private async Task RunFeature(string arg)
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                switch (arg)
                {
                    case "1":
                        await NavigationService.GoBackAsync();
                        break;
                    case "0":
                        if (string.IsNullOrWhiteSpace(Content) || string.IsNullOrWhiteSpace(Number))
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng điền đầy đủ thông tin",
                                "OK");
                        }
                        else
                        {
                            var info = await _licenseKeyService.CheckLicenseForUser();
                            if (info != null)
                            {
                                var json = "";
                                var number = int.Parse(Number);
                                if (number > 0 && number <= int.Parse(MaxNumber))
                                {
                                    _user = await _databaseService.GetAccountUser();
                                    if (_itemMenu.Id == 1)
                                    {
                                        if (string.IsNullOrEmpty(info.HistoryUseProduct))
                                        {
                                            if (number <= 100)
                                            {
                                                json = JsonConvert.SerializeObject(new UserServiceModel()
                                                {
                                                    NumberLike = number,
                                                    DateUse = DateTime.Now,
                                                    Follow = 0
                                                });
                                                await UseService(info, json, number);
                                            }
                                            else
                                            {
                                                await ThongBaoKhongHopLe();
                                            }
                                        }
                                        else
                                        {
                                            var history =
                                                JsonConvert.DeserializeObject<UserServiceModel>(info.HistoryUseProduct);
                                            if (history.DateUse.Date == DateTime.Now.Date)
                                            {
                                                var num = (100 - history.NumberLike) > 0
                                                    ? (100 - history.NumberLike)
                                                    : 0;
                                                if (num >= number)
                                                {
                                                    json = JsonConvert.SerializeObject(new UserServiceModel()
                                                    {
                                                        DateUse = DateTime.Now,
                                                        NumberLike = number + history.NumberLike,
                                                        Follow = history.Follow,
                                                    });
                                                    await UseService(info, json, number);
                                                }
                                                else
                                                {
                                                    await ThongBaoKhongHopLe();
                                                }
                                            }
                                            else
                                            {
                                                if (number <= 100)
                                                {
                                                    json = JsonConvert.SerializeObject(new UserServiceModel()
                                                    {
                                                        NumberLike = number,
                                                        DateUse = DateTime.Now,
                                                        Follow = history.Follow,
                                                    });
                                                    await UseService(info, json, number);
                                                }
                                                else
                                                {
                                                    await ThongBaoKhongHopLe();
                                                }
                                            }
                                        }

                                    }
                                    else if (_itemMenu.Id == 2)
                                    {
                                        if (string.IsNullOrEmpty(info.HistoryUseProduct))
                                        {
                                            if (number <= 2000)
                                            {
                                                json = JsonConvert.SerializeObject(new UserServiceModel()
                                                {
                                                    NumberLike = 0,
                                                    DateUse = DateTime.Now,
                                                    Follow = number
                                                });
                                                await UseService(info, json, number);
                                            }
                                            else
                                            {
                                                await ThongBaoKhongHopLe();
                                            }
                                        }
                                        else
                                        {
                                            var history =
                                                JsonConvert.DeserializeObject<UserServiceModel>(info.HistoryUseProduct);
                                            if (history.DateUse.Date == DateTime.Now.Date)
                                            {
                                                var num = (2000 - history.Follow) > 0 ? (2000 - history.Follow) : 0;
                                                if (num >= number)
                                                {
                                                    json = JsonConvert.SerializeObject(new UserServiceModel()
                                                    {
                                                        DateUse = DateTime.Now,
                                                        NumberLike = history.NumberLike,
                                                        Follow = number + history.Follow,
                                                    });
                                                    await UseService(info, json, number);
                                                }
                                                else
                                                {
                                                    await ThongBaoKhongHopLe();
                                                }
                                            }
                                            else
                                            {
                                                if (number <= 2000)
                                                {
                                                    json = JsonConvert.SerializeObject(new UserServiceModel()
                                                    {
                                                        NumberLike = history.NumberLike,
                                                        DateUse = DateTime.Now,
                                                        Follow = number
                                                    });
                                                    await UseService(info, json, number);
                                                }
                                                else
                                                {
                                                    await ThongBaoKhongHopLe();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await ThongBaoKhongHopLe();
                                }
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo",
                                    "Bạn nên nâng cấp tài khoản để sử dụng đầy đủ tính năng", "OK");
                            }
                        }

                        break;
                    default:
                        break;
                }
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

        private async Task ThongBaoKhongHopLe()
        {
            await _pageDialogService.DisplayAlertAsync("Thông báo",
                "Số lượng nhập vào không hợp lệ", "OK");
            Number = null;
        }

        private async Task UseService(LicenseKeyModel info, string json, int number)
        {
            var addHistoryUse = await _licenseKeyService.UpdateHistory(info.LicenseKey, json);
            if (addHistoryUse != null && addHistoryUse.Code > 0 && addHistoryUse.Data != null)
            {
                var contentSend = JsonConvert.SerializeObject(new ContentSendTelegramModel()
                {
                    Ten_Thong_Bao = _itemMenu.TitleItem,
                    Id_Nguoi_Dung = _user.ID,
                    Noi_Dung_Thong_Bao = new
                    {
                        Noi_Dung = Content
                    },
                    So_Luong = number,
                    Ghi_Chu = new
                    {
                        Ten = _user.Name,
                        Tai_Khoan = _user.UserName,
                        So_dien_thoai = _user.NumberPhone
                    }
                }, Formatting.Indented);
                await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, contentSend);
                await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi vui lòng cài lại", "OK");
            }
            await CheckAcountUseService();
        }
    }
}