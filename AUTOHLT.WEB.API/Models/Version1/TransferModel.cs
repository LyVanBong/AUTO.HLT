﻿using System;

namespace AUTOHLT.WEB.API.Models.Version1
{
    public class TransferModel
    {
        public Guid IdSend { get; set; }
        public Guid IdReceive { get; set; }
        public int Price { get; set; }
    }
}