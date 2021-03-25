using AUTO.DLL.Services;
using AUTO.SEND.MESSAGE.FACEBOOK.TOOL.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        private AcountsModel _selectUser;

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

        public AcountsModel SelectUser
        {
            get => _selectUser;
            set
            {
                if (SetProperty(ref _selectUser, value))
                {


                }
            }
        }

        public MainWindowViewModel()
        {
            LoginFacebookCommand = new DelegateCommand<object>(async (pwd) => await LoginFacebook(pwd));
            SaveInfoCommand = new DelegateCommand(async () => await SaveInfo());

            InitData();

            StartService();
        }

        private async void StartService()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine($"Timer đang chạy : {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
#endif
            if (File.Exists(_path + "/Acounts.json"))
            {
                var json = File.ReadAllText(_path + "/Acounts.json");
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var data = JsonSerializer.Deserialize<List<AcountsModel>>(json);
                    if (data != null && data.Any())
                    {
                        var tmp = 0;
                        foreach (var acount in data)
                        {
                            if (acount.Status == 0)
                            {
                                acount.Status = 1;
                                acount.BackgroundWorker = new BackgroundWorker();
                                acount.BackgroundWorker.WorkerReportsProgress = true;
                                acount.BackgroundWorker.WorkerSupportsCancellation = true;
                                acount.BackgroundWorker.DoWork += Worker_DoWork;
                                acount.BackgroundWorker.ProgressChanged += Worker_ProgressChanged;
                                acount.BackgroundWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                                acount.BackgroundWorker.RunWorkerAsync(acount);
                                tmp++;
                            }
                        }

                        if (tmp > 0)
                        {
                            File.WriteAllText(_path + "/Acounts.json", JsonSerializer.Serialize(data, new JsonSerializerOptions()
                            {
                                WriteIndented = true
                            }));
                            DataUsers = JsonSerializer.Deserialize<ObservableCollection<AcountsModel>>(
                                File.ReadAllText(_path + "/Acounts.json"));
                        }
                    }
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState != null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Message("Lỗi : " + ex.Message);
            }
        }

        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var user = (AcountsModel)e?.Argument;
            try
            {
                if (user != null)
                {
                    var fileFriend = _path + "/Friends/" + user.Id + ".json";
                    var json = File.ReadAllText(fileFriend);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var data = JsonSerializer.Deserialize<List<FriendModel>>(json);
                        if (data != null && data.Any())
                        {
                            var random = new Random();
                            foreach (var friend in data)
                            {
                                if (!friend.IsSend)
                                {
                                    var message = string.Format(user.Message1, friend.name);
                                    var note = "";
                                   
                                    if (await FacebookService.SendMessage(message, friend.id, user.Cookie))
                                    {
                                        (sender as BackgroundWorker)?.ReportProgress(1, new HistoryModel
                                        {
                                            Id = user.Id,
                                            NameFriend = friend.name,
                                            IdFriend = friend.id,
                                            Message = message,
                                            TimeSend = DateTime.Now,
                                            Note = note,
                                        });
                                        await Task.Delay(
                                            TimeSpan.FromMinutes(random.Next(user.TimeDelay - 1, user.TimeDelay + 1)));
                                    }
                                    friend.IsSend = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message("Lỗi : " + ex.Message);
            }
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
                        Passwd, Cookie, Token, Message1, Message2, Message3, TimeDelay, "", 0, null);
                    DataUsers.Add(account);
                    File.WriteAllText(_path + "/Acounts.json", JsonSerializer.Serialize(DataUsers, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));
                    var data = await FacebookService.GetIdFriends(Token);
                    var lsFriend = data.data as List<FriendModel>;
                    File.WriteAllText(_path + @"/Friends/" + Id + ".json", JsonSerializer.Serialize(lsFriend, new JsonSerializerOptions()
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
