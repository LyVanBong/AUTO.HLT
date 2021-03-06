using System;

namespace AUTOHLT.WEB.API.Models.Version1
{
    public class ServiceUseHistoryModel
    {
        public Guid IdProductType { get; set; }
        public string Content { get; set; }
        public Guid IdUser { get; set; }

        /// <summary>
        /// so dich vu da su dung
        /// </summary>
        public int Number { get; set; }

        public DateTime DateCreate { get; set; }
    }
}