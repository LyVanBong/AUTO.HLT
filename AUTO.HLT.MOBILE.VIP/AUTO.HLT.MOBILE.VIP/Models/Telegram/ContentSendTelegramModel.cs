using System;

namespace AUTO.HLT.MOBILE.VIP.Models.Telegram
{
    public class ContentSendTelegramModel
    {
        public string Ten_Dich_Vu_Yeu_Cau { get; set; }
        public int So_Luong { get; set; }
        public object Noi_Dung_Yeu_Cau { get; set; }
        public string Id_Nguoi_Dung { get; set; }
        public DateTime Thoi_Gian_Yeu_Cau => DateTime.Now;
        public string App => "AUTOVIP";
        public object Ghi_Chu { get; set; }
    }
}