using AUTOHLT.MOBILE.CustomRenderer;
using AUTOHLT.MOBILE.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomCollectionViewRenderer), typeof(CollectionViewiOsRenderer))]

namespace AUTOHLT.MOBILE.iOS.CustomRenderer
{
    public class CollectionViewiOsRenderer : CollectionViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Controller.CollectionView.Bounces = false;
            }
        }
    }
}