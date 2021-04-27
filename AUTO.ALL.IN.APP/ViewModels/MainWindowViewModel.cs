using AUTO.ALL.IN.APP.Models;
using AUTO.ALL.IN.APP.Services;
using AUTO.DLL.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AUTO.ALL.IN.APP.Views;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AUTO.ALL.IN.APP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private string _dataJson;
        private UserFacebookModel _userFacebookModel = new UserFacebookModel();
        private ObservableCollection<UserFacebookModel> _dataTool = new ObservableCollection<UserFacebookModel>();
        private DispatcherTimer _dispatcherTimer;
        private int _selectedIndex;
        private ObservableCollection<LoggerModel> _dataLogger = new ObservableCollection<LoggerModel>();
        private Visibility _loading = Visibility.Hidden;
        private AccountStatisticsModel _accountStatistics;
        private bool _isRunningTool;

        public bool IsRunningTool
        {
            get => _isRunningTool;
            set => SetProperty(ref _isRunningTool, value);
        }

        public AccountStatisticsModel AccountStatistics
        {
            get => _accountStatistics;
            set => SetProperty(ref _accountStatistics, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Visibility Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public MainWindowViewModel()
        {
            AddAccount();
            HomeTool();
            StartService().Await();
        }

        #region Logger

        public ObservableCollection<LoggerModel> DataLogger
        {
            get => _dataLogger;
            set => SetProperty(ref _dataLogger, value);
        }

        #endregion
        #region Service

        private async Task StartService()
        {
            try
            {
                var data = await RealtimeDatabaseService.Get<ObservableCollection<UserFacebookModel>>(nameof(UserFacebookModel));
                AccountStatistics = new AccountStatisticsModel(data);
                if (data != null)
                {
                    DataTool = data;
                }
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
                _dispatcherTimer.Tick += Timer_Tick;
                // _dispatcherTimer.Start();

            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(StartService));
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DataTool != null && DataTool.Any())
                {
                    foreach (var user in DataTool)
                    {
                        if (user.Status == 0 && user.EndDate.Date >= DateTime.Now.Date)
                        {
                            user.Worker = new BackgroundWorker();
                            user.Worker.WorkerReportsProgress = true;
                            user.Worker.WorkerSupportsCancellation = true;
                            user.Worker.DoWork += Worker_DoWork;
                            user.Worker.ProgressChanged += Worker_ProgressChanged;
                            user.Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                            user.Worker.RunWorkerAsync(user);
                            user.Status = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex, nameof(Timer_Tick)).Await();
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                var logger = e.Result as LoggerModel;
                if (logger != null)
                {
                    if (DataLogger == null || !DataLogger.Any())
                    {
                        DataLogger = new ObservableCollection<LoggerModel>();
                        logger.No = 1;
                    }
                    else
                    {
                        logger.No = DataLogger.Count + 1;
                    }
                    DataLogger.Add(logger);
                }

            }
            catch (Exception ex)
            {
                ShowMessageError(ex, nameof(Worker_RunWorkerCompleted)).Await();
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var logger = e.UserState as LoggerModel;
            try
            {
                if (logger != null)
                {
                    if (DataLogger == null)
                    {
                        DataLogger = new ObservableCollection<LoggerModel>();
                        logger.No = 1;
                    }
                    else
                    {
                        logger.No = DataLogger.Count + 1;
                    }
                    DataLogger.Add(logger);
                }

            }
            catch (Exception ex)
            {
                ShowMessageError(ex, nameof(Worker_ProgressChanged)).Await();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var user = e.Argument as UserFacebookModel;
            var worker = sender as BackgroundWorker;
            try
            {
                if (user != null)
                {
                    var friend = user?.DataFacebook?.friends?.data;
                    if (friend != null && friend.Any())
                    {
                        var random = new Random();
                        foreach (var myFriend in friend)
                        {
                            if (!myFriend.IsDone)
                            {
                                myFriend.IsDone = true;
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    e.Result = new LoggerModel(0, user.IdFacebook, myFriend.id, 7, "Cancel tool", "Done");
                                    e.Cancel = true;
                                    return;
                                }
                                // tha tim avatar
                                if (user.OptionAvatar.IsSelectFunction)
                                {
                                    ReacAvatarFacebook(user, myFriend, random, worker, e);
                                }
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    e.Result = new LoggerModel(0, user.IdFacebook, myFriend.id, 7, "Cancel tool", "Done");
                                    e.Cancel = true;
                                    return;
                                }
                                // tha tim story
                                if (user.OptionStory.IsSelectFunction)
                                {
                                    SeenStoryFacebook(user, myFriend, random, worker, e);
                                }
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    e.Result = new LoggerModel(0, user.IdFacebook, myFriend.id, 7, "Cancel tool", "Done");
                                    e.Cancel = true;
                                    return;
                                }
                                // ke ban theo goi y
                                if (user.OPtionFriendsSuggestions.IsSelectFunction)
                                {
                                    //FriendsSuggestionsFacebook();
                                }
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    e.Result = new LoggerModel(0, user.IdFacebook, myFriend.id, 7, "Cancel tool", "Done");
                                    e.Cancel = true;
                                    return;
                                }
                                // gui tin nhan
                                if (user.OptionMessage.IsSelectFunction)
                                {
                                    SendMessageFacebook(user, myFriend, random, worker, e);
                                }
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    e.Result = new LoggerModel(0, user.IdFacebook, myFriend.id, 7, "Cancel tool", "Done");
                                    e.Cancel = true;
                                    return;
                                }
                                // bai viet cua ban be
                                if (user.OptionPost.IsSelectFunction)
                                {
                                    ReacPostFacebook(user, myFriend, random, worker, e);
                                }
                            }
                        }
                        e.Result = new LoggerModel(0, user.IdFacebook, "0000000000", 7, "Run tool done", "Done");
                        user.Status = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageError(ex, nameof(Worker_DoWork)).Await();
            }
        }
        /// <summary>
        /// tương tác với ảnh đại diện của bạn bè
        /// </summary>
        /// <param name="user"></param>
        /// <param name="friend"></param>
        /// <param name="random"></param>
        /// <param name="worker"></param>
        /// <param name="ev"></param>
        private void ReacAvatarFacebook(UserFacebookModel user, Datum friend, Random random, BackgroundWorker worker, DoWorkEventArgs ev)
        {
            var delay = user.OptionAvatar.TimeDelay;
            try
            {
                var idPost = FacebookService.GetIdPostFacebook(friend.id, user.Cookie, 0).Result;
                if (!string.IsNullOrWhiteSpace(idPost))
                {
                    var reaction =
                        FacebookService.ReactionPost(idPost, user.Cookie, user.OptionAvatar.IndexOptionReac, user.OptionAvatar.Comment).Result;
                    if (reaction > 0)
                    {
                        var his = new LoggerModel(1, user.IdFacebook, friend.id, 1, user.OptionAvatar.IndexOptionReac + ",Interactive avatar", "Done");
                        worker.ReportProgress(1, his);
                    }
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        var his = new LoggerModel(0, user.IdFacebook, friend.id, 5, "Token or cookie die", "Error");
                        ev.Result = his;
                        ev.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(ReacAvatarFacebook)).Await();
                return;
            }

            Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));

        }

        private void SeenStoryFacebook(UserFacebookModel user, Datum friend, Random random, BackgroundWorker worker, DoWorkEventArgs ev)
        {
            var delay = user.OptionStory.TimeDelay;
            try
            {
                var html = FacebookService.GetHtmlChrome("https://m.facebook.com/", user.Cookie).Result;
                if (string.IsNullOrEmpty(html))
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        var his = new LoggerModel(0, user.IdFacebook, friend.id, 5, "Token or cookie die", "Error");
                        ev.Result = his;
                        ev.Cancel = true;
                        return;
                    }
                }
                else
                {
                    var regex = Regex.Matches(html, @"id="""" data-href=""(.*?)""");
                    foreach (Match o in regex)
                    {
                        var url = o?.Groups[1]?.Value;
                        FacebookService.AutoDropHeartFacebookStory(url, user.Cookie, user.OptionStory.IndexOptionReac).Await();
                        var his = new LoggerModel(1, user.IdFacebook, friend.id, 3, user.OptionMessage.IndexOptionReac + ",Interactive story", "Done");
                        worker.ReportProgress(1, his);
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(SeenStoryFacebook)).Await();
                return;
            }
            Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
        }

        private void FriendsSuggestionsFacebook()
        {

        }

        private void SendMessageFacebook(UserFacebookModel user, Datum friend, Random random, BackgroundWorker worker, DoWorkEventArgs ev)
        {
            var delay = user.OptionMessage.TimeDelay;
            try
            {
                var messager = user.OptionMessage.Messager;
                if (string.IsNullOrEmpty(messager)) return;
                var mess = messager.Split('\n');


                var send = FacebookService.SendMessage(string.Format(mess[random.Next(0, mess.Length - 1)], friend.name), friend.id,
                    user.Cookie).Result;

                if (send)
                {

                    var his = new LoggerModel(1, user.IdFacebook, friend.id, 2, user.OptionMessage.IndexOptionReac + ",Interactive send messager", "Done");
                    worker.ReportProgress(1, his);
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        var his = new LoggerModel(0, user.IdFacebook, friend.id, 5, "Token or cookie die", "Error");
                        ev.Result = his;
                        ev.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(SendMessageFacebook)).Await();
                return;
            }
            Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
        }

        private void ReacPostFacebook(UserFacebookModel user, Datum friend, Random random, BackgroundWorker worker, DoWorkEventArgs ev)
        {
            var delay = user.OptionPost.TimeDelay;
            try
            {
                var idPost = FacebookService.GetIdPostFacebook(friend.id, user.Cookie, 1).Result;
                if (!string.IsNullOrWhiteSpace(idPost))
                {
                    var reaction =
                        FacebookService.ReactionPost(idPost, user.Cookie, user.OptionPost.IndexOptionReac, user.OptionPost.Comment).Result;
                    if (reaction > 0)
                    {
                        var his = new LoggerModel(1, user.IdFacebook, friend.id, 0, user.OptionPost.IndexOptionReac + ",Interactive post", "Done");
                        worker.ReportProgress(1, his);
                    }
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        var his = new LoggerModel(0, user.IdFacebook, friend.id, 5, "Token or cookie die", "Error");
                        ev.Result = his;
                        ev.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(ReacPostFacebook)).Await();
                return;
            }
            Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
        }

        #endregion
        #region Home

        public ObservableCollection<UserFacebookModel> DataTool
        {
            get => _dataTool;
            set => SetProperty(ref _dataTool, value);
        }

        public ICommand StartToolCommand { get; private set; }

        private void HomeTool()
        {
            StartToolCommand = new DelegateCommand(() =>
            {
                if (IsRunningTool)
                {
                    _dispatcherTimer.Stop();
                    IsRunningTool = false;
                }
                else
                {
                    _dispatcherTimer.Start();
                    IsRunningTool = true;
                }

            });
        }

        #endregion
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
        public ICommand SelectOptionStoryCommand { get; private set; }
        public ICommand SelectOptionAvatarCommand { get; private set; }
        public ICommand SelectOptionPostCommand { get; private set; }

        /// <summary>
        /// constructor của chức năng thêm tài khoản
        /// </summary>
        private void AddAccount()
        {
            GetInfoFacebookCommand = new DelegateCommand(async () => await ConvertJsonToInfo());
            LoginFacebookCommand = new DelegateCommand(async () => await LoginFacebook());
            SaveAccountCommand = new DelegateCommand<string>(async (key) => await SaveAccount(key));
            SelectOptionStoryCommand = new DelegateCommand<string>(async (story) => await SelectOptionStory(story));
            SelectOptionAvatarCommand = new DelegateCommand<string>(async (avatar) => await SelectOptionAvatar(avatar));
            SelectOptionPostCommand = new DelegateCommand<string>(async (post) => await SelectOptionPost(post));
        }

        private async Task SelectOptionPost(string post)
        {
            try
            {
                UserFacebookModel.OptionPost.IndexOptionReac = int.Parse(post);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionPost));
            }
        }

        private async Task SelectOptionAvatar(string avatar)
        {
            try
            {
                UserFacebookModel.OptionAvatar.IndexOptionReac = int.Parse(avatar);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionAvatar));
            }
        }

        private async Task SelectOptionStory(string story)
        {
            try
            {
                UserFacebookModel.OptionStory.IndexOptionReac = int.Parse(story);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionStory));
            }
        }

        private async Task SaveAccount(string key)
        {
            try
            {
                if (key == "0")
                {
                    if (UserFacebookModel != null)
                    {
                        if (string.IsNullOrEmpty(UserFacebookModel.Id) || string.IsNullOrEmpty(UserFacebookModel.Cookie) || string.IsNullOrEmpty(UserFacebookModel.Token))
                        {
                            await ShowMessage(@"Vui lòng điền đầy đủ dữ liệu trước khi lưu !").ConfigureAwait(false);
                        }
                        else
                        {
                            if (UserFacebookModel.OptionPost.IsSelectFunction || UserFacebookModel.OPtionFriendsSuggestions.IsSelectFunction || UserFacebookModel.OptionAvatar.IsSelectFunction || UserFacebookModel.OptionStory.IsSelectFunction || UserFacebookModel.OptionMessage.IsSelectFunction)
                            {
                                var random = new Random();

                                var fields = "friends.limit(" + random.Next(4000, 5000) + "){id,name},id,name,picture,email";
                                var json = await FacebookService.GetApiFacebook(UserFacebookModel.Token, fields);
                                if (string.IsNullOrEmpty(json))
                                {
                                    if (await FacebookService.CheckTokenCookie(UserFacebookModel.Token, UserFacebookModel.Cookie))
                                    {
                                        await ShowMessage(@"Lỗi phát sinh vui lòng thử lại !").ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        await ShowMessage(@"Cookie hoặc token die !").ConfigureAwait(false);
                                    }
                                }
                                else
                                {
                                    var facebook = JsonConvert.DeserializeObject<DataFacebookModel>(json);
                                    if (facebook == null)
                                    {
                                        await ShowMessage(@"Lỗi phát sinh vui lòng thử lại !").ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        UserFacebookModel.DataFacebook = facebook;
                                        if (DataTool.Any(x => x.IdFacebook == UserFacebookModel.IdFacebook))
                                        {
                                            if (MessageBox.Show("Tài khoản này đã tồn tại bạn có muỗn cập nhật lại không !", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                            {
                                                var update = DataTool.FirstOrDefault(x =>
                                                    x.IdFacebook == UserFacebookModel.IdFacebook);
                                                if (update == null) return;
                                                update.DataFacebook = facebook;
                                                DataTool.Add(update);
                                                await ShowMessage(@"Cập nhật dữ liệu thành công !").ConfigureAwait(false);
                                            }
                                        }
                                        else
                                        {
                                            DataTool.Add(UserFacebookModel);
                                            await ShowMessage(@"Lưu dữ liệu thành công !").ConfigureAwait(false);
                                        }

                                        if (DataTool.Any())
                                        {
                                            await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                                        }
                                        await ResetInput();
                                    }
                                }
                            }
                            else
                            {
                                await ShowMessage(@"Bạn chưa chọn chức năng nào để chạy công cụ !").ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        await ShowMessage(@"Dữ liệu chưa được khởi tạo").ConfigureAwait(false);
                    }
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
                        var strDate = data.Ghi_Chu.Ngay_Het_Han.Split('/');
                        UserFacebookModel.EndDate = new DateTime(int.Parse(strDate[2]), int.Parse(strDate[1]),
                            int.Parse(strDate[0]));

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
