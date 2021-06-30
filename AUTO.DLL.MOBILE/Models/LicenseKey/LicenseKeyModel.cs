using System;

namespace AUTO.DLL.MOBILE.Models.LicenseKey
{
    public class LicenseKeyModel
    {
        public DateTime DateActive { get; set; }
        public string HistoryUseProduct { get; set; }
        public string LicenseKey { get; set; }
        public int TypeKey { get; set; }
        public string EndDate => DateActive.AddDays(TypeKey).ToString("dd/MM/yyyy");
        public int CountEndDate => (DateActive.AddDays(TypeKey) - DateTime.Now).Days;
    }
}