using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.Main;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

        public ObservableCollection<HistoryModel> DataHistory { get; set; }
        public MainWindowViewModel(IFacebookService facebookService)
        {
            _facebookService = facebookService;
            Loading = Visibility.Visible;
            SaveAccountCommand = new DelegateCommand(SaveAccount);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
            SearchAccountCommand = new DelegateCommand(async () => await SearchAccountUser());
            DataUsers = new ObservableCollection<UserModel>();
            CreateDirectory();
            Loading = Visibility.Hidden;
            StartServiceAuto();
        }

        private void StartServiceAuto()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine(@"Timer đang chạy cẩn thân nhẽ");
#endif
            try
            {
                var json = File.ReadAllText(_fileNameAccount);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var data = JsonConvert.DeserializeObject<List<UserModel>>(json);
                    if (data != null && data.Any())
                    {
                        var tmp = 0;
                        foreach (var user in data)
                        {
                            if (user.TrangThai == 0)
                            {
                                var worker = new BackgroundWorker();
                                worker.WorkerReportsProgress = true;
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

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var user = (UserModel)e.Argument;
                if (user != null)
                {
                    var json = File.ReadAllText(_pathIdFacebook + "/ID_" + user.ID + ".json");
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var data = JsonConvert.DeserializeObject<FriendsModel>(json);
                        if (data != null && data.data.Any())
                        {
                            var random = new Random();
                            var uid = random.Next(0, 1) == 1 ? data.data.OrderBy(x => x.name) : data.data.OrderByDescending(x => x.name);
                            foreach (var id in uid)
                            {
                                var idPost = await _facebookService.GetIdPostFriends("2", user.Token, id.id);
                                if (idPost != null && idPost.id != null)
                                {

                                }
                                else
                                {
                                    if (!await _facebookService.CheckTokenCookie(user.Token, user.Cookie))
                                    {
                                        (sender as BackgroundWorker)?.CancelAsync();
                                    }
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