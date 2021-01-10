using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.VerifyOtp
{
    public class OtpViewModel : BindableBase, IDialogAware
    {
        private string _veryCode;
        private bool _isEnabledLogin;
        private IUserService _userService;
        private double _progressBar;
        private int _coutSubmit;
        private string _codeOtp;

        public double ProgressBar
        {
            get => _progressBar;
            set => SetProperty(ref _progressBar, value);
        }

        public bool IsEnabledLogin
        {
            get => _isEnabledLogin;
            set => SetProperty(ref _isEnabledLogin, value);
        }

        public ICommand UseServiceCommand { get; private set; }
        public string VeryCode
        {
            get => _veryCode;
            set => SetProperty(ref _veryCode, value);
        }

        public OtpViewModel(IUserService userService)
        {
            _userService = userService;
            UseServiceCommand = new Command(UseService);
        }

        private void UseService()
        {
            try
            {
                if (VeryCode != null && VeryCode.Length == 6 && _coutSubmit < 3)
                {
                    _coutSubmit++;
                    if (_codeOtp == VeryCode)
                    {
                        ClosePopup("1");
                    }
                    else
                    {
                        VeryCode = "";
                    }

                    if (_coutSubmit ==2)
                    {
                        ClosePopup("0");
                    }
                }
                else
                {
                    VeryCode = "";
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public bool CanCloseDialog() => true;

        private void ClosePopup(string para)
        {
            if (RequestClose != null)
            {
                var pa = new DialogParameters();
                pa.Add("OtpSms",para);
                RequestClose(pa);
            }
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                var phone = parameters.GetValue<string>("NumberPhone");
                if (phone != null)
                {
                    var sendOtp = await _userService.SendOtp(phone);
                    if (sendOtp != null && sendOtp.Code > 0)
                    {
                        _codeOtp = sendOtp.Data;
                        var i = 0;
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            i++;
                            ProgressBar = i;
                            if (i == 120)
                            {
                                ClosePopup("0");
                            }
                            return i < 120;
                        });
                    }
                }
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}