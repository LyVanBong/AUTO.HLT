using System;
using System.Reflection;
using AUTOHLT.MOBILE.Resources.Languages;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Models.Product
{
    public class ProductModel
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string DateCreate { get; set; }
        public string EndDate { get; set; }
        public string ProductContent { get; set; }
        public string Number { get; set; }
        public string GroupProduct { get; set; }

        #region Extend

        public bool IsRegisterProduct { get; set; }
        public ImageSource IconLike => ImageSource.FromResource($"AUTOHLT.MOBILE.Resources.Images.{Icon}", typeof(ProductModel).GetTypeInfo().Assembly);
        public string Icon { get; set; }

        public string TitleProduct =>
            $"Buff {Number} like / {Resource._1000088} {Resource._1000089} {EndDate} {Resource._1000088}";
        public string ColorBg => IsRegisterProduct ? "#d0e8f2" : "#f1f1f1";
        public string BadgeView { get; set; }

        #endregion
    }
}