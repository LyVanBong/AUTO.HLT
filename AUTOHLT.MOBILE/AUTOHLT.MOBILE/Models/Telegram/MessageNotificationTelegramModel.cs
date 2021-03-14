using System;
using Xamarin.Essentials;

namespace AUTOHLT.MOBILE.Models.Telegram
{
    public class MessageNotificationTelegramModel
    {
        public string Ten_Thong_Bao { get; set; }
        public int So_Luong { get; set; }
        public object Noi_Dung_Thong_Bao { get; set; }
        public string Id_Nguoi_Dung { get; set; }
        public object Ghi_Chu { get; set; }
        public DateTime Thoi_Gian_Thong_Bao => DateTime.UtcNow;
        public string App => "AUTOHLT";
        public string Version_Application => "Version " + VersionTracking.CurrentVersion + $" ({VersionTracking.CurrentBuild})";

        public string Device_Type => DeviceInfo.DeviceType.ToString();
        public string Device_Model => DeviceInfo.Model;
        public string Manufacturer => DeviceInfo.Manufacturer;
    }
}