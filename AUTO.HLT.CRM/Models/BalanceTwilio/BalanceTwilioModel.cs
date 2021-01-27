namespace AUTO.HLT.CRM.Models.BalanceTwilio
{
    public class BalanceTwilioModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string currency { get; set; }
        public string balance { get; set; }
        public string account_sid { get; set; }
    }
}