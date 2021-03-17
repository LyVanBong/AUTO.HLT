using System;

namespace AUTO.HLT.ADMIN.Models.Telegram
{
    public class ContentSendTelegramModel
    {
        public string Ten_Thong_Bao { get; set; }
        public int So_Luong { get; set; }
        public object Noi_Dung_Thong_Bao { get; set; }
        public string Id_Nguoi_Dung { get; set; }
        public object Ghi_Chu { get; set; }
        public DateTime Thoi_Gian_Thong_Bao => DateTime.UtcNow;
        public string App => "AUTOTOOL";

    }
}