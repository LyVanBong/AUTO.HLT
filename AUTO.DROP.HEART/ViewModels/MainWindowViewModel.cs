using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AUTO.DLL.Services;
using AUTO.DROP.HEART.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace AUTO.DROP.HEART.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Thả tim story facebook";
        private string _userName;
        private string _passwd;
        private string _cookie;
        private string _token;
        private ObservableCollection<AccountModel> _dataUsers;
        private string _id;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand LoginFacebookCommand { get; private set; }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Cookie
        {
            get => _cookie;
            set => SetProperty(ref _cookie, value);
        }

        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public ObservableCollection<AccountModel> DataUsers
        {
            get => _dataUsers;
            set => SetProperty(ref _dataUsers, value);
        }

        public ICommand SaveInfoCommand { get; private set; }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public MainWindowViewModel()
        {
            LoginFacebookCommand = new DelegateCommand<PasswordBox>(async (obj) => await LoginFacebook(obj));
            SaveInfoCommand = new DelegateCommand(async () => await SaveInfo());

            CreateData();
        }

        private void CreateData()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "/DATA";
                if (Directory.Exists(path))
                {
                    var json = File.ReadAllText(path + "/Account.json");
                    var data = JsonSerializer.Deserialize<List<AccountModel>>(json);
                    if (data != null && data.Any())
                    {
                        DataUsers = new ObservableCollection<AccountModel>(data);
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "/Friends");
                    Directory.CreateDirectory(path + "/Logs");
                }
            }
            catch (Exception e)
            {
                Message("Loi khoi tao du lieu: " + e.Message);
            }
        }

        private async Task SaveInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Cookie) && !string.IsNullOrEmpty(Token))
                {
                    var max = 0;
                    if (DataUsers == null)
                    {
                        DataUsers = new ObservableCollection<AccountModel>();
                        max = 1;
                    }
                    else
                    {
                        max = DataUsers.Max(x => x.Stt) + 1;
                    }
                    DataUsers.Add(new AccountModel()
                    {
                        Stt = max,
                        Cookie = Cookie,
                        Token = Token,
                        Id = Id,
                        Note = "Mới thêm",
                        Passwd = Passwd,
                        UserName = UserName,
                        Status = 0,
                    });
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/DATA/Account.json", JsonSerializer.Serialize(DataUsers, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                    }));

                    Id = null;
                    Cookie = null;
                    Token = null;
                    UserName = null;
                    Message("Lưu thông tin tài khoản thành công");
                }
            }
            catch (Exception e)
            {
                Message("Loi: " + e.Message);
            }
        }

        private async Task LoginFacebook(PasswordBox pwd)
        {
            try
            {
                if (pwd != null)
                {
                    var pass = pwd.Password;
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
            catch (Exception e)
            {
                Message("Loi: " + e.Message);
            }
        }
        /// <summary>
        /// Gui thong bao
        /// </summary>
        /// <param name="message"></param>
        private void Message(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
