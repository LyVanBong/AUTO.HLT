using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Services.Product;

namespace AUTOHLT.MOBILE.ViewModels.BuffLikes
{
    public class BuffLikeViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IProductService _productService;
        private ObservableCollection<ProductModel> _productData;

        public ObservableCollection<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BuffLikeViewModel(INavigationService navigationService, IProductService productService) : base(navigationService)
        {
            _productService = productService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializeData();
            IsLoading = false;
        }

        private async Task InitializeData()
        {
            try
            {
                var data = await _productService.GetAllProduct();
                if (data != null && data.Code > 0 && data.Data.Any())
                {
                    ProductData = new ObservableCollection<ProductModel>();
                    var lsProduct = data.Data;
                    foreach (var item in lsProduct)
                    {
                        if (item.GroupProduct == "1")
                        {
                            item.Icon = "icon_like_product.png";
                            ProductData.Add(item);
                            await Task.Delay(TimeSpan.FromMilliseconds(50));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}