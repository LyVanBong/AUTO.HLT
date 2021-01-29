using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using AUTO.HLT.ADMIN.Models.AddWork;
using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Databases;
using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Services.Telegram;

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
        private AutohltAdminEntities _dbAdminEntities;
        private ObservableCollection<GetAllUserAutoLikeComment_Result> _dataUserAutoLikeCommentAvatar;
        private GetAllUserAutoLikeComment_Result _itemUserAuto;
        private string _searchUser;
        private List<GetAllUserAutoLikeComment_Result> _dataUser = new List<GetAllUserAutoLikeComment_Result>();
        private IFacebookService _facebookService;
        private string[] _cmtEmojis = new string[]
        {
            ":)",":(",":p",":D",":o",";)","8-)",":*","<3","^_^",":v","(y)",":3"
        };

        private ITelegramService _telegramService;
        private string _idChatWork = "-537876883";

        public string SearchUser
        {
            get => _searchUser;
            set => SetProperty(ref _searchUser, value);
        }

        public ICommand SearchUserAutoCommand { get; private set; }
        public GetAllUserAutoLikeComment_Result ItemUserAuto
        {
            get => _itemUserAuto;
            set => SetProperty(ref _itemUserAuto, value, SelectItemUserAuto);
        }

        public ObservableCollection<GetAllUserAutoLikeComment_Result> DataUserAutoLikeCommentAvatar
        {
            get => _dataUserAutoLikeCommentAvatar;
            set => SetProperty(ref _dataUserAutoLikeCommentAvatar, value);
        }

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

        public AddWorkViewModel(IRegionManager regionManager, IFacebookService facebookService, ITelegramService telegramService) : base(regionManager)
        {
            _telegramService = telegramService;
            _facebookService = facebookService;
            SaveAndRunWorkCommand = new DelegateCommand(SaveAndRunWork);
            _dbAdminEntities = new AutohltAdminEntities();
            SearchUserAutoCommand = new DelegateCommand(SearchUserAuto);
        }

        private void SearchUserAuto()
        {
            if (Guid.TryParse(SearchUser, out var res))
            {

                DataUserAutoLikeCommentAvatar = new ObservableCollection<GetAllUserAutoLikeComment_Result>(_dataUser.Where(x => x.Id == res));
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ID của người dùng !", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void SelectItemUserAuto()
        {
            var data = ItemUserAuto;
        }
        private void SaveAndRunWork()
        {
            if (!string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(EndDate) && !string.IsNullOrWhiteSpace(Cookie) && !string.IsNullOrWhiteSpace(Token))
            {
                if (int.TryParse(EndDate, out var result))
                {
                    if (Guid.TryParse(Id, out var id))
                    {
                        var update =
                            _dbAdminEntities.UpdateUserAutoLikeComment(id, DateCreate, result, Cookie, Token);
                        var data = new GetAllUserAutoLikeComment_Result()
                        {
                            DateCreate = DateCreate,
                            EndDate = result,
                            Id = new Guid(Id),
                            F_Cookie = Cookie,
                            F_Tooken = Token,
                        };
                        GetDataUserAuto();
                        new Thread(() => RunWorkAutoLikeAndComment(data)).Start();
                    }
                    else
                    {
                        MessageBox.Show("ID không hợp lệ vui lòng thử lại !", "Thông báo", MessageBoxButton.OK,
                            MessageBoxImage.Information);
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

        private async void RunWorkAutoLikeAndComment(GetAllUserAutoLikeComment_Result data)
        {
            await _telegramService.SendMessageToTelegram(_idChatWork, $"Bắt đầu chạy auto like và comment cho ID {data.Id}\nVào lúc {DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}");
            UseService(data);
        }

        private void GetDataUserAuto()
        {
            _dataUser = _dbAdminEntities.GetAllUserAutoLikeComment().ToList();
            DataUserAutoLikeCommentAvatar =
                new ObservableCollection<GetAllUserAutoLikeComment_Result>(_dataUser);
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
                throw;
            }

            return null;
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
            catch (Exception e)
            {
                return false;
            }
        }
        private async void UseService(GetAllUserAutoLikeComment_Result obj)
        {
            var random = new Random();
            while (true)
            {
                var cookie = obj.F_Cookie;
                var token = obj.F_Tooken;
                if (await CheckCookieAndToken(token, cookie))
                {
                    var lsUid = await _facebookService.GetAllFriend("5000", token);
                    var infoFace = await _facebookService.GetInfoUser("name,picture", token);
                    var data = lsUid?.data;
                    if (data != null && data.Any() && infoFace != null)
                    {
                        try
                        {
                            var totalFriend = 0;
                            foreach (var uid in data)
                            {
                                var urlface = $"https://d.facebook.com/{uid.id}";
                                if (await CheckCookieAndToken(token, cookie))
                                {
                                    var htmlProfile = await HtmlProfile(urlface, cookie);
                                    await AutoLike(htmlProfile, "https://d.facebook.com", cookie, uid, obj, infoFace);
                                }
                                else
                                {
                                    await _telegramService.SendMessageToTelegram(_idChatWork, $"Autolike: Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                                    return;
                                }

                                if (await CheckCookieAndToken(token, cookie))
                                {
                                    var htmlProfile2 = await HtmlProfile(urlface, cookie);
                                    await AutoComment(htmlProfile2, "https://d.facebook.com", cookie, uid, obj, infoFace);
                                }
                                else
                                {
                                    await _telegramService.SendMessageToTelegram(_idChatWork, $"AutoComment: Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                                    return;
                                }

                                totalFriend++;
                                // sau mỗi lần thực hiện nhiệm vụ sẽ tạm nghỉ từ 5 - 10 phút rồi làm tiếp
                                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(200000, 500000)));
                            }
                            await _telegramService.SendMessageToTelegram(_idChatWork, $"Đã thực hiện auto like và comment {totalFriend} của ID {obj.Id} \n UID facebooke {infoFace.id} - {infoFace.name} \n vào lúc {DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")} ");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"Lỗi : {e.Message} \n Lỗi id {obj.Id}", "thong bao", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    await _telegramService.SendMessageToTelegram(_idChatWork, $"Cookie or Token facebook của id {obj.Id} đã hỏng gọi khách để yêu cầu họ cài lại auto like và comment avatar bạn bè");
                    return;
                }
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
        private async Task AutoComment(string htmlProfile, string urlface, string cookie, FriendsModel.Datum data, GetAllUserAutoLikeComment_Result obj, NamePictureUserModel info)
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
                        var addHis = _dbAdminEntities.AddHistoryAutoLikeComment(obj.Id, info.id, info.name,
                            info.picture.data.url, "Auto Comment Avatar", data.id, data.name, data.picture.data.url);
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
        private async Task AutoLike(string htmlProfile, string urlface, string cookie, FriendsModel.Datum data, GetAllUserAutoLikeComment_Result obj, NamePictureUserModel info)
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

                    var addHis = _dbAdminEntities.AddHistoryAutoLikeComment(obj.Id, info.id, info.name,
                        info.picture.data.url, "Auto like Avatar", data.id, data.name, data.picture.data.url);
                }
            }
        }
    }
}