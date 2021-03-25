using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AUTO.DLL.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Xml;
using AUTO.SEND.MESSAGE.FACEBOOK.TOOL.Models;
using Prism.Common;

namespace AUTO.SEND.MESSAGE.FACEBOOK.TOOL.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Gửi tin nhắn tự động";
        private string _userName;
        private string _id;
        private string _cookie;
        private string _token;
        private string _message1 = "Chào buổi sáng {0}";
        private string _message2 = "Buổi trưa vui vẻ {0} nhẽ";
        private string _message3 = "Chúc {0} ngủ ngon mơ đẹp";
        private int _timeDelay = 5;
        private string _path = Directory.GetCurrentDirectory() + "/Data";
        private ObservableCollection<AcountsModel> _dataUsers;
        private string _passwd;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public ICommand LoginFacebookCommand { get; private set; }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Cookie
        {
            get => _cookie;
            set => SetProperty(ref _cookie, value);
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string Message1
        {
            get => _message1;
            set => SetProperty(ref _message1, value);
        }

        public string Message2
        {
            get => _message2;
            set => SetProperty(ref _message1, value);
        }

        public string Message3
        {
            get => _message3;
            set => SetProperty(ref _message3, value);
        }

        public int TimeDelay
        {
            get => _timeDelay;
            set => SetProperty(ref _timeDelay, value);
        }

        public ICommand SaveInfoCommand { get; private set; }

        public ObservableCollection<AcountsModel> DataUsers
        {
            get => _dataUsers;
            set => SetProperty(ref _dataUsers, value);
        }

        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }

        public MainWindowViewModel()
        {
            LoginFacebookCommand = new DelegateCommand<object>(async (pwd) => await LoginFacebook(pwd));
            SaveInfoCommand = new DelegateCommand(async () => await SaveInfo());

            InitData();
        }

        private async Task SaveInfo()
        {
            try
            {
                if (Id != null && Cookie != null && Token != null && Message1 != null && Message2 != null && Message3 != null && TimeDelay > -1)
                {
                    var max = 0;
                    if (DataUsers == null)
                    {
                        DataUsers = new ObservableCollection<AcountsModel>();
                        max = 1;
                    }
                    else
                    {
                        max = DataUsers.Max(x => x.Stt) + 1;
                    }
                    var account = new AcountsModel(Id, max, UserName,
                        Passwd, Cookie, Token, Message1, Message2, Message3, TimeDelay, "", 0);
                    DataUsers.Add(account);
                    File.WriteAllText(_path + "/Acounts.json", JsonSerializer.Serialize(DataUsers, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));
                    Message("Lưu thành công");
                }
                else
                {
                    Message("Dữ liệu nhập chưa đủ");
                }
            }
            catch (Exception e)
            {
                Message("Lỗi: " + e.Message);
            }
        }

        /// <summary>
        /// khởi tạo dữ liệu
        /// </summary>
        private void InitData()
        {
            try
            {
                if (Directory.Exists(_path))
                {
                    var json = File.ReadAllText(_path + "/Acounts.json");
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        DataUsers = JsonSerializer.Deserialize<ObservableCollection<AcountsModel>>(json);
                    }
                }
                else
                {
                    Directory.CreateDirectory(_path);
                    Directory.CreateDirectory(_path + "/Logs");
                    Directory.CreateDirectory(_path + "/Friends");
                    File.Create(_path + "/Acounts.json");
                }
            }
            catch (Exception e)
            {
                Message("Lỗi: " + e.Message);
            }
        }

        private void Message(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// đăng nhập vào facebook lấy cookie token
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private async Task LoginFacebook(object pwd)
        {
            if (pwd != null)
            {
                var pass = ((PasswordBox)pwd).Password;
                if (!string.IsNullOrWhiteSpace(pass) && UserName != null)
                {
                    var login = await FacebookService.Login(UserName, pass);
                    if (login.Token != null && login.Cookie != null)
                    {
                        Passwd = pass;
                        Cookie = login.Cookie;
                        Token = login.Token;
                        Message("Thành công");
                    }
                    else
                    {
                        Message("Đăng nhập chưa thành công");
                    }
                }
                else
                {
                    Message("Tài khoản hoặc mật khẩu còn thiếu");
                }
            }
        }
    }
}
