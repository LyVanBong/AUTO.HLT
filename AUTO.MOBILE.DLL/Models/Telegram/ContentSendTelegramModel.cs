using System;

namespace AUTO.MOBILE.DLL.Models.Telegram
{
    public class ContentSendTelegramModel
    {
        public string Ten_Thong_Bao { get; set; }
        public int So_Luong { get; set; }
        public object Noi_Dung_Thong_Bao { get; set; }
        public string Id_Nguoi_Dung { get; set; }
        public object Ghi_Chu { get; set; }
        public DateTime Thoi_Gian_Thong_Bao => DateTime.UtcNow;
        public string App { get; set; }
        public string Version_Application { get; set; }
        public string Device_Type { get; set; }
        public string Device_Model { get; set; }
        public string Manufacturer { get; set; }
        public string Platform { get; set; }
        public string Device_Name { get; set; }

        public ContentSendTelegramModel(string tenThongBao, int soLuong, object noiDungThongBao, string idNguoiDung, object ghiChu, string app, string versionApplication, string deviceType, string deviceModel, string manufacturer, string platform, string deviceName)
        {
            Ten_Thong_Bao = tenThongBao;
            So_Luong = soLuong;
            Noi_Dung_Thong_Bao = noiDungThongBao;
            Id_Nguoi_Dung = idNguoiDung;
            Ghi_Chu = ghiChu;
            App = app;
            Version_Application = versionApplication;
            Device_Type = deviceType;
            Device_Model = deviceModel;
            Manufacturer = manufacturer;
            Platform = platform;
            Device_Name = deviceName;
        }
    }
}