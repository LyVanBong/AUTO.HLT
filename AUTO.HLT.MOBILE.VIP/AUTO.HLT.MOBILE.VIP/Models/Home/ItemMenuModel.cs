using System.Reflection;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.Models.Home
{
    public class ItemMenuModel
    {
        public int Id { get; set; }
        public int Role { get; set; }
        public Color BgColor { get; set; }
        public string Icon { get; set; }
        public ImageSource IconMenu => ImageSource.FromResource($"AUTO.HLT.MOBILE.VIP.Resources.Images.{Icon}", typeof(ItemMenuModel).GetTypeInfo().Assembly);
        public string TitleItem { get; set; }
    }
}