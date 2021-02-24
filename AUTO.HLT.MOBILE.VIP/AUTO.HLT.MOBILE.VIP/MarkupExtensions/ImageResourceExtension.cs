using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTO.HLT.MOBILE.VIP.MarkupExtensions
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            return ImageSource.FromResource("AUTO.HLT.MOBILE.VIP.Resources.Images." + Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }
    }
}