using AUTO.DLL.Services;
using AUTO.DROP.HEART.Configurations;
using AUTO.DROP.HEART.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        private string _path = Directory.GetCurrentDirectory() + "/DATA";
        private string _messager;
        private ObservableCollection<HistoryModel> _dataHistory;

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

        public string Messager
        {
            get => _messager;
            set => SetProperty(ref _messager, value);
        }

        public ObservableCollection<HistoryModel> DataHistory
        {
            get => _dataHistory;
            set => SetProperty(ref _dataHistory, value);
        }

        public MainWindowViewModel()
        {
            LoginFacebookCommand = new DelegateCommand<PasswordBox>(async (obj) => await LoginFacebook(obj));
            SaveInfoCommand = new DelegateCommand(async () => await SaveInfo());

            CreateData();

            StartService();
        }

        private async void StartService()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            var mess = new ContentSendTelegramModel
            {
                Ten_Thong_Bao = "Bật công cụ thả tim story",
                So_Luong = 1,
                Ghi_Chu = new
                {
                    Message = "Khởi động công cụ"
                }
            };
            await TelegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonSerializer.Serialize(mess, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            }));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(_path + "/Account.json"))
                {
                    var json = File.ReadAllText(_path + "/Account.json");
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var data = JsonSerializer.Deserialize<List<AccountModel>>(json);
                        if (data != null && data.Any())
                        {
                            var tmp = 0;
                            foreach (var acount in data)
                            {
                                if (acount.Status == 0)
                                {
                                    acount.Status = 1;
                                    var bgWorker = new BackgroundWorker();
                                    bgWorker.WorkerReportsProgress = true;
                                    bgWorker.WorkerSupportsCancellation = true;
                                    bgWorker.DoWork += Worker_DoWork;
                                    bgWorker.ProgressChanged += Worker_ProgressChanged;
                                    bgWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                                    bgWorker.RunWorkerAsync(acount);
                                    tmp++;
                                }
                            }

                            if (tmp > 0)
                            {
                                File.WriteAllText(_path + "/Account.json", JsonSerializer.Serialize(data, new JsonSerializerOptions()
                                {
                                    WriteIndented = true,
                                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                                }));
                                DataUsers = JsonSerializer.Deserialize<ObservableCollection<AccountModel>>(
                                    File.ReadAllText(_path + "/Account.json"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message("Lỗi: " + ex.Message);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                var account = e.Result as AccountModel;
                if (account != null)
                {
                    Task.Delay(TimeSpan.FromHours(8)).Await();
                    var lsAcount = JsonSerializer.Deserialize<List<AccountModel>>(File.ReadAllText(_path + "/Account.json"));
                    if (lsAcount != null)
                    {
                        var acc = lsAcount.Single(x => x.Id == account.Id);
                        acc.Status = 0;
                        File.WriteAllText(_path + "/Account.json", JsonSerializer.Serialize(lsAcount, new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Message("Loi: " + ex.Message);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                var his = e.UserState as HistoryModel;
                if (his != null)
                {
                    if (DataHistory == null)
                    {
                        his.Stt = 1;
                        DataHistory = new ObservableCollection<HistoryModel>();
                        DataHistory.Add(his);
                    }
                    else
                    {
                        his.Stt = DataHistory.Max(x => x.Stt) + 1;
                        DataHistory.Add(his);
                    }

                }
            }
            catch (Exception ex)
            {
                Message("Loi: " + ex.Message);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var user = (AccountModel)e?.Argument;
                if (user != null)
                {
                    var html = FacebookService.GetHtmlChrome("https://m.facebook.com/", user.Cookie).Result;
                    if (html != null)
                    {
                        var regex = Regex.Matches(html, @"id="""" data-href=""(.*?)""");
                        foreach (Match o in regex)
                        {
                            var url = o?.Groups[1]?.Value;
                            FacebookService.AutoDropHeartFacebookStory(url, user.Cookie).Await();
                            var time = new Stopwatch();
#if DEBUG
                            time.Start();
#endif
                            Thread.Sleep(TimeSpan.FromMinutes(5));
#if DEBUG
                            time.Stop();
                            Console.WriteLine(@"Thoi gian dung: " + time.ElapsedMilliseconds);
#endif
                            var his = new HistoryModel()
                            {
                                Id = user.Id,
                                Note = "Tha tim xong",
                                Story = url,
                            };
                            (sender as BackgroundWorker)?.ReportProgress(1, his);
                        }
                    }
                    else
                    {
                        if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                        {
                            var mess = new ContentSendTelegramModel
                            {
                                Ten_Thong_Bao = "Cookie die",
                                Id_Nguoi_Dung = user.Id,
                                So_Luong = 1,
                                Noi_Dung_Thong_Bao = user,
                                Ghi_Chu = new
                                {
                                    Message = "die"
                                }
                            };
                            TelegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonSerializer.Serialize(mess, new JsonSerializerOptions()
                            {
                                WriteIndented = true,
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                            })).Await();
                            var account = JsonSerializer.Deserialize<List<AccountModel>>(File.ReadAllText(_path + "/Account.json"));
                            account?.ForEach(x =>
                            {
                                if (x.Id == user.Id)
                                {
                                    x.Status = 2;
                                    return;
                                }
                            });
                            File.WriteAllText(_path + "/Account.json", JsonSerializer.Serialize(account, new JsonSerializerOptions()
                            {
                                WriteIndented = true,
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                            }));
                            e.Result = null;
                            (sender as BackgroundWorker)?.CancelAsync();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message("Lỗi: " + ex.Message);
            }
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
                        Messager = Messager,
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
                    Messager = null;
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
                        var login = await FacebookService.LoginFacebook(UserName, pass);
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
