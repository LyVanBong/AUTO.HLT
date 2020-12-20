﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Models.Home
{
   public class ServiceModel
    {
        public string Icon { get; set; }
        public ImageSource IconService => ImageSource.FromResource($"AUTOHLT.MOBILE.Resources.Images.{Icon}", typeof(ServiceModel).GetTypeInfo().Assembly);
        public string TitleService { get; set; }
        public int TypeService { get; set; }
    }
}
