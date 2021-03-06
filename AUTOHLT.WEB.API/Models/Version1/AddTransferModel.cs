using System;

namespace AUTOHLT.WEB.API.Models.Version1
{
    public class AddTransferModel
    {
        public double Discount { get; set; }
        public int Price { get; set; }
        public Guid IdSend { get; set; }
        public Guid IdReceive { get; set; }
        public int TransferType { get; set; }
    }
}