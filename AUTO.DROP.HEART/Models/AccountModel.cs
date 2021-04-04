using System;
using Prism.Mvvm;

namespace AUTO.DROP.HEART.Models
{
    public class AccountModel : BindableBase
    {
        private string _id;
        private int _stt;
        private string _userName;
        private string _passwd;
        private string _cookie;
        private string _token;
        private int _status;
        private string _note;

        public int Stt
        {
            get => _stt;
            set => SetProperty(ref _stt, value);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
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

        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public DateTime DateCreate => DateTime.Now;
    }
}