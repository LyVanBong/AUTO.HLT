using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class ContentViewModel : ViewModelBase
    {
        private string _uriWebApp;
        private ObservableCollection<ProductModel> _productData;
        private IDatabaseService _databaseService;
        private IProductService _productService;

        public ObservableCollection<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public string UriWebApp
        {
            get => _uriWebApp;
            set => SetProperty(ref _uriWebApp, value);
        }

        public ContentViewModel(INavigationService navigationService, IProductService productService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _productService = productService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                Title = parameters["Title"].ToString();
                UriWebApp = parameters["Uri"].ToString();
            }

            await InitializeData();
        }
        private async Task InitializeData()
        {
            try
            {
                var data = await _productService.GetAllProduct();
                if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                {
                    var product = new List<ProductModel>();
                    var lsProduct = data.Data.ToList();
                    foreach (var item in lsProduct)
                    {
                       
                            item.TitleProduct = $"Buff {item.Number} {Title} / {Resource._1000088} {Resource._1000089} {item.EndDate} {Resource._1000088}";
                            item.Icon = UriWebApp;
                            product.Add(item);
                        
                    }

                    ProductData = new ObservableCollection<ProductModel>(product);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}