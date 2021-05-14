using AUTO.ALL.IN.APP.Models;
using AUTO.ALL.IN.APP.Services;
using AUTO.DLL.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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
        private AccountStatisticsModel _accountStatistics = new AccountStatisticsModel();
        private bool _isRunningTool;
        private string _notification = $"[{DateTime.Now}] Thông báo mới tại đây";

        public bool IsRunningTool
        {
            get => _isRunningTool;
            set => SetProperty(ref _isRunningTool, value);
        }
        public string Notification
        {
            get => _notification;
            set => SetProperty(ref _notification, value);
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

        public async void UpdateDatabase()
        {
            try
            {
                if (_dispatcherTimer.IsEnabled)
                {
                    _dispatcherTimer.Stop();
                    _dispatcherTimer = null;
                }
                if (DataTool != null && DataTool.Any())
                {
                    foreach (var user in DataTool)
                    {
                        if (user.Worker != null)
                        {
                            user.Worker.CancelAsync();
                            user.Worker.Dispose();
                            user.Worker = null;
                        }
                        if (user.Status == 1)
                            user.Status = 0;
                    }
                    // Lưu dữ liệu lên database
                    await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(UpdateDatabase));
            }
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
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\DATA\LOGGER"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\DATA\LOGGER");
                }
                var data =
                    await RealtimeDatabaseService.Get<ObservableCollection<UserFacebookModel>>(
                        nameof(UserFacebookModel));
                if (data != null)
                {
                    DataTool = data;
                    await UpAccountStatistics();
                }

                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
                _dispatcherTimer.Tick += Timer_Tick;
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(StartService));
            }
            finally
            {
                if ((await ResultMessageBox("Bạn có muộn bật tool luôn không")) == MessageBoxResult.OK)
                {
                    await TurnOnOffService();
                }
            }
        }

        public async Task UpAccountStatistics()
        {
            try
            {
                if (DataTool != null && DataTool.Any())
                {
                    AccountStatistics.Total = DataTool.Count;
                    AccountStatistics.New = DataTool.Count(x => x.Status == 0);
                    AccountStatistics.Running = DataTool.Count(x => x.Status == 1);
                    AccountStatistics.Pause = DataTool.Count(x => x.Status == 2);
                    AccountStatistics.Died = DataTool.Count(x => x.Status == 3);
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(UpAccountStatistics));
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine($@"[{DateTime.Now}] Start timer");
#endif
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

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private async Task AddLogger(LoggerModel logger)
        {
            try
            {
                var pathLog = AppDomain.CurrentDomain.BaseDirectory + "/DATA/LOGGER/Log-" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (DataLogger == null)
                {
                    DataLogger = new ObservableCollection<LoggerModel>();
                }
                if (!File.Exists(pathLog))
                {
                    if (DataLogger.Any())
                        DataLogger.Clear();
                }
                logger.No = DataLogger.Count + 1;
                Notification = $"[{logger.No}]-[{logger.DateTime}]-[{logger.TypeLogger}]-[{logger.Uid}]-[{logger.UidFriend}]-[{logger.LoggerContent}]-[{logger.Note}]";
                File.AppendAllLines(pathLog, new[] { Notification });
                App.Current.Dispatcher.Invoke(() => DataLogger.Add(logger));
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(AddLogger));
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var user = e.Argument as UserFacebookModel;
            var worker = sender as BackgroundWorker;
            try
            {
                if (user != null && worker != null)
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
                                    AddLogger(new LoggerModel(user.IdFacebook, myFriend.id, 7, "Tạm dừng tài khoản", "Done")).Await();
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
                                    AddLogger(new LoggerModel(user.IdFacebook, myFriend.id, 7, "Tạm dừng tài khoản", "Done")).Await();
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
                                    AddLogger(new LoggerModel(user.IdFacebook, myFriend.id, 7, "Tạm dừng tài khoản", "Done")).Await();
                                    e.Cancel = true;
                                    return;
                                }
                                // ke ban theo goi y
                                if (user.OPtionFriendsSuggestions.IsSelectFunction)
                                {
                                    FriendsSuggestionsFacebook(user, myFriend, random, worker, e);
                                }
                                if (worker.CancellationPending == true)
                                {
                                    user.Status = 2;
                                    AddLogger(new LoggerModel(user.IdFacebook, myFriend.id, 7, "Tạm dừng tài khoản", "Done")).Await();
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
                                    AddLogger(new LoggerModel(user.IdFacebook, myFriend.id, 7, "Tạm dừng tài khoản", "Done")).Await();
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
                        AddLogger(new LoggerModel(user.IdFacebook, "", 7, "Run tool done", "Done")).Await();
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
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 1, "Tương tác với ảnh đại diện của bạn bè", "Done")).Await();
                        user.OptionAvatar.TotalReaction++;
                    }
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 5, "Token or cookie die", "Error")).Await();
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
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 5, "Token or cookie die", "Error")).Await();
                        ev.Cancel = true;
                        return;
                    }
                }
                else
                {
                    var regex = Regex.Matches(html, @"id="""" data-href=""(.*?)""");
                    if (regex.Count > 0)
                    {
                        var url = regex[random.Next(6)]?.Groups[1]?.Value;
                        if (!string.IsNullOrEmpty(url))
                        {
                            FacebookService.AutoDropHeartFacebookStory(url, user.Cookie, user.OptionStory.IndexOptionReac).Await();
                            user.OptionStory.TotalReaction++;
                            AddLogger(new LoggerModel(user.IdFacebook, friend.id, 3, "Xem story của bạn bè: " + url, "Done")).Await();
                            Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(SeenStoryFacebook)).Await();
                return;
            }
        }

        private void FriendsSuggestionsFacebook(UserFacebookModel user, Datum friend, Random random, BackgroundWorker worker, DoWorkEventArgs ev)
        {
            try
            {
                var delay = user.OPtionFriendsSuggestions.TimeDelay;
                if (FacebookService.AddFriend("").Result)
                {
                    user.OptionStory.TotalReaction++;
                    AddLogger(new LoggerModel(user.IdFacebook, friend.id, 4, "Gửi lời mời kết bạn", "Done")).Await();
                    Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 5, "Token or cookie die", "Error")).Await();
                        ev.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                ShowMessageError(e, nameof(FriendsSuggestionsFacebook)).Await();
            }
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
                    AddLogger(new LoggerModel(user.IdFacebook, friend.id, 2, "Gửi tin nhắn", "Done")).Await();
                    user.OptionMessage.TotalReaction++;
                    Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 5, "Token or cookie die", "Error")).Await();
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
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 0, user.OptionPost.IndexOptionReac + ",Interactive post", "Done")).Await();
                        user.OptionPost.TotalReaction++;
                        Thread.Sleep(TimeSpan.FromMinutes(random.Next(delay - 2, delay + 2)));
                    }
                }
                else
                {
                    if (!FacebookService.CheckTokenCookie(user.Token, user.Cookie).Result)
                    {
                        user.Status = 3;
                        AddLogger(new LoggerModel(user.IdFacebook, friend.id, 5, "Token or cookie die", "Error")).Await();
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
        }

        #endregion
        #region Home

        public ObservableCollection<UserFacebookModel> DataTool
        {
            get => _dataTool;
            set => SetProperty(ref _dataTool, value);
        }

        public ICommand StartToolCommand { get; private set; }
        public ICommand UpdateUserCommand { get; private set; }
        public ICommand PauseUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand ContinueUserCommand { get; private set; }
        private void HomeTool()
        {
            UpdateUserCommand = new DelegateCommand<UserFacebookModel>(async (user) => await UpdateUser(user));
            StartToolCommand = new DelegateCommand(async () => await TurnOnOffService());
            PauseUserCommand = new DelegateCommand<UserFacebookModel>(async (user) => await PauseUser(user));
            DeleteUserCommand = new DelegateCommand<UserFacebookModel>(async (user) => await DeleteUser(user));
            ContinueUserCommand = new DelegateCommand<UserFacebookModel>(async (user) => await ContinueUser(user));
        }

        private async Task ContinueUser(UserFacebookModel user)
        {
            try
            {
                user.Status = 0;
                await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                await UpAccountStatistics();
                Notification = $"[{DateTime.Now}] Chạy lại tài khoản: " + user.UserNameApp;
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(ContinueUser));
            }
        }

        private async Task TurnOnOffService()
        {
            await Task.Run(() =>
            {
                if (IsRunningTool)
                {
                    Notification = $"[{DateTime.Now}] Tạm dừng chạy công cụ";
                    _dispatcherTimer.Stop();
                    IsRunningTool = false;
                }
                else
                {
                    Timer_Tick(null, null);
                    Notification = $"[{DateTime.Now}] Bật công cụ";
                    _dispatcherTimer.Start();
                    IsRunningTool = true;
                }
            });
        }

        private async Task DeleteUser(UserFacebookModel user)
        {
            try
            {
                if ((await ResultMessageBox($"Bạn muỗn xóa tài khoản {user.UserNameApp}")) == MessageBoxResult.OK)
                {
                    if (user.Worker != null)
                    {
                        user.Worker.CancelAsync();
                        user.Worker = null;
                    }
                    DataTool.Remove(user);
                    await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                    await UpAccountStatistics();
                    Notification = $"[{DateTime.Now}] Xóa tài khoản";
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(DeleteUser));
            }
        }

        private Task<MessageBoxResult> ResultMessageBox(string message)
        {
            return Task.FromResult(
                MessageBox.Show(message, "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Question));
        }

        private async Task PauseUser(UserFacebookModel user)
        {
            try
            {
                user.Status = 2;
                if (user.Worker != null)
                {
                    user.Worker.CancelAsync();
                    user.Worker = null;
                }
                await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                await UpAccountStatistics();
                Notification = $"[{DateTime.Now}] Tạm dừng tài khoản";
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(PauseUser));
            }
        }

        private async Task UpdateUser(UserFacebookModel user)
        {
            try
            {
                SelectedIndex = 1;
                UserFacebookModel = user;
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(UpdateUser));
            }
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
                        if (string.IsNullOrEmpty(UserFacebookModel.Id) ||
                            string.IsNullOrEmpty(UserFacebookModel.Cookie) ||
                            string.IsNullOrEmpty(UserFacebookModel.Token))
                        {
                            await ShowMessage(@"Vui lòng điền đầy đủ dữ liệu trước khi lưu !").ConfigureAwait(false);
                        }
                        else
                        {
                            if (UserFacebookModel.OptionPost.IsSelectFunction ||
                                UserFacebookModel.OPtionFriendsSuggestions.IsSelectFunction ||
                                UserFacebookModel.OptionAvatar.IsSelectFunction ||
                                UserFacebookModel.OptionStory.IsSelectFunction ||
                                UserFacebookModel.OptionMessage.IsSelectFunction)
                            {
                                var random = new Random();

                                var fields = "friends.limit(" + random.Next(4000, 5000) +
                                             "){id,name},id,name,picture,email";
                                var json = await FacebookService.GetApiFacebook(UserFacebookModel.Token, fields);
                                if (string.IsNullOrEmpty(json))
                                {
                                    if (await FacebookService.CheckTokenCookie(UserFacebookModel.Token,
                                        UserFacebookModel.Cookie))
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
                                            if (MessageBox.Show(
                                                    "Tài khoản này đã tồn tại bạn có muỗn cập nhật lại không !",
                                                    "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                                                MessageBoxResult.Yes)
                                            {
                                                var removeUser = DataTool.FirstOrDefault(x =>
                                                    x.IdFacebook == UserFacebookModel.IdFacebook);
                                                if (removeUser == null) return;
                                                DataTool.Remove(removeUser);
                                                DataTool.Add(UserFacebookModel);
                                                Notification = $"[{DateTime.Now}] Cập nhật dữ liệu thành công";
                                                await ShowMessage(@"Cập nhật dữ liệu thành công !")
                                                    .ConfigureAwait(false);
                                            }
                                        }
                                        else
                                        {
                                            DataTool.Add(UserFacebookModel);
                                            Notification = $"[{DateTime.Now}] Lưu dữ liệu thành công";
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
                                await ShowMessage(@"Bạn chưa chọn chức năng nào để chạy công cụ !")
                                    .ConfigureAwait(false);
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
            finally
            {
                await UpAccountStatistics();
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

                    var data = JsonConvert.DeserializeObject<JsonInfoModel>(DataJson);
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
