using System;
using System.Collections.Generic;

namespace AUTO.HLT.CRM.Models.TopUp
{
    public class HistoryTopUpModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<TopUpModel> Data { get; set; }
    }

    public class TopUpModel
    {
        public int  Stt { get; set; }
        public string ID { get; set; }
        public DateTime DateCreate { get; set; }
        public float Discount { get; set; }
        public int Price { get; set; }
        public string ID_Send { get; set; }
        public string ID_Receive { get; set; }
        public int MoneyTransferType { get; set; }
    }
}