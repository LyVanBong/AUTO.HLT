using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Views.AddWork;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Views.AutoHltCrm;
using AUTO.HLT.ADMIN.Views.CheckWork;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private IFacebookService _facebookService;
        private string _title = "AUTOHLT ADMIN";
        private IRegionManager _regionManager;

        private string[] _cmtEmojis = new string[]
        {
            ":)",":(",":p",":D",":o",";)","8-)",":*","<3","^_^",":v","(y)",":3"
        };

        public ICommand UseFeatureCommand { get; private set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ICommand AutoLikeCommand { get; private set; }
        public MainWindowViewModel(IFacebookService facebookService, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _facebookService = facebookService;
            UseFeatureCommand = new DelegateCommand<string>(UseFeature);
        }

        private void UseFeature(string key)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            switch (key)
            {
                case "0":
                    _regionManager.RequestNavigate("ContentRegion", nameof(AddWorkView));
                    break;
                case "1":
                    _regionManager.RequestNavigate("ContentRegion", nameof(CheckWorkView));
                    break;
                case "2":
                    _regionManager.RequestNavigate("ContentRegion", nameof(AutoHltCrmView));
                    break;
            }
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
        private async void UseService()
        {

            var cookie =
                "datr=8UYSYGQgkW8PzNvUe0Beh8m2;sb=8UYSYO9m33eJJ1XpqaRShRg6;fr=1KBAI6pFamqI2Cvqc.AWWieBQWmJrXCzg6OenNPBTD-5U.BgEkcC.ae.AAA.0.0.BgEoqj.AWVCBMGXKTA;locale=vi_VN;c_user=100027295904383;xs=37%3AYkLTI4f0u82Nrw%3A2%3A1611827875%3A5627%3A11419";
            var token = "EAAAAZAw4FxQIBAKRgl6LZCVg0cctZA0ZAPNBsn6NkHZA5YQQN2f0Xy2HpAHQO5IOvnA8kXCV3pGAEKZAzAjSqvUPlZCFFIhLaailn71KkwakyVWZC4ZCsIyOrcTULZAqhGxGeKgIRvdABNWVyDJEuiaZCW3IOk4CPSnxddzIzj3MXdbPwZDZD";
            if (await CheckCookieAndToken(token, cookie))
            {
                var lsUid = await _facebookService.GetAllFriend("5000", token);
                var data = lsUid?.data;
                if (data != null && data.Any())
                {
                    try
                    {
                        foreach (var uid in data)
                        {
                            var urlface = $"https://d.facebook.com/{uid.id}";
                            var htmlProfile = await HtmlProfile(urlface, cookie);
                            await AutoLike(htmlProfile, "https://d.facebook.com", cookie);
                            var htmlProfile2 = await HtmlProfile(urlface, cookie);
                            await AutoComment(htmlProfile2, "https://d.facebook.com", cookie);
                        }
                        MessageBox.Show("Thanh cong !", $"Thong bao", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Lỗi : {e.Message}", "thong bao", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
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
        private async Task AutoComment(string htmlProfile, string urlface, string cookie)
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
        private async Task AutoLike(string htmlProfile, string urlface, string cookie)
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
                }
            }
        }
    }
}
