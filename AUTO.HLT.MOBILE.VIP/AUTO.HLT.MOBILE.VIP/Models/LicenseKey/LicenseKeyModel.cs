using System;

namespace AUTO.HLT.MOBILE.VIP.Models.LicenseKey
{
    public class LicenseKeyModel
    {
        public DateTime DateActive { get; set; }
        public object HistoryUseProduct { get; set; }
        public string LicenseKey { get; set; }
        public string EndDate => DateActive.AddYears(1).ToString("dd/MM/yyyy");
        public int CountEndDate => (DateActive.AddYears(1) - DateTime.Now).Days;
    }
}