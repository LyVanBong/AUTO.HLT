﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Helpers;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Models.User;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.User;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.KeyGeneration
{
    public class KeyGenerationViewModel : ViewModelBase
    {
        private string _userName;
        private string _fullName;
        private string _passwd = "Autovip@12345";
        private string _phoneNumber;
        private ILoginService _loginService;
        private IPageDialogService _pageDialogService;
        private ITelegramService _telegramService;
        private IUserService _userService;
        private int _selectedIndex;
        private ObservableRangeCollection<UserModel> _userData;
        private bool _isLoading;
        private List<UserModel> _userCache;
        private int _countUser;
        private ObservableRangeCollection<UserModel> _agecyData;
        private int _countAgecy;
        private string _searchAgecyTxt;
        private UserModel _agecy;
        private string _amountKey = "10";
        private ILicenseKeyService _licenseKeyService;
        private string _numDateUseKey = "365";

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public ICommand FunctionExecuteCommand { get; private set; }
        public ICommand ResetPasswdCommand { get; private set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public ObservableRangeCollection<UserModel> UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public int CountUser
        {
            get => _countUser;
            set => SetProperty(ref _countUser, value);
        }

        public string SearchUserTxt { get; set; }
        public ICommand SearchUserCommand { get; private set; }
        public ICommand SetRoleCommand { get; private set; }

        public ObservableRangeCollection<UserModel> AgecyData
        {
            get => _agecyData;
            set => SetProperty(ref _agecyData, value);
        }

        public int CountAgecy
        {
            get => _countAgecy;
            set => SetProperty(ref _countAgecy, value);
        }

        public string SearchAgecyTxt { get; set; }
        public ICommand SearchAgecyCommand { get; private set; }

        public UserModel Agecy
        {
            get => _agecy;
            set => SetProperty(ref _agecy, value);
        }

        public string AmountKey
        {
            get => _amountKey;
            set => SetProperty(ref _amountKey, value);
        }

        public ICommand CreateKeyCommand { get; private set; }

        public string NumDateUseKey
        {
            get => _numDateUseKey;
            set => SetProperty(ref _numDateUseKey, value);
        }

        public KeyGenerationViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, ITelegramService telegramService, IUserService userService, ILicenseKeyService licenseKeyService) : base(navigationService)
        {
            _licenseKeyService = licenseKeyService;
            _userService = userService;
            _telegramService = telegramService;
            _loginService = loginService;
            _pageDialogService = pageDialogService;
            FunctionExecuteCommand = new AsyncCommand<string>(async (key) => await FunctionExecute(key));
            ResetPasswdCommand = new AsyncCommand<UserModel>(async (obj) => await ResetPasswd(obj));
            SearchUserCommand = new AsyncCommand(async () => await SearchUser());
            SetRoleCommand = new AsyncCommand<UserModel>(async (model) => await SetRole(model));
            SearchAgecyCommand = new AsyncCommand(async () => await SearchAgecy());
            CreateKeyCommand = new AsyncCommand(async () => await CreateKey());
        }

        private async Task CreateKey()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (Agecy != null && !string.IsNullOrEmpty(Agecy.ID) && !string.IsNullOrEmpty(AmountKey) && !string.IsNullOrEmpty(NumDateUseKey) && Agecy.Role == 3)
                {
                    if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Tạo " + AmountKey + " mã bản quyền cho đại lý " + Agecy.Name, "OK", "Thôi"))
                    {
                        var createKey = await _licenseKeyService.CreateLicense(Agecy.ID, AmountKey,NumDateUseKey);
                        if (createKey != null && createKey.Code > 0)
                        {
                            await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Tạo mã bản quyền",
                                Ghi_Chu = new
                                {
                                    Nguoi_Tao = "Tài khoản do admin tạo"
                                },
                                Id_Nguoi_Dung = Agecy.UserName,
                                So_Luong = 1,
                                Noi_Dung_Thong_Bao = new
                                {
                                    Noi_Dung = "Tạo " + AmountKey + " mã bản quyền, cho tài khoản " + Agecy.UserName + " thành công !",
                                    Tai_Khoan = Agecy.UserName,
                                    So_Dien_Thoi = Agecy.NumberPhone,
                                },
                            }, Formatting.Indented));
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh, vui lòng thử lại.", "OK");
                        }

                        Agecy = new UserModel();
                        AmountKey = "";
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Chưa đủ dữ liệu đầu vào", "OK");
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

        private async Task SearchAgecy()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (AgecyData != null && AgecyData.Any())
                {
                    if (string.IsNullOrEmpty(SearchAgecyTxt))
                    {
                        var agecy = UserData.Where(x => x.Role == 3);
                        AgecyData = new ObservableRangeCollection<UserModel>(agecy);
                    }
                    else
                    {
                        var agecy = UserData.Where(x => x.Role == 3);
                        var find = agecy.Where(x => x.UserName.Contains(SearchAgecyTxt));
                        if (find.Any())
                        {
                            AgecyData = new ObservableRangeCollection<UserModel>(find);
                        }
                    }
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

        private async Task SetRole(UserModel model)
        {
            try
            {
                if (IsLoading)
                    return;
                IsLoading = true;
                if (model != null)
                {

                    if (model.Role == 0)
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Tài khoản " + model.UserName + ", đang là admin bạn không thay đổi quyền tài khoản này !", "OK");
                    }
                    else
                    {
                        if (model.Role == 2)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Nâng quyền cho tài khoản " + model.UserName + ", lên làm đại lý !", "OK", "Thôi"))
                            {
                                model.Role = 3;
                                var updateRole3 = await _userService.UpdateUser(model);
                                if (updateRole3 != null && updateRole3.Code > 0)
                                {
                                    await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                                    {
                                        Ten_Thong_Bao = "Sét quyền",
                                        Ghi_Chu = new
                                        {
                                            Nguoi_Tao = "Tài khoản do admin tạo"
                                        },
                                        Id_Nguoi_Dung = model.UserName,
                                        So_Luong = 1,
                                        Noi_Dung_Thong_Bao = new
                                        {
                                            Noi_Dung = "Nâng quyền cho tài khoản " + model.UserName + ", lên làm đại lý !",
                                            Tai_Khoan = model.UserName,
                                            So_Dien_Thoi = model.NumberPhone,
                                        },
                                    }, Formatting.Indented));
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh, vui lòng thử lại.", "OK");
                                }
                            }
                        }
                        else if (model.Role == 3)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Xóa quyền đại lý của tài khoản " + model.UserName + " !", "OK", "Thôi"))
                            {
                                model.Role = 2;
                                var updateRole2 = await _userService.UpdateUser(model);
                                if (updateRole2 != null && updateRole2.Code > 0)
                                {
                                    await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                                    {
                                        Ten_Thong_Bao = "Sét quyền",
                                        Ghi_Chu = new
                                        {
                                            Nguoi_Tao = "Tài khoản do admin tạo"
                                        },
                                        Id_Nguoi_Dung = model.UserName,
                                        So_Luong = 1,
                                        Noi_Dung_Thong_Bao = new
                                        {
                                            Noi_Dung = "Xóa quyền đại lý của tài khoản " + model.UserName + " !",
                                            Tai_Khoan = model.UserName,
                                            So_Dien_Thoi = model.NumberPhone,
                                        },
                                    }, Formatting.Indented));
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh, vui lòng thử lại.", "OK");
                                }
                            }
                        }
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh !", "OK");
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                await InitializeData();
                IsLoading = false;
            }
        }

        private async Task SearchUser()
        {
            try
            {
                if (IsLoading)
                    return;
                IsLoading = true;

                if (string.IsNullOrEmpty(SearchUserTxt))
                {
                    await InitializeData();
                }
                else
                {
                    var allUser = await AllUser();
                    if (allUser != null && allUser.Any())
                    {
                        var findUserName = allUser.Where(x => x.UserName.Contains(SearchUserTxt));
                        if (findUserName.Any())
                        {
                            UserData = new ObservableRangeCollection<UserModel>(findUserName);
                        }
                        else
                        {
                            var findId = allUser.Where(x => x.ID.Contains(SearchUserTxt));
                            if (findId.Any())
                            {
                                UserData = new ObservableRangeCollection<UserModel>(findId);
                            }
                            else
                            {
                                var findPhone = allUser.Where(x => x.NumberPhone.Contains(SearchUserTxt));
                                if (findPhone.Any())
                                {
                                    UserData = new ObservableRangeCollection<UserModel>(findPhone);
                                }
                                else
                                {
                                    var findName = allUser.Where(x => x.Name.Contains(SearchUserTxt));
                                    if (findName.Any())
                                    {
                                        UserData = new ObservableRangeCollection<UserModel>(findName);
                                    }
                                }
                            }
                        }

                        CountUser = UserData.Count;
                    }
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            await InitializeData();
            IsLoading = false;
        }

        private async Task InitializeData()
        {
            var data = await AllUser();
            if (data != null)
            {
                UserData = new ObservableRangeCollection<UserModel>(data);
                CountUser = data.Count;
                var agecy = data.Where(x => x.Role == 3);
                if (agecy.Any())
                {
                    AgecyData = new ObservableRangeCollection<UserModel>(agecy);
                    CountAgecy = AgecyData.Count;
                }
            }
        }

        private async Task<List<UserModel>> AllUser()
        {
            try
            {
                var data = await _userService.GetAllUser();
                if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                {
                    return data.Data;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }
        private async Task ResetPasswd(UserModel obj)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (obj != null)
                {
                    if (await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn muốn đặt lại mật khẩu tài khoản " + obj.UserName + ", mật khẩu mặc định là: Autovip@12345",
                        "Đặt lại ngay", "Thôi"))
                    {
                        obj.Password = HashFunctionHelper.GetHashCode("Autovip@12345", 1);
                        var chanagePass = await _userService.UpdateUser(obj);
                        if (chanagePass != null && chanagePass.Code > 0)
                        {
                            await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                            {
                                Ten_Thong_Bao = "Đặt lại mật khẩu",
                                Ghi_Chu = new
                                {
                                    Nguoi_Tao = "Tài khoản do admin tạo"
                                },
                                Id_Nguoi_Dung = obj.UserName,
                                So_Luong = 1,
                                Noi_Dung_Thong_Bao = new
                                {
                                    Noi_Dung = "Đặt lại mật khẩu cho khách hàng",
                                    Tai_Khoan = obj.UserName,
                                    So_Dien_Thoi = obj.NumberPhone,
                                },
                            }, Formatting.Indented));
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Đổi mật khẩu thành công !", "OK");
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh, vui lòng thử lại.", "OK");
                        }
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh !", "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                await InitializeData();
                IsLoading = false;
            }
        }

        private async Task FunctionExecute(string key)
        {
            try
            {
                switch (key)
                {
                    case "0":
                        await DoSigup();
                        break;
                    case "1":
                        if (string.IsNullOrEmpty(PhoneNumber)) return;
                        await CheckPhone(PhoneNumber);
                        break;
                    case "2":
                        if (string.IsNullOrEmpty(UserName)) return;
                        await CheckUserName(UserName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
        private async Task DoSigup()
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Passwd))
                {
                    var sigup = await _loginService.Sigup(new SigupModel
                    {
                        UserName = UserName,
                        Name = FullName,
                        NumberPhone = PhoneNumber.Replace(" ", ""),
                        Password = Passwd
                    });
                    if (sigup != null && sigup.Code > 0)
                    {
                        await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                        {
                            Ten_Thong_Bao = "Đăng ký tài khoản mới",
                            Ghi_Chu = new
                            {
                                Nguoi_Tao = "Tài khoản do admin tạo"
                            },
                            Id_Nguoi_Dung = UserName,
                            So_Luong = 1,
                            Noi_Dung_Thong_Bao = new
                            {
                                Tai_Khoan = UserName,
                                Trang_Thai = "Đăng ký thành công"
                            },
                        }, Formatting.Indented));
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Tạo tài khoản thành công",
                            "OK");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Tạo tài khoản lỗi vui lòng thử lại",
                            "OK");
                        Passwd = "";
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng điền đầy đủ thông tin",
                        "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo",
                    "Lỗi phát sinh trong quá trình xử lý vui lòng thử lại",
                    "OK");
            }
            finally
            {
                UserName = Passwd = PhoneNumber = FullName = "";
            }
        }
        private async Task CheckPhone(string phoneNumber)
        {
            try
            {
                if (PhoneNumber != null)
                {
                    var phone = PhoneNumber.Replace(" ", "");
                    if (phone.Length == 10)
                    {
                        var data = await _loginService.CheckExistPhone(phone.Replace(" ", ""));
                        if (data != null && data.Code > 0)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Số điện thoại {data.Data} đã được đăng ký bởi một tài khoản khác", "OK");
                            PhoneNumber = "";
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", $"Số điện thoại {phone} chưa chính xác", "OK");
                        PhoneNumber = "";
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
        private async Task CheckUserName(string userName)
        {
            try
            {
                if (userName != null)
                {
                    var usr = Regex.Match(UserName, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                    if (!string.IsNullOrWhiteSpace(usr))
                    {
                        var data = await _loginService.CheckExistUser(usr);
                        if (data != null && data.Code < 0)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Tài khoản {UserName} đã tồn tại", "OK");
                            UserName = "";
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", $"Tên đăng nhập {UserName} của bạn chứa các ký tự đặc biệt vui lòng nhập lại", "OK");
                        UserName = "";
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}