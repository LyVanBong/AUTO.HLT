using System;
using Prism.Mvvm;

namespace AUTO.ALL.IN.APP.Models
{
    public class LoggerModel : BindableBase
    {
        private long _no;
        private string _uid;
        private string _uidFriend;
        private int _typeLogger;
        private string _loggerContent;
        private string _note;

        public long No
        {
            get => _no;
            set => SetProperty(ref _no, value);
        }

        public string Uid
        {
            get => _uid;
            set => SetProperty(ref _uid, value);
        }

        public string UidFriend
        {
            get => _uidFriend;
            set => SetProperty(ref _uidFriend, value);
        }
        /// <summary>
        /// 0. post
        /// 1. avatar
        /// 2. nhan tin
        /// 3. xem story
        /// 4. ket ban theo goi y
        /// 5. cookie ore token die
        /// 6. tam dung
        /// </summary>
        public int TypeLogger
        {
            get => _typeLogger;
            set => SetProperty(ref _typeLogger, value);
        }

        public string LoggerContent
        {
            get => _loggerContent;
            set => SetProperty(ref _loggerContent, value);
        }

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public DateTime DateTime => DateTime.Now;
        public LoggerModel(long no, string uid, string uidFriend, int typeLogger, string loggerContent, string note)
        {
            _no = no;
            _uid = uid;
            _uidFriend = uidFriend;
            _typeLogger = typeLogger;
            _loggerContent = loggerContent;
            _note = note;
        }
    }
}