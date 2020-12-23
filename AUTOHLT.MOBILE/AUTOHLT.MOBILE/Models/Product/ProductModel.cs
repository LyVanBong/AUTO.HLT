﻿using System.Reflection;
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
        public string TmpEndDate => EndDate == "365" ? Resource._1000055 : Resource._1000054;
        public string ProductContent { get; set; }
        public string Number { get; set; }
        public bool IsRegisterProduct { get; set; }
        public string GroupProduct { get; set; }
        public ImageSource IconLike => ImageSource.FromResource($"AUTOHLT.MOBILE.Resources.Images.icon_like_product.png", typeof(ProductModel).GetTypeInfo().Assembly);
    }
}