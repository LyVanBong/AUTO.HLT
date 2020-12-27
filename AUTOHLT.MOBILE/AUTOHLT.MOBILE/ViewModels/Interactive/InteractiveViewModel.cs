using AUTOHLT.MOBILE.Models.Product;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Interactive
{
    public class InteractiveViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IDatabaseService _databaseService;
        private IProductService _productService;
        private List<ListRegisterProductModel> _regis;
        private List<ProductModel> _productData;

        public List<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public InteractiveViewModel(INavigationService navigationService, IDatabaseService databaseService, IProductService productService) : base(navigationService)
        {
            _productService = productService;
            _databaseService = databaseService;
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
                var user = await _databaseService.GetAccountUser();
                var data = await _productService.GetAllProduct();
                var regisProductData = await _productService.GetListRegisterProductForUser(user.ID);
                if (regisProductData != null && regisProductData.Code > 0 && regisProductData.Data != null &&
                    regisProductData.Data.Any())
                    _regis = new List<ListRegisterProductModel>(regisProductData.Data);
                if (data != null && data.Code > 0 && data.Data.Any())
                {
                    var product = new List<ProductModel>();
                    var lsProduct = data.Data;
                    foreach (var item in lsProduct)
                    {
                        if (item.GroupProduct == "3")
                        {
                            if (item.ProductName == "Tự động thả tim bạn bè")
                            {
                                item.Icon = "icon_heart_auto.png";
                            }
                            else if (item.ProductName == "Tự động chọc bạn bè")
                            {
                                item.Icon = "icon_poke_face.png";
                            }
                            else /*if (item.ProductName == "")*/
                            {
                                item.Icon = "icon_happy_birthday.png";
                            }
                            if (_regis != null && _regis.Any())
                            {
                                var obj = _regis.FirstOrDefault(x => x.ID_ProductType == item.ID);
                                if (obj != null)
                                {
                                    var endDate = DateTime.Parse(obj.DateCreate);
                                    var totalDay = (DateTime.Now - endDate).TotalDays;
                                    var number = double.Parse(item.Number);
                                    if (totalDay <= number)
                                    {
                                        item.IsRegisterProduct = true;
                                        item.BadgeView = "Paid";
                                    }
                                }
                            }
                            product.Add(item);
                        }
                    }

                    ProductData = new List<ProductModel>(product);
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}