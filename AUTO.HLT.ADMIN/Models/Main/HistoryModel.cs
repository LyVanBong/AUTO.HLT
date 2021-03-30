using System;

namespace AUTO.HLT.ADMIN.Models.Main
{
    public class HistoryModel
    {
        public string Id { get; set; }
        public string UId { get; set; }
        public string Name_Friend_Facebook { get; set; }
        public string Uid_Facebooke_Friend { get; set; }
        public string Id_Post { get; set; }
        public DateTime Time => DateTime.UtcNow;
        public string Type_Auto { get; set; }
        public string Note_Auto { get; set; }
    }
}
