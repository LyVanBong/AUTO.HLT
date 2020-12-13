using AUTOHLT.MOBILE.Resources.Languages;
using System;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.MarkupExtensions
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension
    {
        private static readonly string _resourceId = typeof(Resource).FullName;

        private static readonly Lazy<ResourceManager> _resourceManager =
            new Lazy<ResourceManager>(() =>
                new ResourceManager(_resourceId, typeof(TranslateExtension)
                    .GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text == null ? null : _resourceManager.Value.GetString(Text.ToUpper());
        }
    }
}