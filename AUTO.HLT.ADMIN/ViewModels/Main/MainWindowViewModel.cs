using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.Main;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.Facebook;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AUTO.HLT.ADMIN.Configurations;
using AUTO.HLT.ADMIN.Models.Telegram;
using AUTO.HLT.ADMIN.Services.Telegram;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private ObservableCollection<UserModel> _dataUsers;
        private string _id;
        private string _ngayDangKy;
        private int _thoiHan;
        private string _cookie;
        private string _token;
        private bool _thaTim;
        private bool _tuongTacAnhDaiDien;
        private string _searchAccount;
        private UserModel _selectUser;
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "/Data_Auto";
        private string _pathIdFacebook = AppDomain.CurrentDomain.BaseDirectory + "/Data_Auto/Id_Facebook";
        private string _pathLog = AppDomain.CurrentDomain.BaseDirectory + "/Data_Auto/Log";
        private string _fileNameAccount = AppDomain.CurrentDomain.BaseDirectory + "/Data_Auto/Account_Facebook.json";
        private Visibility _loading;
        private IFacebookService _facebookService;
        private List<Datum> _lsUID;
        private ObservableCollection<HistoryModel> _dataHistory;
        private ITelegramService _telegramService;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<UserModel> DataUsers
        {
            get => _dataUsers;
            set => SetProperty(ref _dataUsers, value);
        }

        public ICommand SaveAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }


        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string NgayDangKy
        {
            get => _ngayDangKy;
            set => SetProperty(ref _ngayDangKy, value);
        }

        public int ThoiHan
        {
            get => _thoiHan;
            set => SetProperty(ref _thoiHan, value);
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

        public bool ThaTim
        {
            get => _thaTim;
            set => SetProperty(ref _thaTim, value);
        }

        public bool TuongTacAnhDaiDien
        {
            get => _tuongTacAnhDaiDien;
            set => SetProperty(ref _tuongTacAnhDaiDien, value);
        }

        public string SearchAccount
        {
            get => _searchAccount;
            set
            {
                if (SetProperty(ref _searchAccount, value))
                {
                    if (string.IsNullOrWhiteSpace(_searchAccount))
                        GetAccount();
                }
            }
        }
        public ICommand SearchAccountCommand { get; private set; }

        public UserModel SelectUser
        {
            get => _selectUser;

            set
            {
                if (SetProperty(ref _selectUser, value))
                {
                    if (_selectUser != null && _selectUser.TrangThai != 1)
                    {
                        ID = _selectUser.ID;
                        NgayDangKy = _selectUser.NgayDangKy.ToShortDateString();
                        ThoiHan = _selectUser.ThoiHan;
                        Cookie = _selectUser.Cookie;
                        Token = _selectUser.Token;
                        ThaTim = _selectUser.ThaTim;
                        TuongTacAnhDaiDien = _selectUser.TuongTacAnhDaiDien;
                    }
                }
            }
        }

        public Visibility Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public ObservableCollection<HistoryModel> DataHistory
        {
            get => _dataHistory;
            set => SetProperty(ref _dataHistory, value);
        }

        public MainWindowViewModel(IFacebookService facebookService, ITelegramService telegramService)
        {
            _telegramService = telegramService;
            _facebookService = facebookService;
            Loading = Visibility.Visible;
            SaveAccountCommand = new DelegateCommand(SaveAccount);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
            SearchAccountCommand = new DelegateCommand(async () => await SearchAccountUser());
            CreateDirectory();
            Loading = Visibility.Hidden;
            StartServiceAuto();
        }

        private void StartServiceAuto()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Tick += Timer_Tick;
            timer.Start();
            var message = new ContentSendTelegramModel
            {
                Ten_Thong_Bao = "Khởi động tool",
                Id_Nguoi_Dung = "Tool bắt đầu chạy",
                So_Luong = 1,
                Noi_Dung_Thong_Bao = null,
                Ghi_Chu = new
                {
                    Message = "chay tool"
                }
            };
            _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(message, Formatting.Indented));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine(@"Timer đang chạy cẩn thân nhẽ");
