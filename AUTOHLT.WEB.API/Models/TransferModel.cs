using System;

namespace AUTOHLT.WEB.API.Models
{
    public class TransferModel
    {
        public Guid IdSend { get; set; }
        public Guid IdReceive { get; set; }
        public int Price { get; set; }
    }
}