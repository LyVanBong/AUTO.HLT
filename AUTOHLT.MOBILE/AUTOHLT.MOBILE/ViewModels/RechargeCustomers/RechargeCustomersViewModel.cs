﻿using System;
using System.Globalization;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.RechargeCustomers
{
    public class RechargeCustomersViewModel : ViewModelBase
    {
        private string _userName;
        private string _currentBalance;
        private string _numberMoney;
        private bool _isEnabledButton;
        private IUserService _userService;
        private bool _isLoading;
        private IPageDialogService _pageDialogService;
        private IDatabaseService _databaseService;
        private string _discount;
        private string _totalMoney;
        private string _idRec;
        private ITelegramService _telegramService;


        public ICommand UnfocusedDiscountCommand { get; set; }
        public string TotalMoney
        {
            get => _totalMoney;
            set => SetProperty(ref _totalMoney, value);
        }

        public string Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand TransfersCommand { get; private set; }
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        public string NumberMoney
        {
            get => _numberMoney;
            set => SetProperty(ref _numberMoney, value);
        }

        public ICommand UnfocusedNumberMoneyCommand { get; private set; }
        public string CurrentBalance
        {
            get => _currentBalance;
            set => SetProperty(ref _currentBalance, value);
        }

        public ICommand UnfocusedUserNameCommand { get; private set; }
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public RechargeCustomersViewModel(INavigationService navigationService, IUserService userService, IPageDialogService pageDialogService, IDatabaseService databaseService, ITelegramService telegramService) : base(navigationService)
        {
            _telegramService = telegramService;
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            UnfocusedUserNameCommand = new Command(UnfocusedUserName);
            UnfocusedNumberMoneyCommand = new Command(UnfocusedNumberMoney);
            TransfersCommand = new Command(Transfers);
            IsLoading = true;
            Discount = "50";
            UnfocusedDiscountCommand = new Command(UnfocusedDiscount);
        }

        private void UnfocusedDiscount()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Discount))
                {
                    if (NumberMoney != null)
                    {

                        var number = int.Parse(NumberMoney);
                        var discount = 0.0;
                        if (Discount != null)
                        {
                            var dis = float.Parse(Discount) / 100;
                            discount = number * dis;
                        }

                        TotalMoney = (number + discount).ToString();
                    }
                    else
                    {
                        TotalMoney = "0";
                    }
                }
                else
                {
                    TotalMoney = NumberMoney;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }

        private async void Transfers()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var message = string.Format(Resource._1000067,
                    string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(TotalMoney)), UserName);
                var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, message, "OK", "Cancel");
                if (res)
                {
                    var myMoney = int.Parse(CurrentBalance);
                    var number = int.Parse(NumberMoney);
                    var user = await _databaseService.GetAccountUser();
                    if (user != null)
                    {
                        var data = new UserModel
                        {
                            UserName = UserName,
                            Name = user.Name,
                            Password = user.Password,
                            Email = user.Email,
                            NumberPhone = user.NumberPhone,
                            Sex = user.Sex,
                            Role = user.Role,
                            IsActive = user.IsActive,
                            Age = user.Age,
                            Price = myMoney + int.Parse(TotalMoney),
                            IdDevice = user.IdDevice,
                        };
                        var setMoneyForUser = await _userService.UpdateUser(data.UserName, data.Name, data.Password,
                            data.Email, data.NumberPhone.ToString(), data.Sex.ToString(), data.Role.ToString(),
                            data.IsActive.ToString(), data.Age.ToString(), data.Price.ToString(), data.IdDevice);
                        if (setMoneyForUser != null && setMoneyForUser.Code > 0)
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                            var log = await _userService.HistorySetMoneyForUser(Discount, NumberMoney, user.ID, _idRec,
                                "1");
                            var messageTele = $"Nạp tiền\n" +
                                          $"Id người gửi: {user.ID}\n" +
                                          $"Id người nhận: {_idRec}\n" +
                                          $"Số tiền nạp: {string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(NumberMoney))} VND\n" +
                                          $"Chiết khẩu: {Discount} %\n" +
                                          $"Thời gian chuyển: {DateTime.Now.ToString("F")}";
                            var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWMoneyHistory,
                                messageTele);
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
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
                UserName = string.Empty;
                IsLoading = false;
            }
        }

        private void UnfocusedNumberMoney()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(NumberMoney))
                {
                    if (NumberMoney != null)
                    {

                        var number = int.Parse(NumberMoney);
                        var discount = 0.0;
                        if (Discount != null)
                        {
                            var dis = float.Parse(Discount) / 100;
                            discount = number * dis;
                        }

                        TotalMoney = (number + discount).ToString();
                    }
                    else
                    {
                        TotalMoney = "0";
                    }

                    if (!string.IsNullOrWhiteSpace(UserName))
                        IsEnabledButton = true;
                    else
                        IsEnabledButton = false;
                }
                else
                {
                    TotalMoney = "0";
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

        private async void UnfocusedUserName()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (!string.IsNullOrWhiteSpace(UserName))
                {

                    var user = await _userService.CheckExistAccount(UserName);
                    if (user != null && user.Code < 0)
                    {
                        _idRec = user.Data;
                        var money = await _userService.GetMoneyUser(UserName);
                        if (money != null && money.Code > 0 && money.Data != null)
                        {
                            CurrentBalance = money.Data;
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000037, "OK");
                        UserName = "";
                    }

                    if (!string.IsNullOrWhiteSpace(NumberMoney))
                        IsEnabledButton = true;
                    else
                        IsEnabledButton = false;
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
    }
}