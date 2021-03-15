using System;

namespace AUTO.HLT.ADMIN.Models.Main
{
    public class UserModel
    {
        public int STT { get; set; }
        public string ID { get; set; }
        public DateTime NgayDangKy { get; set; }
        public int ThoiHan { get; set; }
        public string Cookie { get; set; }
        public string Token { get; set; }
        public bool ThaTim { get; set; }
        public bool TuongTacAnhDaiDien { get; set; }
        public int TrangThai { get; set; }
        public string Note { get; set; }
        public DateTime NgayHetHan => NgayDangKy.AddDays(ThoiHan);
    }
}
