using System;

namespace AUTO.HLT.ADMIN.Models.AddWork
{
    public class WorkModel
    {
        public string Id { get; set; }
        public DateTime DateCreate { get; set; }
        public int EndDate { get; set; }
        public string Cookie { get; set; }
        public string Token { get; set; }
    }
}