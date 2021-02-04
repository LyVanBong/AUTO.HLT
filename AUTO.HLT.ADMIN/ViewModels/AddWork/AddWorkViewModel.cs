using AUTO.HLT.ADMIN.Models.AddWork;
using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Services.Telegram;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar;
using AUTO.HLT.ADMIN.Services.AutoLikeCommentAvatar;

namespace AUTO.HLT.ADMIN.ViewModels.AddWork
{
    public class AddWorkViewModel : ViewModelBase
    {
        private WorkModel _work;
        private string _id;
        private DateTime _dateCreate;
        private string _endDate;
        private string _cookie;
        private string _token;
        private DateTime _todayTime;
        private string _searchUser;
        private IFacebookService _facebookService;

        private string[] _cmtEmojis = new string[]
        {
            ":)",":(",":p",":D",":o",";)","8-)",":*","<3","^_^",":v","(y)",":3"
        };

        private ITelegramService _telegramService;
        private string _idChatWork = "-537876883";
        private IAutoAvatarService _autoAvatarService;
        private UserAutoModel _itemUserAuto;
        private ObservableCollection<UserAutoModel> _dataUserAutoLikeCommentAvatar;


        public ICommand RunWorkAllCommand { get; private set; }
        public ICommand DeleteWorkCommand { get; private set; }
        public string SearchUser
        {
            get => _searchUser;
            set => SetProperty(ref _searchUser, value);
        }

        public ICommand SearchUserAutoCommand { get; private set; }

