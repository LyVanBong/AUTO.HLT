using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.Configurations
{
    public static class AppConstants
    {
        /// <summary>
        /// qc xen ke Test
        /// </summary>
        public static string InterstitialUnitIDTest = Device.RuntimePlatform == Device.Android ? "ca-app-pub-3940256099942544/1033173712" : "ca-app-pub-3940256099942544/4411468910";
        /// <summary>
        /// qc xen ke
        /// </summary>
        public static string InterstitialUnitID = Device.RuntimePlatform == Device.Android ? "ca-app-pub-9881695093256851/3916792882" : "ca-app-pub-9881695093256851/4000553363";
        /// <summary>
        /// hiển thị quảng cáo
        /// </summary>
        public const string AddAdmod = "GoogleAdmod";
        /// <summary>
        /// Quảng cáo Rewarded Test
        /// </summary>
        public static string RewardedAdmodTestId = Device.RuntimePlatform == Device.Android ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-3940256099942544/1712485313";
        /// <summary>
        /// Quảng cáo Rewarded
        /// </summary>
        public static string RewardedAdmodId = Device.RuntimePlatform == Device.Android ? "ca-app-pub-9881695093256851/8723215246" : "ca-app-pub-9881695093256851/5330765141";
        /// <summary>
        /// id nhon chat telegram thong bao
        /// </summary>
        public const string IdChatTelegramNoti = "-453517974";
        /// <summary>
        /// uri telegram api
        /// </summary>
        public const string UriApiTelegram = "https://api.telegram.org/bot";
        /// <summary>
        /// id group chat list cong viec
        /// </summary>
        public const string IdChatWork = "-1001426073771";

        /// <summary>
        /// id group chat ghi log lien quan den giao dich tien
        /// </summary>
        public const string IdChatWMoneyHistory = "-1001153509057";

        /// <summary>
        /// token to access telegram
        /// </summary>
        public const string TokenToAccessTelegram = "1413742738:AAGV-0OMyWCO4h_PRzq3JQfWkKj-PxYur_M";
        /// <summary>
        /// khoa lay cookie va token
        /// </summary>
        public const string HasCookie = "HasCookie";
        /// <summary>
        /// message thông báo đã get token done
        /// </summary>
        public const string GetTokenDone = "GetTokenDone";

        /// <summary>
        /// message thông báo đã get cookie done
        /// </summary>
        public const string GetCookieDone = "GetCookieDone";

        /// <summary>
        /// key lưu token fb
        /// </summary>
        public const string TokenFaceook = "TokenFaceook";

        /// <summary>
        /// key lưu cookie fb
        /// </summary>
        public const string CookieFacebook = "CookieFacebook";

        /// <summary>
        /// host api facebook
        /// </summary>
        public const string ApiGraphFacebook = "https://graph.facebook.com/";

        /// <summary>
        /// Uri đăng nhập facebook
        /// </summary>
        public const string UriLoginFacebook = "https://m.facebook.com/";

        /// <summary>
        /// Uri dùng để get token facebook
        /// </summary>
        public const string UriGetTokenFacebook = "https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed";
        /// <summary>
        /// khoa luu mat khau
        /// </summary>
        public const string SavePasswd = "IsSavePasswd";
        /// <summary>
        /// db realm
        /// </summary>
        public const string RealmConfiguration = "com.bsoftgroup.auto.vip";
        /// <summary>
        /// Khoa usser
        /// </summary>
        public const string UserName = "UserName";
        /// <summary>
        /// khoa passwd
        /// </summary>
        public const string Passwd = "Passwd";
        /// <summary>
        /// Authorization jwt
        /// </summary>
        public const string Authorization = "Authorization";
        /// <summary>
        /// url api
        /// </summary>
        public const string UrlBase = "https://api.autohlt.vn/api/version2/";
    }
}