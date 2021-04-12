using System;

namespace AUTOHLT.MOBILE.Views.FakeUpApp.Views
{
    public class DatLenhPageMasterMenuItem
    {
        public DatLenhPageMasterMenuItem()
        {
            TargetType = typeof(DatLenhPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}