#endif
            try
            {
                var json = "";
                if (File.Exists(_fileNameAccount))
                    json = File.ReadAllText(_fileNameAccount);
                else
                    MessageBoxNoti("Chưa có dữ liệu");
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var data = JsonConvert.DeserializeObject<List<UserModel>>(json);
                    if (data != null && data.Any())
                    {
                        var tmp = 0;
                        foreach (var user in data)
                        {
                            if (user.TrangThai == 0 && user.NgayHetHan >= DateTime.UtcNow.Date)
                            {
                                var worker = new BackgroundWorker();
                                worker.WorkerReportsProgress = true;
                                worker.WorkerSupportsCancellation = true;
                                worker.DoWork += Worker_DoWork;
                                worker.ProgressChanged += Worker_ProgressChanged;
                                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                                worker.RunWorkerAsync(user);
                                user.TrangThai = 1;
                                tmp++;
                            }
                        }
                        if (tmp > 0)
                        {
                            WriteAllFile(data, _fileNameAccount);
                            GetAccount();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxNoti($"Lỗi phát sinh: {ex.Message}");
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                var data = e.Result as UserModel;
                if (data != null)
                {
                    var json = File.ReadAllText(_fileNameAccount);
                    if (json != null)
                    {
                        var LsUser = JsonConvert.DeserializeObject<List<UserModel>>(json);
                        if (LsUser != null && LsUser.Any())
                        {
                            var user = LsUser.Find(x => x.ID == data.ID);
                            user.TrangThai = 0;
                            File.WriteAllText(_fileNameAccount, JsonConvert.SerializeObject(LsUser));
                            var message = new ContentSendTelegramModel
                            {
                                Ten_Thong_Bao = "Chạy auto xong",
                                Id_Nguoi_Dung = data.ID,
                                So_Luong = 1,
                                Noi_Dung_Thong_Bao = data,
                                Ghi_Chu = new
                                {
                                    Message = "Đã chạy auto cho tất cả bạn bè"
                                }
                            };
                            _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(message, Formatting.Indented));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState != null)
                {
                    var data = e.UserState as HistoryModel;
                    if (data != null)
                    {
                        if (e.ProgressPercentage == 1)
                        {
                            if (DataHistory == null) DataHistory = new ObservableCollection<HistoryModel>();
                            DataHistory.Add(data);
                            var log = data.Id + ";" + data.Id_Post + ";" + data.Name_Friend_Facebook + ";" + data.UId + ";" +
                                      data.Uid_Facebooke_Friend + ";" + data.Type_Auto + ";" + data.Note_Auto + ";" + data.Time;
                            var pathFileLog = _pathLog + "/log_" + data.Id + ".json";
                            File.AppendAllLines(pathFileLog, new string[] { log });
                        }
                        else
                        {
                            var json = File.ReadAllText(_fileNameAccount);
                            var lsUser = JsonConvert.DeserializeObject<List<UserModel>>(json);
                            var usr = lsUser.Find(x => x.ID == data.Id);
                            usr.TrangThai = 2;
                            File.WriteAllText(_fileNameAccount, json);
                            var message = new ContentSendTelegramModel
                            {
                                Ten_Thong_Bao = "Token hoặc Cookie Facebook chết",
                                Id_Nguoi_Dung = data.Id,
                                So_Luong = 1,
                                Noi_Dung_Thong_Bao = data,
                                Ghi_Chu = new
                                {
                                    Message = "Liên hệ khách để cài lại"
                                }
                            };
                            _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(message, Formatting.Indented));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var user = (UserModel)e.Argument;
            try
            {
                if (user != null)
                {
                    var pathFileName = _pathIdFacebook + "/ID_" + user.ID + ".json";
#if DEBUG
                    Console.WriteLine($"duong dang file danh sach uid {pathFileName}");
#endif
                    var json = File.ReadAllText(pathFileName);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var data = JsonConvert.DeserializeObject<FriendsModel>(json);
                        if (data != null && data.data.Any())
                        {
                            var random = new Random();
                            var uid = random.Next(10) > 5
                                ? data.data.OrderBy(x => x.name)
                                : data.data.OrderByDescending(x => x.name);
                            foreach (var id in uid)
                            {
                                if (user.TuongTacAnhDaiDien)
                                {
                                    var urlface = $"https://d.facebook.com/{id.id}";

                                    var htmlProfile = HtmlProfile(urlface, user.Cookie).Result;
                                    if (!string.IsNullOrWhiteSpace(htmlProfile))
                                    {
                                        var autoLike = AutoLike(htmlProfile, "https://d.facebook.com", user.Cookie).Result;
                                        if (autoLike)
                                        {
                                            var his = new HistoryModel
                                            {
                                                Id = user.ID,
                                                Id_Post = null,
                                                Type_Auto = "Thả tim ảnh đại diện",
                                                UId = id.id,
                                                Uid_Facebooke_Friend = id.id,
                                                Name_Friend_Facebook = id.name,
                                                Note_Auto = "Thả tim ảnh đại diện thành công"
                                            };
                                            (sender as BackgroundWorker)?.ReportProgress(1, his);
                                        }
                                    }
                                    else
                                    {
                                        CheckCookieToken(sender, user.Token, user.Cookie, user);
                                    }

                                    Task.Delay(TimeSpan.FromMinutes(random.Next(18, 22))).Wait();

                                    var htmlProfile2 = HtmlProfile(urlface, user.Cookie).Result;
                                    if (!string.IsNullOrWhiteSpace(htmlProfile2))
                                    {
                                        var autoComment = AutoComment(htmlProfile2, "https://d.facebook.com", user?.Cookie).Result;
                                        if (autoComment)
                                        {
                                            var his = new HistoryModel
                                            {
                                                Id = user.ID,
                                                Id_Post = null,
                                                Type_Auto = "Bình luận ảnh đại diện",
                                                UId = id.id,
                                                Uid_Facebooke_Friend = id.id,
                                                Name_Friend_Facebook = id.name,
                                                Note_Auto = "Bình luận ảnh đại diện thành công"
                                            };
                                            (sender as BackgroundWorker)?.ReportProgress(1, his);
                                        }
                                    }
                                    else
                                    {
                                        CheckCookieToken(sender, user.Token, user.Cookie, user);
                                    }
                                    Task.Delay(TimeSpan.FromMinutes(random.Next(18, 22))).Wait();
                                }

                                var post = _facebookService.GetIdPostFriends("2", user.Token, id.id).Result;
                                if (post != null)
                                {
                                    var idp = post?.posts?.data;
                                    if (user.ThaTim && idp != null && idp.Length > 0)
                                    {
                                        foreach (var datum in idp)
                                        {
                                            var urlface = $"https://d.facebook.com/{datum.id}";
                                            var htmlProfile =
                                                _facebookService.GetHtmlFacebook(urlface, user.Cookie);
                                            if (!string.IsNullOrWhiteSpace(htmlProfile.Result))
                                            {
                                                var tim = AutoThaTim(htmlProfile.Result, "https://d.facebook.com",
                                                    user.Cookie).Result;
                                                if (tim)
                                                {
                                                    var his = new HistoryModel
                                                    {
                                                        Id = user.ID,
                                                        Id_Post = datum.id,
                                                        Type_Auto = "Thả tim",
                                                        UId = post.id,
                                                        Uid_Facebooke_Friend = id.id,
                                                        Name_Friend_Facebook = id.name,
                                                        Note_Auto = "Thả tim thành công"
                                                    };
                                                    (sender as BackgroundWorker)?.ReportProgress(1, his);
                                                }
                                            }
                                            else
                                            {
                                                CheckCookieToken(sender, user?.Token, user?.Cookie, user);
                                            }

                                            Task.Delay(TimeSpan.FromMinutes(random.Next(18, 22))).Wait();
                                        }
                                    }

                                    if (user.TuongTacAnhDaiDien)
                                    {

                                    }
                                }
                                else
                                {
                                    CheckCookieToken(sender, user?.Token, user?.Cookie, user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxNoti("Lỗi: " + ex.Message);
            }
            finally
            {
                e.Result = user;
            }

            Task.Delay(TimeSpan.FromMinutes(30)).Await();
            e.Result = 0;
        }
        /// <summary>
        /// auto comment avatar
        /// </summary>
        /// <param name="htmlProfile"></param>
        /// <param name="urlface"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task<bool> AutoComment(string htmlProfile, string urlface, string cookie)
        {
            if (!string.IsNullOrWhiteSpace(htmlProfile))
            {
                var urlPostComment = Regex.Match(htmlProfile, @"""><form method=""post"" action=""(.*?)""")?.Groups[1]
                    ?.Value;
                if (!string.IsNullOrWhiteSpace(urlPostComment))
                {
                    var url = HttpUtility.HtmlDecode(urlface + urlPostComment);
                    var fb_dtsg = Regex.Match(htmlProfile, @"""><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")
                        ?.Groups[1]?.Value;
                    var jazoest = Regex.Match(htmlProfile, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)""")
                        ?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(fb_dtsg) && !string.IsNullOrWhiteSpace(jazoest))
                    {
                        var ran = new Random();
                        var para = new List<RequestParameter>()
                        {
                            new RequestParameter("fb_dtsg",fb_dtsg),
                            new RequestParameter("jazoest",jazoest),
                            new RequestParameter("comment_text","<3"),
                        };
                        var html = await _facebookService.PostHtmlFacebook(url, cookie, para);
                        if (html != null && html.Contains("ref_component=mbasic_home_logo"))
                            return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// auto like avatar
        /// </summary>
        /// <param name="htmlProfile"></param>
        /// <param name="urlface"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task<bool> AutoLike(string htmlProfile, string urlface, string cookie)
        {
            if (!string.IsNullOrWhiteSpace(htmlProfile))
            {
                var datareactions = Regex.Match(htmlProfile, @"<a href=""/reactions/picker/\?is_permalink=1.*?</a>")?.Value;
                if (!string.IsNullOrWhiteSpace(datareactions))
                {
                    var reac = Regex.Match(datareactions, @"<a href=""(.*?)""")?.Groups[1]?.Value + "\"";
                    var para = new List<RequestParameter>()
            {
                new RequestParameter("is_permalink", Regex.Match(reac, @"/?is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ft_id", Regex.Match(reac, @"&amp;ft_id=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("origin_uri", Regex.Match(reac, @"&amp;origin_uri=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(reac, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("refid", Regex.Match(reac, @"&amp;refid=(.*?)""")?.Groups[1]?.Value),
            };
                    var htmlReaction = await _facebookService.GetHtmlFacebook($"{urlface}/reactions/picker", cookie, para);

                    Regex regex = new Regex(@"<a href=""/ufi/reaction/.*?</a>",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlReaction);
                    var ran = new Random();
                    var like = matchCollection[ran.Next(7)]?.Value;
                    var paraLike = new List<RequestParameter>()
            {
                new RequestParameter("ft_ent_identifier", Regex.Match(like, @"/?ft_ent_identifier=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("reaction_type", Regex.Match(like, @"&amp;reaction_type=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("is_permalink", Regex.Match(like, @"&amp;is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("basic_origin_uri", Regex.Match(like, @"amp;basic_origin_uri=(.*?)&")?.Groups[1]?.Value),
                // new RequestParameter("_ft_",Regex.Match(like,@"&amp;_ft_(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(like, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ext", Regex.Match(like, @"&amp;ext=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("hash", Regex.Match(like, @"&amp;hash=(.*?)""")?.Groups[1]?.Value),
            };
                    var uriLike = $"{urlface}{Regex.Match(like, @"<a href=""(.*?)""")?.Groups[1]?.Value}";
                    var actionLike = await _facebookService.GetHtmlFacebook(HttpUtility.HtmlDecode(uriLike), cookie);
                    if (actionLike != null && actionLike.Contains("ref_component=mbasic_home_logo"))
                        return true;
                }
            }

            return false;
        }
        private async Task<string> HtmlProfile(string urlface, string cookie)
        {
            var html = await _facebookService.GetHtmlFacebook(urlface, cookie);
            if (html != null)
            {
                var dataUrlProfileFacebook = Regex.Match(html, @"<a href=""/photo.php.*?""")?.Value;
                if (!string.IsNullOrWhiteSpace(dataUrlProfileFacebook))
                {
                    var url = Regex.Match(dataUrlProfileFacebook, @"<a href=""(.*?)""")?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        var htmlProfile = await _facebookService.GetHtmlFacebook(HttpUtility.HtmlDecode("https://d.facebook.com" + url), cookie);
                        if (!string.IsNullOrWhiteSpace(htmlProfile))
                        {
                            return htmlProfile;
                        }
                    }
                }
            }

            return null;
        }
        private async void CheckCookieToken(object sender, string token, string cookie, UserModel user)
        {
            if (!await _facebookService.CheckTokenCookie(token, cookie))
            {
                var bw = (sender as BackgroundWorker);
                if (bw != null)
                {
                    var his = new HistoryModel
                    {
                        Id = user.ID,
                        Note_Auto = "Token hoac cookie die"
                    };
                    bw.ReportProgress(0, new HistoryModel());
                    bw.CancelAsync();
                }
            }
        }

        /// <summary>
        /// tha tim bai viet cua ban be
        /// </summary>
        /// <param name="htmlProfile"></param>
        /// <param name="urlface"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task<bool> AutoThaTim(string htmlProfile, string urlface, string cookie)
        {
            if (!string.IsNullOrWhiteSpace(htmlProfile))
            {
                var datareactions = Regex.Match(htmlProfile, @"<a href=""/reactions/picker/\?is_permalink=1.*?</a>")?.Value;
                if (!string.IsNullOrWhiteSpace(datareactions))
                {
                    var reac = Regex.Match(datareactions, @"<a href=""(.*?)""")?.Groups[1]?.Value + "\"";
                    var para = new List<RequestParameter>()
            {
                new RequestParameter("is_permalink", Regex.Match(reac, @"/?is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ft_id", Regex.Match(reac, @"&amp;ft_id=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("origin_uri", Regex.Match(reac, @"&amp;origin_uri=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(reac, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("refid", Regex.Match(reac, @"&amp;refid=(.*?)""")?.Groups[1]?.Value),
            };
                    var htmlReaction = await _facebookService.GetHtmlFacebook($"{urlface}/reactions/picker", cookie, para);

                    Regex regex = new Regex(@"<a href=""/ufi/reaction/.*?</a>",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlReaction);
                    var ran = new Random();
                    var like = matchCollection[1]?.Value;
                    var paraLike = new List<RequestParameter>()
            {
                new RequestParameter("ft_ent_identifier", Regex.Match(like, @"/?ft_ent_identifier=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("reaction_type", Regex.Match(like, @"&amp;reaction_type=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("is_permalink", Regex.Match(like, @"&amp;is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("basic_origin_uri", Regex.Match(like, @"amp;basic_origin_uri=(.*?)&")?.Groups[1]?.Value),
                // new RequestParameter("_ft_",Regex.Match(like,@"&amp;_ft_(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(like, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ext", Regex.Match(like, @"&amp;ext=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("hash", Regex.Match(like, @"&amp;hash=(.*?)""")?.Groups[1]?.Value),
            };
                    var uriLike = $"{urlface}{Regex.Match(like, @"<a href=""(.*?)""")?.Groups[1]?.Value}";
                    var actionLike = await _facebookService.GetHtmlFacebook(HttpUtility.HtmlDecode(uriLike), cookie);
                    if (actionLike != null && actionLike.Contains("ref_component=mbasic_home_logo"))
                        return true;
                }
            }

            return false;
        }
        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                    Directory.CreateDirectory(_path + "/Id_Facebook");
                    Directory.CreateDirectory(_path + "/Log");
                }
                else
                {
                    GetAccount();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Khởi tạo dữ liệu lỗi: " + e.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void GetAccount()
        {
            if (File.Exists(_fileNameAccount))
            {
                if (DataUsers == null)
                    DataUsers = new ObservableCollection<UserModel>();
                DataUsers =
                    JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(
                        File.ReadAllText(_fileNameAccount));
            }
        }
        private Task SearchAccountUser()
        {
            try
            {
                if (Loading == Visibility.Visible) return Task.FromResult(0);
                Loading = Visibility.Visible;
                if (SearchAccount != null)
                {
                    DataUsers = DataUsers.Where(x => x.ID == SearchAccount) as ObservableCollection<UserModel>;
                }
            }
            catch (Exception e)
            {
                MessageBoxNoti("Lỗi phát sinh: " + e.Message);
            }
            finally
            {
                Loading = Visibility.Hidden;
            }
            return Task.FromResult(0);
        }

        private static void MessageBoxNoti(string mssage)
        {
            System.Windows.MessageBox.Show(mssage, "Thông báo", MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
        }

        private void DeleteAccount()
        {
            try
            {
                if (Loading == Visibility.Visible) return;
                Loading = Visibility.Visible;
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    if (DataUsers != null && DataUsers.Any())
                    {
                        var usr = DataUsers.SingleOrDefault(x => x.ID == ID);
                        if (usr != null)
                            DataUsers.Remove(usr);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxNoti("Lỗi phát sinh: " + e.Message);
            }
            finally
            {
                ID = "";
                Cookie = "";
                Token = "";
                ThoiHan = 0;
                NgayDangKy = "";
                TuongTacAnhDaiDien = false;
                ThaTim = false;
                Loading = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Tạo số thứ tự tự động từ list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetIdMax(ObservableCollection<UserModel> data)
        {
            if (data == null || !data.Any())
            {
                return 1;
            }
            else
            {
                return data.Max(x => x.STT) + 1;
            }
        }
        private async void SaveAccount()
        {
            try
            {
                if (Loading == Visibility.Visible)
                    return;
                Loading = Visibility.Visible;
                if (ID != null && NgayDangKy != null && ThoiHan > 0 && Cookie != null && Token != null)
                {
                    if (ThaTim || TuongTacAnhDaiDien)
                    {
                        var user = new UserModel
                        {
                            STT = GetIdMax(_dataUsers),
                            ID = ID,
                            NgayDangKy = DateTime.Parse(NgayDangKy),
                            ThoiHan = ThoiHan,
                            Cookie = Cookie,
                            Token = Token,
                            TuongTacAnhDaiDien = TuongTacAnhDaiDien,
                            ThaTim = ThaTim,
                            TrangThai = 0,
                        };
                        if (DataUsers == null)
                            DataUsers = new ObservableCollection<UserModel>();
                        if (DataUsers.Any())
                        {
                            var usr = _dataUsers.FirstOrDefault(x => x.ID == ID);
                            if (usr != null && usr?.ID != null)
                            {
                                usr.NgayDangKy = DateTime.Parse(NgayDangKy);
                                usr.ThoiHan = ThoiHan;
                                usr.Cookie = Cookie;
                                usr.Token = Token;
                                usr.TuongTacAnhDaiDien = TuongTacAnhDaiDien;
                                usr.ThaTim = ThaTim;
                                usr.TrangThai = 0;
                            }
                            else
                            {
                                DataUsers.Add(user);
                            }
                        }
                        else
                        {
                            DataUsers.Add(user);
                        }

                        await SaveUidFriendFacebook(Token, Cookie, ID);
                        WriteAllFile(DataUsers, _fileNameAccount);
                        MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButton.OK,
                            MessageBoxImage.Asterisk);
                        ID = "";
                        Cookie = "";
                        Token = "";
                        ThoiHan = 0;
                        NgayDangKy = "";
                        TuongTacAnhDaiDien = false;
                        ThaTim = false;
                    }
                    else
                    {
                        MessageBox.Show("Nhập dữ liệu chưa đủ", "Thông báo", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Nhập dữ liệu chưa đủ", "Thông báo", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi phát sinh: " + e.Message, "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            finally
            {
                Loading = Visibility.Hidden;
            }
        }
        /// <summary>
        /// lưu danh sách user ra file json
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cookie"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task SaveUidFriendFacebook(string token, string cookie, string id)
        {
            var friends = await _facebookService.GetIdFriends(token);
            if (friends != null)
            {
                File.WriteAllText(_pathIdFacebook + "/ID_" + id + ".json", JsonConvert.SerializeObject(friends));
            }
        }

        /// <summary>
        /// ghi file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        private void WriteAllFile(object obj, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }
    }
}