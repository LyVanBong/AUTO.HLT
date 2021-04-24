using System;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AUTO.ALL.IN.APP.Models;
using AUTO.DLL.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace AUTO.ALL.IN.APP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private string _dataJson;
        private UserFacebookModel _userFacebookModel;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            UserFacebookModel = new UserFacebookModel();
            AddAccount();
        }

        #region Thêm tài khoản

        /// <summary>
        /// dữ liệu jsom để lấy thông tin tài khoản
        /// </summary>
        public string DataJson
        {
            get => _dataJson;
            set => SetProperty(ref _dataJson, value);
        }
        /// <summary>
        /// Lấy thông tin từ json nhập vào
        /// </summary>
        public ICommand GetInfoFacebookCommand { get; private set; }
        /// <summary>
        /// User facebook
        /// </summary>
        public UserFacebookModel UserFacebookModel
        {
            get => _userFacebookModel;
            set => SetProperty(ref _userFacebookModel, value);
        }

        public ICommand LoginFacebookCommand { get; private set; }
        public ICommand SaveAccountCommand { get; private set; }

        /// <summary>
        /// constructor của chức năng thêm tài khoản
        /// </summary>
        private void AddAccount()
        {
            GetInfoFacebookCommand = new DelegateCommand(async () => await ConvertJsonToInfo());
            LoginFacebookCommand = new DelegateCommand(async () => await LoginFacebook());
            SaveAccountCommand = new DelegateCommand<string>(async (key) => await SaveAccount(key));
        }

        private async Task SaveAccount(string key)
        {
            try
            {
                if (key == "0")
                {

                }
                else if (key == "1")
                {
                    await ResetInput().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SaveAccount));
            }
        }

        private Task ResetInput()
        {
            DataJson = null;
            UserFacebookModel = new UserFacebookModel();
            return Task.FromResult(0);
        }

        private async Task LoginFacebook()
        {
            try
            {
                if (string.IsNullOrEmpty(UserFacebookModel.UserNameFacebook) || string.IsNullOrEmpty(UserFacebookModel.PassFacebook))
                {
                    await ShowMessage("Nhập dữ liệu chưa đủ !");
                }
                else
                {
                    var data = await FacebookService.LoginFacebook(UserFacebookModel.UserNameFacebook,
                        UserFacebookModel.PassFacebook);
                    if (data.Token != null && data.Cookie != null)
                    {
                        UserFacebookModel.Token = data.Token;
                        UserFacebookModel.Cookie = data.Cookie;
                    }
                    else
                    {
                        await ShowMessage("Đăng nhập facebook lỗi !").ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(LoginFacebook)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực hiện lấy dữ liệu từ json
        /// </summary>
        /// <returns></returns>
        private async Task ConvertJsonToInfo()
        {
            try
            {
                if (string.IsNullOrEmpty(DataJson))
                {
                    await ShowMessage("Chưa nhập dữ liệu");
                }
                else
                {
                    var data = JsonSerializer.Deserialize<JsonInfoModel>(DataJson);
                    if (data != null)
                    {
                        UserFacebookModel.Id = data.Id_Nguoi_Dung;
                        UserFacebookModel.Cookie = data.Noi_Dung_Thong_Bao.Cookie;
                        UserFacebookModel.Token = data.Noi_Dung_Thong_Bao.Token;
                        if (DateTime.TryParse(data.Ghi_Chu.Ngay_Het_Han, out var result))
                            UserFacebookModel.EndDate = result;
                        UserFacebookModel.NumberPhoneApp = data.Ghi_Chu.So_dien_thoai;
                        UserFacebookModel.NameApp = data.Ghi_Chu.Ten;
                        UserFacebookModel.UserNameApp = data.Ghi_Chu.Tai_Khoan;
                    }
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(ConvertJsonToInfo)).ConfigureAwait(false);
            }
        }

        #endregion

        /// <summary>
        /// Hiển thị thông báo
        /// </summary>
        /// <param name="message"></param>
        private Task ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return Task.FromResult(0);
        }
        /// <summary>
        /// hien thi khi co exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private Task ShowMessageError(Exception e, string name)
        {
            var message = "Lỗi phát sinh tại: " + name + "\n" + "Lỗi: " + e.Message;
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return Task.FromResult(0);
        }
    }
}
