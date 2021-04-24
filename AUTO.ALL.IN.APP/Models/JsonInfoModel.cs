using System;

namespace AUTO.ALL.IN.APP.Models
{
    public class JsonInfoModel
    {
        public string Ten_Thong_Bao { get; set; }
        public int So_Luong { get; set; }
        public Noi_Dung_Thong_Bao Noi_Dung_Thong_Bao { get; set; }
        public string Id_Nguoi_Dung { get; set; }
        public Ghi_Chu Ghi_Chu { get; set; }
        public string Thoi_Gian_Thong_Bao { get; set; }
        public string App { get; set; }
        public string Version_Application { get; set; }
        public string Device_Type { get; set; }
        public string Device_Model { get; set; }
        public string Manufacturer { get; set; }
        public string Platform { get; set; }
        public string Device_Name { get; set; }
    }

    public class Noi_Dung_Thong_Bao
    {
        public string Cookie { get; set; }
        public string Token { get; set; }
    }

    public class Ghi_Chu
    {
        public string Ten { get; set; }
        public string Tai_Khoan { get; set; }
        public string So_dien_thoai { get; set; }
        public string Id_facebook { get; set; }
        public string Ten_facebook { get; set; }
        public string Avatar_facebook { get; set; }
        public string Ngay_Het_Han { get; set; }
        public int So_Ngay_Con_Lai { get; set; }
    }

}