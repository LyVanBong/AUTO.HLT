using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AUTO.DLL.Services
{
    public static class FacebookService
    {
        public static async Task<bool> SendMessage(string message, string id)
        {

            try
            {

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Đăng nhập facebook
        /// </summary>
        /// <param name="user">Tài khoản</param>
        /// <param name="pass">Mật khẩu</param>
        /// <returns>Cookie và token</returns>
        public static async Task<(string Cookie, string Token)> Login(string user, string pass)
        {
            try
            {
                var token = "";
                var cookie = "";
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-1,-1");
                //options.AddArgument("--window-position=-32000,-32000");
                var driver = new ChromeDriver(service, options);
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
                throw e;
            }
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