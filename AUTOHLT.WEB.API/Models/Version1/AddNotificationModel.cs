using System;

namespace AUTOHLT.WEB.API.Models.Version1
{
    public class AddNotificationModel
    {
        public string ContentNotification { get; set; }
        public Guid IdSend { get; set; }
        public Guid IdReceive { get; set; }
        public int NotificationType { get; set; }
        public DateTime DateCreate { get; set; }
    }
}