using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class ContentViewModel : ViewModelBase
    {
        private string _uriWebApp;
        private ObservableCollection<ProductModel> _productData;
        private IDatabaseService _databaseService;
        private IProductService _productService;
        private IPageDialogService _pageDialogService;

        public ICommand UseServiceCommand { get; private set; }
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

        public ContentViewModel(INavigationService navigationService, IProductService productService, IDatabaseService databaseService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _productService = productService;
            UseServiceCommand = new Command<ProductModel>(UseService);
        }

        private async void UseService(ProductModel obj)
        {
            try
            {
                await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                    $"Bạn đã hoàn thành {obj.EndDate} ngày làm nhiệm vụ {Title}, điểm thưởng sẽ được thêm vào tài khoản vào lúc {DateTime.Now.ToString("F")} chúc bạn một ngày làm việc tốt lành !!!",
                    "OK");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
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

                    var random = new Random();
                    ProductData = new ObservableCollection<ProductModel>(product.Where(x => x.GroupProduct == $"{random.Next(1, 9)}"));
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}