        public DateTime TodayTime
        {
            get => _todayTime;
            set => SetProperty(ref _todayTime, value);
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string Cookie
        {
            get => _cookie;
            set => SetProperty(ref _cookie, value);
        }

        public string EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public DateTime DateCreate
        {
            get => _dateCreate;
            set => SetProperty(ref _dateCreate, value);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public ICommand SaveAndRunWorkCommand { get; private set; }

        public WorkModel Work
        {
            get => _work;
            set => SetProperty(ref _work, value);
        }

        public ObservableCollection<UserAutoModel> DataUserAutoLikeCommentAvatar
        {
            get => _dataUserAutoLikeCommentAvatar;
            set => SetProperty(ref _dataUserAutoLikeCommentAvatar, value);
        }

        public List<UserAutoModel> _dataUser { get; set; }

        public UserAutoModel ItemUserAuto
        {
            get => _itemUserAuto;
            set => SetProperty(ref _itemUserAuto, value, SelectItemUserAuto);
        }

        public AddWorkViewModel(IRegionManager regionManager, IFacebookService facebookService, ITelegramService telegramService, IAutoAvatarService autoAvatarService) : base(regionManager)
        {
            _autoAvatarService = autoAvatarService;
            _telegramService = telegramService;
            _facebookService = facebookService;
            SaveAndRunWorkCommand = new DelegateCommand(SaveAndRunWork);
            SearchUserAutoCommand = new DelegateCommand(SearchUserAuto);
            DeleteWorkCommand = new DelegateCommand(DeleteWork);
            RunWorkAllCommand = new DelegateCommand(RunWorkAll);
        }

        private void RunWorkAll()
        {
            if (DataUserAutoLikeCommentAvatar != null && DataUserAutoLikeCommentAvatar.Any())
            {
                var countWork = 0;
                foreach (var data in DataUserAutoLikeCommentAvatar)
                {
                    if ((DateTime.Now - data.RegistrationDate).TotalDays < data.ExpiredTime)
                    {
                        new Thread(() => RunWorkAutoLikeAndComment(data)).Start();
                        countWork++;
                    }
                }

                MessageBox.Show("Thanh cong", "Thong bao", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Chua co cong viec nao de thuc hien", "Thong bao", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            GetDataUserAuto();
        }

        private async void DeleteWork()
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var dele = await _autoAvatarService.DeleteUserAuto(Id);
                Id = EndDate = Token = Cookie = "";
                DataUserAutoLikeCommentAvatar.Remove(DataUserAutoLikeCommentAvatar.FirstOrDefault(x => x.Id == Id));
            }
            else
            {
                MessageBox.Show("Nhập tài khoản cần xóa !", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void SearchUserAuto()
        {
            if (!string.IsNullOrWhiteSpace(SearchUser))
            {
                DataUserAutoLikeCommentAvatar = new ObservableCollection<UserAutoModel>(_dataUser.Where(x => x.Id == SearchUser));
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ID của người dùng !", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void SelectItemUserAuto()
        {
            if (ItemUserAuto != null)
            {
                Id = ItemUserAuto.Id;
                DateCreate = ItemUserAuto.RegistrationDate;
                EndDate = ItemUserAuto.ExpiredTime + "";
                TodayTime = DateCreate;
                Cookie = ItemUserAuto.F_Cookie;
                Token = ItemUserAuto.F_Token;
            }
        }

        private async void SaveAndRunWork()
        {
            if (!string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(EndDate) && !string.IsNullOrWhiteSpace(Cookie) && !string.IsNullOrWhiteSpace(Token))
            {
                if (int.TryParse(EndDate, out var result))
                {
                    var data = new UserAutoModel()
                    {
                        RegistrationDate = DateCreate,
                        ExpiredTime = result,
                        F_Cookie = Cookie,
                        F_Token = Token,
                        Id = Id
                    };
                    if ((DateTime.Now - data.RegistrationDate).TotalDays < data.ExpiredTime)
                    {
                        var update = await _autoAvatarService.AddUserAuto(data.Id, data.RegistrationDate.ToString(), data.ExpiredTime + "", data.F_Cookie, data.F_Token);
                        DataUserAutoLikeCommentAvatar.Add(data);
                        new Thread(() => RunWorkAutoLikeAndComment(data)).Start();
                    }
                }
                else
                {
                    MessageBox.Show("Số ngày sử dụng không hợp lệ !", "Thông báo", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui long điền đầy đủ thông tin !", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private async void RunWorkAutoLikeAndComment(UserAutoModel data)
        {
            await _telegramService.SendMessageToTelegram(_idChatWork, $"Bắt đầu chạy auto like và comment cho ID {data.Id}\nVào lúc {DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}");
            UseService(data);
        }

        private async void GetDataUserAuto()
        {
            var data = await _autoAvatarService.GetAllUserAuto();
            if (data != null && data.Code > 0 && data.Data != null)
            {
                _dataUser = data.Data;
                DataUserAutoLikeCommentAvatar =
                    new ObservableCollection<UserAutoModel>(_dataUser);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            DateCreate = TodayTime = DateTime.Today;
            GetDataUserAuto();
        }

        private async Task<FriendsModel> GetAllFriend(string token, string numberFriends)
        {
            try
            {
                var data = await _facebookService.GetAllFriend(numberFriends, token);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<bool> CheckCookieAndToken(string token, string cookie)
        {
            try
            {
                var isCookie = await _facebookService.CheckCookie(cookie);
                if (!isCookie)
                {
                    return false;
                }

                var friends = await GetAllFriend(token, "1");
                if (friends == null || friends.data == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void UseService(UserAutoModel obj)
        {
            try
            {
                var random = new Random();
                while (true)
                {
                    var cookie = obj?.F_Cookie;
                    var token = obj?.F_Token;
                    if (await CheckCookieAndToken(token, cookie))
                    {
                        var lsUid = await _facebookService.GetAllFriend("5000", token);
                        var infoFace = await _facebookService.GetInfoUser("name,picture", token);
                        var data = lsUid?.data;
                        if (data != null && data.Any() && infoFace != null)
                        {
                            try
                            {
                                var updateInfFace = await _autoAvatarService.UpdateUserFaceInfo(obj?.Id, infoFace?.id, infoFace?.name,
                                            infoFace?.picture?.data?.url, true);
                                var totalFriend = 0;
                                var dataUidFriend = await _autoAvatarService.GetUIdFacebook(obj.Id);
                                foreach (var uid in data)
                                {
                                    if (dataUidFriend != null && dataUidFriend.Code > 0 && dataUidFriend.Data != null)
                                    {
                                        var dataUid = dataUidFriend.Data;
                                        var fu = dataUid.FirstOrDefault(x => x.Id == obj.Id);
                                        if (fu != null && fu.Id != null)
                                        {
                                            continue;
                                        }
                                    }
                                    var urlface = $"https://d.facebook.com/{uid.id}";
                                    if (await CheckCookieAndToken(token, cookie))
                                    {
                                        var htmlProfile = await HtmlProfile(urlface, cookie);
                                        if (!string.IsNullOrWhiteSpace(htmlProfile))
                                        {
                                            await AutoLike(htmlProfile, "https://d.facebook.com", cookie, uid, obj,
                                                infoFace);
                                        }
                                    }
                                    else
                                    {
                                        await _telegramService.SendMessageToTelegram(_idChatWork,
                                            $"Autolike: Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                                        return;
                                    }

                                    if (await CheckCookieAndToken(token, cookie))
                                    {
                                        var htmlProfile2 = await HtmlProfile(urlface, cookie);
                                        if (!string.IsNullOrWhiteSpace(htmlProfile2))
                                        {
                                            await AutoComment(htmlProfile2, "https://d.facebook.com", cookie, uid, obj,
                                                infoFace);
                                        }
                                    }
                                    else
                                    {
                                        await _telegramService.SendMessageToTelegram(_idChatWork,
                                            $"AutoComment: Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                                        return;
                                    }

                                    totalFriend++;
                                    await Task.Delay(TimeSpan.FromMilliseconds(random.Next(200000, 500000)));
                                    await _autoAvatarService.AddUidFacebook(obj.Id, uid.id);
                                }

                                await _autoAvatarService.DeleteUserAuto(obj?.Id);
                                await _telegramService.SendMessageToTelegram(_idChatWork,
                                    $"Đã thực hiện auto like và comment {totalFriend} bạn bè của ID {obj.Id} \n UID facebooke {infoFace.id} - {infoFace.name} \n vào lúc {DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")} ");
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"Lỗi : {e.Message} \n Lỗi id {obj.Id}", "thong bao",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        await _telegramService.SendMessageToTelegram(_idChatWork,
                            $"Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                        return;
                    }
                    if ((DateTime.Now - obj.RegistrationDate).TotalDays > obj.ExpiredTime)
                        return;
                    await Task.Delay(TimeSpan.FromMilliseconds(random.Next(1800000, 2000000)));
                }
            }
            catch (Exception)
            {
                await _telegramService.SendMessageToTelegram(_idChatWork, $"ID {obj.Id} loi vui long kiem tra lai");
            }
            finally
            {
                await _telegramService.SendMessageToTelegram(_idChatWork, $"ID {obj.Id} loi vui long kiem tra lai");
            }
        }

        private async Task<string> HtmlProfile(string urlface, string cookie)
        {
            var html = await _facebookService.GetHtmlFacebook(urlface, cookie);
            if (html != null)
            {
                var dataUrlProfileFacebook = Regex.Match(html, @"<a href=""/photo.php.*?""")?.Value;
                if (!string.IsNullOrWhiteSpace(dataUrlProfileFacebook))
                {
                    var url = Regex.Match(dataUrlProfileFacebook, @"<a href=""(.*?)""")?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        var htmlProfile = await _facebookService.GetHtmlFacebook(HttpUtility.HtmlDecode("https://d.facebook.com" + url), cookie);
                        if (!string.IsNullOrWhiteSpace(htmlProfile))
                        {
                            return htmlProfile;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// auto comment avatar
        /// </summary>
        /// <param name="htmlProfile"></param>
        /// <param name="urlface"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task AutoComment(string htmlProfile, string urlface, string cookie, FriendsModel.Datum data, UserAutoModel obj, NamePictureUserModel info)
        {
            if (!string.IsNullOrWhiteSpace(htmlProfile))
            {
                var urlPostComment = Regex.Match(htmlProfile, @"""><form method=""post"" action=""(.*?)""")?.Groups[1]
                    ?.Value;
                if (!string.IsNullOrWhiteSpace(urlPostComment))
                {
                    var url = HttpUtility.HtmlDecode(urlface + urlPostComment);
                    var fb_dtsg = Regex.Match(htmlProfile, @"""><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")
                        ?.Groups[1]?.Value;
                    var jazoest = Regex.Match(htmlProfile, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)""")
                        ?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(fb_dtsg) && !string.IsNullOrWhiteSpace(jazoest))
                    {
                        var ran = new Random();
                        var para = new List<RequestParameter>()
                        {
                            new RequestParameter("fb_dtsg",fb_dtsg),
                            new RequestParameter("jazoest",jazoest),
                            new RequestParameter("comment_text",_cmtEmojis[ran.Next(13)]),
                        };
                        var html = await _facebookService.PostHtmlFacebook(url, cookie, para);
                        try
                        {
                            var addHis = await _autoAvatarService.AddHistoryAuto(obj.Id, info.id, info.name,
                                info.picture.data.url, "Auto Comment Avatar", data.id, data.name, data.picture.data.url);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Loi: " + e.Message, "Thong bao", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// auto like avatar
        /// </summary>
        /// <param name="htmlProfile"></param>
        /// <param name="urlface"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task AutoLike(string htmlProfile, string urlface, string cookie, FriendsModel.Datum data, UserAutoModel obj, NamePictureUserModel info)
        {
            if (!string.IsNullOrWhiteSpace(htmlProfile))
            {
                var datareactions = Regex.Match(htmlProfile, @"<a href=""/reactions/picker/\?is_permalink=1.*?</a>")?.Value;
                if (!string.IsNullOrWhiteSpace(datareactions))
                {
                    var reac = Regex.Match(datareactions, @"<a href=""(.*?)""")?.Groups[1]?.Value + "\"";
                    var para = new List<RequestParameter>()
            {
                new RequestParameter("is_permalink", Regex.Match(reac, @"/?is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ft_id", Regex.Match(reac, @"&amp;ft_id=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("origin_uri", Regex.Match(reac, @"&amp;origin_uri=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(reac, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("refid", Regex.Match(reac, @"&amp;refid=(.*?)""")?.Groups[1]?.Value),
            };
                    var htmlReaction = await _facebookService.GetHtmlFacebook($"{urlface}/reactions/picker", cookie, para);

                    Regex regex = new Regex(@"<a href=""/ufi/reaction/.*?</a>",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlReaction);
                    var ran = new Random();
                    var like = matchCollection[ran.Next(7)]?.Value;
                    var paraLike = new List<RequestParameter>()
            {
                new RequestParameter("ft_ent_identifier", Regex.Match(like, @"/?ft_ent_identifier=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("reaction_type", Regex.Match(like, @"&amp;reaction_type=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("is_permalink", Regex.Match(like, @"&amp;is_permalink=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("basic_origin_uri", Regex.Match(like, @"amp;basic_origin_uri=(.*?)&")?.Groups[1]?.Value),
                // new RequestParameter("_ft_",Regex.Match(like,@"&amp;_ft_(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("av", Regex.Match(like, @"&amp;av=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("ext", Regex.Match(like, @"&amp;ext=(.*?)&")?.Groups[1]?.Value),
                new RequestParameter("hash", Regex.Match(like, @"&amp;hash=(.*?)""")?.Groups[1]?.Value),
            };
                    var uriLike = $"{urlface}{Regex.Match(like, @"<a href=""(.*?)""")?.Groups[1]?.Value}";
                    var actionLike = await _facebookService.GetHtmlFacebook(HttpUtility.HtmlDecode(uriLike), cookie);

                    try
                    {
                        var addHis = await _autoAvatarService.AddHistoryAuto(obj.Id, info.id, info.name,
                            info.picture.data.url, "Auto like Avatar", data.id, data.name, data.picture.data.url);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Loi: " + e.Message, "Thong bao", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}