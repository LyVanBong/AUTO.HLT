using System.Reflection;
using AUTOHLT.MOBILE.Resources.Languages;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Models.Product
{
    public class BuffLikeModel : ProductModel
    {
        public bool IsRegisterProduct { get; set; }
        public ImageSource IconLike => ImageSource.FromResource($"AUTOHLT.MOBILE.Resources.Images.{Icon}", typeof(ProductModel).GetTypeInfo().Assembly);
        public string Icon { get; set; }

        public string TitleProduct =>
            $"{ProductName} {Number} like / {Resource._1000088} {Resource._1000089} {EndDate} {Resource._1000088}";
        public string ColorBg => IsRegisterProduct ? "#d0e8f2" : "#f1f1f1";
    }
}