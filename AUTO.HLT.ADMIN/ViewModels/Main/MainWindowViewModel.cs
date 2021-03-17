using AUTO.HLT.ADMIN.Models.Main;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private string _path = AppDomain.CurrentDomain.BaseDirectory + "/DataAuto";
        private string _fileNameAccount = "/AccountFacebook.json";
        private Visibility _loading;

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

        public MainWindowViewModel()
        {
            Loading = Visibility.Visible;
            SaveAccountCommand = new DelegateCommand(SaveAccount);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
            SearchAccountCommand = new DelegateCommand(async () => await SearchAccountUser());
            DataUsers = new ObservableCollection<UserModel>();
            CreateDirectory();
            Loading = Visibility.Hidden;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
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
            var pathFileAccount = _path + _fileNameAccount;
            if (File.Exists(pathFileAccount))
            {
                DataUsers =
                    JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(
                        File.ReadAllText(pathFileAccount));
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
        private void SaveAccount()
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

                        WriteAllFile(DataUsers, _path + _fileNameAccount);
                        MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButton.OK,
                            MessageBoxImage.Asterisk);
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