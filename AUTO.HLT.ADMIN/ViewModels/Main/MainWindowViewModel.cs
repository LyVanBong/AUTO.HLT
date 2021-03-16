using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Models.Main;
using Prism.Commands;
using Prism.Mvvm;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private List<UserModel> _dataUsers;
        private int _stt = 0;
        private UserModel _user;
        private string _id;
        private DateTime _ngayDangKy;
        private int _thoiHan;
        private string _cookie;
        private string _token;
        private bool _thaTim;
        private bool _tuongTacAnhDaiDien;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public List<UserModel> DataUsers
        {
            get => _dataUsers;
            set => SetProperty(ref _dataUsers, value);
        }

        public ICommand SaveAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }

        public UserModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime NgayDangKy
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

        public MainWindowViewModel()
        {
            SaveAccountCommand = new DelegateCommand(SaveAccount);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
        }

        private void DeleteAccount()
        {
            MessageBox.Show("test 1");
        }

        private void SaveAccount()
        {
            var data =new  UserModel()
            {
                ID = ID,
                NgayDangKy = NgayDangKy,
                Cookie = Cookie,
            };
        }
    }
}