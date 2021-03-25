using System.ComponentModel;
using Prism.Mvvm;

namespace AUTO.SEND.MESSAGE.FACEBOOK.TOOL.Models
{
    public class AcountsModel : BindableBase
    {
        private string _id;
        private int _stt;
        private string _userName;
        private string _passwd;
        private string _cookie;
        private string _token;
        private string _message1;
        private string _message2;
        private string _message3;
        private int _timeDelay;
        private string _note;
        private int _status;
        private BackgroundWorker _backgroundWorker;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public int Stt
        {
            get => _stt;
            set => SetProperty(ref _stt, value);
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

        public string Message1
        {
            get => _message1;
            set => SetProperty(ref _message1, value);
        }

        public string Message2
        {
            get => _message2;
            set => SetProperty(ref _message2, value);
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

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public BackgroundWorker BackgroundWorker
        {
            get => _backgroundWorker;
            set => _backgroundWorker = value;
        }

        public AcountsModel(string id, int stt, string userName, string passwd, string cookie, string token, string message1, string message2, string message3, int timeDelay, string note, int status, BackgroundWorker backgroundWorker)
        {
            _id = id;
            _stt = stt;
            _userName = userName;
            _passwd = passwd;
            _cookie = cookie;
            _token = token;
            _message1 = message1;
            _message2 = message2;
            _message3 = message3;
            _timeDelay = timeDelay;
            _note = note;
            _status = status;
            _backgroundWorker = backgroundWorker;
        }
    }
}