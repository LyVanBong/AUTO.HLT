using System;
using Prism.Mvvm;

namespace AUTO.HLT.ADMIN.Models.Main
{
    public class UserModel : BindableBase
    {
        private int _stt;
        private string _id;
        private DateTime _ngayDangKy;
        private int _thoiHan;
        private string _cookie;
        private string _token;
        private bool _thaTim;
        private bool _tuongTacAnhDaiDien;
        private int _trangThai;
        private string _note;
        private string _comment;

        public int STT
        {
            get => _stt;
            set => SetProperty(ref _stt, value);
        }

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime NgayDangKy
        {
            get => _ngayDangKy;
            set
            {
                if (SetProperty(ref _ngayDangKy, value))
                {
                    RaisePropertyChanged(nameof(NgayHetHan));
                }
            }
        }

        public int ThoiHan
        {
            get => _thoiHan;
            set
            {
                if (SetProperty(ref _thoiHan, value))
                {
                    RaisePropertyChanged(nameof(NgayHetHan));
                }
            }
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

        /// <summary>
        /// 0. tắt
        /// 1. đang chạy
        /// 2. die data
        /// </summary>
        public int TrangThai
        {
            get => _trangThai;
            set => SetProperty(ref _trangThai, value);
        }

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public DateTime NgayHetHan => NgayDangKy.AddDays(ThoiHan).Date;

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }
    }
}
