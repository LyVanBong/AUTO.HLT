using System;

namespace AUTOHLT.WEB.API.Models.Version2.LicenseKey
{
    public class AddLicenseModel
    {
        public DateTime DateActiveLicense { get; set; }
        public Guid LicenseKey { get; set; }
    }
}