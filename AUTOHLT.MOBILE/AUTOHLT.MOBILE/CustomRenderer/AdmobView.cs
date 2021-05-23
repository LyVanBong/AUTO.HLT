using Xamarin.Forms;

namespace AUTOHLT.MOBILE.CustomRenderer
{
    public class AdmobView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create("AdsUnitId", typeof(string), typeof(AdmobView), string.Empty);

        public string AdsUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }
    }
}