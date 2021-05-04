using System;
using System.Collections.Generic;

namespace AUTOHLT.CONSOLE
{
    public class AllUserActiveModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<Lincese> Data { get; set; }
    }

    public class Lincese
    {
        public string LicenseKey { get; set; }
        public string IdAgency { get; set; }
        public DateTime DateCreateKey { get; set; }
        public string IdUser { get; set; }
        public int TypeKey { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateActive { get; set; }
        public object Note { get; set; }
        public string HistoryUseProduct { get; set; }
        public string IdCreateKey { get; set; }
    }
}