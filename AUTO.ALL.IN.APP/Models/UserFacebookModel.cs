using Prism.Mvvm;
using System;

namespace AUTO.ALL.IN.APP.Models
{
    public class UserFacebookModel : BindableBase
    {
        private string _id = Guid.NewGuid() + "";
        private string _cookie;
        private string _token;
        private string _userNameFacebook;
        private string _passFacebook;
        private string _avatarFacebook;
        private string _nameFacebook;
        private string _idFacebook;
        private string _numberPhoneFacebook;
        private string _numberPhoneApp;
        private string _nameApp;
        private string _userNameApp;
        private DateTime _endDate;
        private OptionReacModel _optionPost = new OptionReacModel();
        private OptionReacModel _optionAvatar = new OptionReacModel();
        private OptionReacModel _optionStory = new OptionReacModel();
        private OptionReacModel _optionMessage = new OptionReacModel();
        private OptionReacModel _oPtionFriendsSuggestions = new OptionReacModel();

        /// <summary>
        /// Id nguoi dung
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Cookie facebook
        /// </summary>
        public string Cookie
        {
            get => _cookie;
            set => SetProperty(ref _cookie, value);
        }

        /// <summary>
        /// Token facebook
        /// </summary>
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        /// <summary>
        /// UserName facebook
        /// </summary>
        public string UserNameFacebook
        {
            get => _userNameFacebook;
            set => SetProperty(ref _userNameFacebook, value);
        }

        /// <summary>
        /// Passwd facebook
        /// </summary>
        public string PassFacebook
        {
            get => _passFacebook;
            set => SetProperty(ref _passFacebook, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public string AvatarFacebook
        {
            get => _avatarFacebook;
            set => SetProperty(ref _avatarFacebook, value);
        }

        public string NameFacebook
        {
            get => _nameFacebook;
            set => SetProperty(ref _nameFacebook, value);
        }

        public string IdFacebook
        {
            get => _idFacebook;
            set => SetProperty(ref _idFacebook, value);
        }

        public string NumberPhoneFacebook
        {
            get => _numberPhoneFacebook;
            set => SetProperty(ref _numberPhoneFacebook, value);
        }

        public string NumberPhoneApp
        {
            get => _numberPhoneApp;
            set => SetProperty(ref _numberPhoneApp, value);
        }

        public string NameApp
        {
            get => _nameApp;
            set => SetProperty(ref _nameApp, value);
        }

        public string UserNameApp
        {
            get => _userNameApp;
            set => SetProperty(ref _userNameApp, value);
        }

        public OptionReacModel OptionPost
        {
            get => _optionPost;
            set => SetProperty(ref _optionPost, value);
        }

        //Interaction Avatar
        public OptionReacModel OptionAvatar
        {
            get => _optionAvatar;
            set => SetProperty(ref _optionAvatar, value);
        }

        //Interaction Story
    
        public OptionReacModel OptionStory
        {
            get => _optionStory;
            set => SetProperty(ref _optionStory, value);
        }

        //Send Message
        public OptionReacModel OptionMessage
        {
            get => _optionMessage;
            set => SetProperty(ref _optionMessage, value);
        }

        public OptionReacModel OPtionFriendsSuggestions
        {
            get => _oPtionFriendsSuggestions;
            set => SetProperty(ref _oPtionFriendsSuggestions, value);
        }
    }
}