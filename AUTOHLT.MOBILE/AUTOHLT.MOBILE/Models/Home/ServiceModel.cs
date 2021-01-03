using System.Reflection;
using Syncfusion.XForms.BadgeView;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Models.Home
{
    public class ServiceModel
    {
        public string Icon { get; set; }
        public ImageSource IconService => ImageSource.FromResource($"AUTOHLT.MOBILE.Resources.Images.{Icon}", typeof(ServiceModel).GetTypeInfo().Assembly);
        public string TitleService { get; set; }
        public int TypeService { get; set; }
        public string BadgeView { get; set; }
        public string UserRole { get; set; }
        public BadgeType BadgeType { get; set; }
    }
}