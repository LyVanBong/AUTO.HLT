using System;

namespace AUTOHLT.WEB.API.Models.Version2.LicenseKey
{
    public class CreatekeyModel
    {
        public Guid IdUserAgecy { get; set; }
        public int TypeKey { get; set; }
        public int AmountKey { get; set; }
    }
}