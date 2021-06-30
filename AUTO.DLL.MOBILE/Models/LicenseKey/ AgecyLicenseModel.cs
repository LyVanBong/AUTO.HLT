using System;

namespace AUTO.DLL.MOBILE.Models.LicenseKey
{
    public class AgecyLicenseModel
    {
        public string LicenseKey { get; set; }
        public DateTime? DateCreateKey { get; set; }
        public string IdUser { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateActive { get; set; }
        public string Note { get; set; }

        public int TypeLicenseKey
        {
            get
            {
                if (IsActive)
                {
                    if (string.IsNullOrEmpty(IdUser))
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}