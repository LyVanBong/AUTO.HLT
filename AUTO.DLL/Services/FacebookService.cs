using AUTO.DLL.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AUTO.DLL.Services
{
    public static class FacebookService
    {
        /// <summary>
        /// binh luan vao bai viet cua ban be
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cookie"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static async Task<int> CommentPost(string html, string cookie, string comment)
        {
            var result = 0;
            if (string.IsNullOrEmpty(comment))
                return result;
            try
            {
                var random = new Random();
                var cmt = comment.Split('\n');
                if ((await RestSharpService.PostAsync(HttpUtility.HtmlDecode(@"https://d.facebook.com" + Regex.Matches(html, @"action=""(.*?)""")[0].Groups[1].Value), new List<RequestParameter>()
                {
                    new RequestParameter("fb_dtsg", Regex.Matches(html, @"name=""fb_dtsg"" value=""(.*?)""")[0].Groups[1].Value),
                    new RequestParameter("jazoest", Regex.Matches(html, @"name=""jazoest"" value=""(.*?)""")[0].Groups[1].Value),
                    new RequestParameter("comment_text", cmt[random.Next(0,cmt.Length-1)]),
                }, cookie)).Contains("/home.php?ref_component=mbasic_home_logo"))
                {
                    result = 1;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }
        /// <summary>
        /// tương tác với bài viết của của bạn bè
        /// </summary>
        /// <param name="fbid"></param>
        /// <param name="cookie"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static async Task<int> ReactionPost(string fbid, string cookie, int option, string comment)
        {
            var result = 0;
            try
            {
                var html = await FacebookService.GetHtmlFacebook("https://d.facebook.com/" + HttpUtility.HtmlDecode(
                    Regex.Matches(
                            await FacebookService.GetHtmlFacebook(
                                "https://d.facebook.com/reactions/picker/?is_permalink=1&ft_id=" + fbid, cookie),
                            @"/ufi/reaction.*?""")[option].Value
                        .Replace("\"", "")), cookie);
                if (html.Contains("/home.php?ref_component=mbasic_home_logo"))
                {
                    result += await CommentPost(html, cookie, comment);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }
        /// <summary>
        /// get id avartar
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static async Task<string> GetIdPostFacebook(string uid, string cookie, int option)
        {
            var id = "";
            try
            {
                id = Regex.Match(
                    Regex.Matches(await FacebookService.GetHtmlFacebook("https://d.facebook.com/" + uid, cookie),
                        @"\/photo.php\?fbid\=.*?refid\=17")[option].Value, @"fbid=(.*?)&").Groups[1].Value;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return id;
        }

        /// <summary>
        /// post facebook get html
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<string> PostHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null)
        {
            var html = "";
            try
            {
                html = await RestSharpService.PostAsync(url, parameters, cookie);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return html;
        }

        /// <summary>
        /// lấy dữ liệu facebook thông qua api
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static async Task<string> GetApiFacebook(string accessToken, string fields)
        {
            var json = "";
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields",fields),
                    new RequestParameter("access_token",accessToken),
                };
                json = await RestSharpService.GetAsync("https://graph.facebook.com/v9.0/me", para);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi: " + e.Message);
            }

            return json;
        }
        /// <summary>
        /// Lấy thông tin tài khoản facebook qua api
        /// </summary>
        /// <param name="accessToken">Token facebook</param>
        /// <param name="fields">Các thông tin cần nấy mặc định là ảnh và tên</param>
        /// <returns>Các thông tin đã yêu cầu cại fields</returns>
        public static async Task<NamePictureUserModel> GetInfoUser(string accessToken, string fields = "name,picture")
        {
            try
            {
                if (accessToken != null)
                {
                    var para = new List<RequestParameter>
                    {
                        new RequestParameter("fields",fields),
                        new RequestParameter("access_token",accessToken),
                    };
                    var data = await RestSharpService.GetAsync("https://graph.facebook.com/v9.0/me", para);
                    if (data != null)
                    {
                        var info = JsonSerializer.Deserialize<NamePictureUserModel>(data);
                        if (info != null)
                            return info;
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        /// <summary>
        /// Lấy html của một trang web
        /// </summary>
        /// <param name="url">Đường đẫn của trang web</param>
        /// <param name="cookie">cookie của trang web</param>
        /// <param name="parameters">các tham số cần</param>
        /// <returns>Html</returns>
        public static async Task<string> GetHtmlFacebook(string url, string cookie = null, List<RequestParameter> parameters = null)
        {
            try
            {
                var html = await RestSharpService.GetAsync(url, parameters, cookie);
                return html;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Lấy danh sách bạn bè
        /// </summary>
        /// <param name="token">Token facebook</param>
        /// <param name="fields">các thông tin cần lấy</param>
        /// <param name="limit">Số lượng bạn bè muỗn lấy</param>
        /// <returns>Danh sách bạn bè</returns>
        public static async Task<FriendsModel> GetIdFriends(string token, string fields = "id,name", string limit = "5000")
        {
            try
            {
                var rd = new Random();
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields",fields),
                    new RequestParameter("limit",limit),
                    new RequestParameter("access_token",token),
                };
                var data = await RestSharpService.GetAsync("https://graph.facebook.com/v9.0/me/friends", para);
                if (data != null)
                {
                    return JsonSerializer.Deserialize<FriendsModel>(data);
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
        /// <summary>
        /// Kiểm tra cookie và token còn sống không
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="cookie">cookie</param>
        /// <returns>true là còn sống, false là đã die</returns>
        public static async Task<bool> CheckTokenCookie(string token, string cookie)
        {
            try
            {
                if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(token))
                {
                    return false;
                }
                else
                {
                    var html = await RestSharpService.GetAsync(@"https://m.facebook.com/", null, cookie);
                    var friend = await GetInfoUser(token);
                    if (friend != null && html != null)
                    {
                        if (html.Contains("mbasic_logout_button") && friend.name != null)
                            return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// Lấy tham số facebook
        /// </summary>
        /// <param name="cookie">cookie facebook</param>
        /// <returns>jazoest và fbdtsg</returns>
        public static async Task<(string Jazoest, string Fbdtsg)> GeJazoestAndFbdtsg(string cookie)
        {
            try
            {
                var html = await GetHtmlFacebook("https://m.facebook.com/", cookie);
                var jazoest = Regex.Match(html, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)"" autocomplete=""off"" /><input type=""hidden"" name=""privacyx""")?.Groups[1]?.Value;
                var fbdtsg = Regex.Match(html, @"id=""mbasic-composer-form""><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")?.Groups[1]?.Value;
                return (Jazoest: jazoest, Fbdtsg: fbdtsg);
            }
            catch (Exception e)
            {
                return ("", "");
            }
        }
        public static async Task<bool> SendMessage(string message, string id, string cookie)
        {

            try
            {
                var getParaFace = await GeJazoestAndFbdtsg(cookie);
                var para = new List<RequestParameter>()
                {
                    new RequestParameter("fb_dtsg",getParaFace.Fbdtsg),
                    new RequestParameter("jazoest",getParaFace.Jazoest),
                    new RequestParameter("body",message),
                    new RequestParameter("ids",id),
                };
                var data = await RestSharpService.PostAsync("https://d.facebook.com/messages/send/?icm=1", para,
                    cookie);
#if DEBUG
                Console.WriteLine("Send message data : " + data);
#endif
                if (data.Contains("mbasic_logout_button"))
                    return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// Thả tim story facebook
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task AutoDropHeartFacebookStory(string url, string cookie, int optionStory = 1)
        {
            ChromeDriver driver = new ChromeDriver();
            try
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                //options.AddArgument("--window-position=-1,-1");
                //options.AddArgument("--window-position=-32000,-32000");
                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl("https://m.facebook.com/");

                if (cookie != null)
                {
                    var ck = cookie.Split(';');
                    foreach (var s in ck)
                    {
                        var c = s.Split('=');
                        driver.Manage().Cookies.AddCookie(new Cookie(c[0], c[1]));
                    }
                }

                driver.Navigate().GoToUrl("https://m.facebook.com/" + url);
                await Task.Delay(5000);

                var element =
                    driver.FindElementByXPath(
                        @"/html/body/div[1]/div/div[2]/div/div[2]/div/div/div/div[2]/span/div[3]/div[9]/div");
                element.Click();
                await Task.Delay(1000);

                var lsElements = driver.FindElementsByClassName(@"_7ko-");
                var random = new Random();
                var num = random.Next(3, 6);
                for (int i = 0; i < num; i++)
                {
                    lsElements[optionStory].Click();
                    await Task.Delay(333);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Loi: " + ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
        /// <summary>
        /// get html bằng ChromeDriver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static async Task<string> GetHtmlChrome(string url, string cookie = null)
        {
            var driver = new ChromeDriver();
            try
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-1,-1");
                options.AddArgument("--window-position=-32000,-32000");
                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl(url);

                if (cookie != null)
                {
                    var ck = cookie.Split(';');
                    foreach (var s in ck)
                    {
                        var c = s.Split('=');
                        driver.Manage().Cookies.AddCookie(new Cookie(c[0], c[1]));
                    }
                }

                driver.Navigate().Refresh();
                await Task.Delay(5000);
                var html = driver.PageSource;
                return html;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Loi: " + ex.Message);
            }
            finally
            {
                driver.Quit();
            }

            return null;
        }
        /// <summary>
        /// Đăng nhập facebook
        /// </summary>
        /// <param name="user">Tài khoản</param>
        /// <param name="pass">Mật khẩu</param>
        /// <returns>Cookie và token</returns>
        public static async Task<(string Cookie, string Token)> LoginFacebook(string user, string pass)
        {
            var driver = new ChromeDriver();
            try
            {
                var token = "";
                var cookie = "";
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-1,-1");
                //options.AddArgument("--window-position=-32000,-32000");
                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl("https://www.facebook.com/");
                await Task.Delay(2000);
                // nhập tài khoản
                var element = driver.FindElementById("email");
                element.SendKeys(user);
                await Task.Delay(3000);
                // nhập mật khẩu
                element = driver.FindElementById("pass");
                element.SendKeys(pass);
                await Task.Delay(4000);
                // login
                element = driver.FindElementByName("login");
                element.Click();
                await Task.Delay(2000);
                for (int j = 0; j < 20; j++)
                {
                    cookie = "";
                    var allCookie = driver.Manage().Cookies.AllCookies;
                    var countCookie = allCookie?.Count;
                    for (int i = 0; i < countCookie; i++)
                    {
                        var c = allCookie[i];
                        cookie += c.Name + "=" + c.Value + ((i + 1) == countCookie ? "" : ";");
                    }

                    if (cookie.Contains("c_user="))
                    {
                        token = GetToken(driver);
#if DEBUG
                        Console.WriteLine("cookie: " + cookie);
                        Console.WriteLine("token: " + token);
#endif
                        return (Cookie: cookie, Token: token);
                    }

                    await Task.Delay(1000);
                }

                token = GetToken(driver);
#if DEBUG
                Console.WriteLine("cookie: " + cookie);
                Console.WriteLine("token: " + token);
#endif
                return (Cookie: cookie, Token: GetToken(driver));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loi: " + e.Message);
            }
            finally
            {
                driver.Quit();
            }

            return (null, null);
        }
        /// <summary>
        /// loc lay token facebook
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private static string GetToken(ChromeDriver driver)
        {
            string token;
            driver.Navigate().GoToUrl("https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed");
            token = Regex.Match(driver?.PageSource, @"\,\\""accessToken\\"":\\""(.*?)\\")?.Groups[1]?.Value;
            driver.Quit();
            return token;
        }
    }
}