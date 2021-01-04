using System;
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

namespace AUTOHLT.MOBILE.ViewModels.Transfers
{
    public class TransferViewModel : ViewModelBase
    {
        private bool _isSaveContacts;
        private string _userName;
        private string _amountMoney;
        private bool _isEnabledTransfers;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private string _idReceive;
        private UserModel _userModel;
        private ITelegramService _telegramService;

        public ICommand UnfocusedAmountMoneyCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand UnfocusedUserNameCommand { get; private set; }
        public bool IsEnabledTransfers
        {
            get => _isEnabledTransfers;
            set => SetProperty(ref _isEnabledTransfers, value);
        }

        public ICommand TransfersCommand { get; private set; }
        public ICommand ContactsCommand { get; private set; }
        public string AmountMoney
        {
            get => _amountMoney;
            set => SetProperty(ref _amountMoney, value, CheckInputTransfers);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value, CheckInputTransfers);
        }

        public bool IsSaveContacts
        {
            get => _isSaveContacts;
            set => SetProperty(ref _isSaveContacts, value);
        }

        public TransferViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserService userService, IDatabaseService databaseService, ITelegramService telegramService) : base(navigationService)
        {
            _telegramService = telegramService;
            _databaseService = databaseService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            ContactsCommand = new Command(Contacts);
            TransfersCommand = new Command(Transfers);
            UnfocusedUserNameCommand = new Command(UnfocusedUserName);
            UnfocusedAmountMoneyCommand = new Command(UnfocusedAmountMoney);
        }

        private async void UnfocusedAmountMoney()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var user = await _databaseService.GetAccountUser();
                if (user != null)
                {
                    _userModel = user;
                    var data = await _userService.GetMoneyUser(_userModel.UserName);
                    if (data != null && data.Code > 0)
                    {
                        var money = data.Data.Replace(".0000", "");
                        var myPrice = int.Parse(money);
                        var priceInput = int.Parse(AmountMoney);
                        if (myPrice < priceInput)
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000048, "OK");
                            AmountMoney = myPrice + "";
                        }
                        else
                        {
                            AmountMoney = "";
                        }
                    }
                    else
                    {
                        AmountMoney = "";
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

        private void CheckInputTransfers()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(AmountMoney))
            {
                IsEnabledTransfers = true;
            }
            else
            {
                IsEnabledTransfers = false;
            }
        }
        private async void UnfocusedUserName()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName)) return;
                if (IsLoading) return;
                IsLoading = true;
                var data = await _userService.CheckExistAccount(UserName);
                if (data != null && data.Code > 0)
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000046, "OK");
                    UserName = "";
                }
                _idReceive = data.Data;
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

        private async void Transfers()
        {
            try
            {
                IsLoading = false;
                if (IsLoading) return;
                IsLoading = true;
                var user = _userModel;
                if (user != null)
                {
                    var id = user.ID;
                    var data = await _userService.TransferMoney(id, _idReceive, AmountMoney);
                    if (data != null && data.Code > 0 && data.Data == "2")
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000047, "OK");
                        var message = $"Chuyển tiền\n" +
                                      $"Id người gửi: {id}\n" +
                                      $"Id người nhận: {_idReceive}\n" +
                                      $"Số tiền chuyển: {string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(AmountMoney))} VND\n" +
                                      $"Thời gian chuyển: {DateTime.Now.ToString("F")}";
                        var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWMoneyHistory,
                            message);
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "0k");
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                UserName = "";
                AmountMoney = "";
                IsLoading = false;
            }
        }

        private void Contacts()
        {

        }
    }
}