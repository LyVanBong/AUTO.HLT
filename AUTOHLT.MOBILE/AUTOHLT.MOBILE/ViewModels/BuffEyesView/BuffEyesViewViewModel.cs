using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Controls.Dialog.UseService;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.BuffEyesView
{
    public class BuffEyesViewViewModel : ViewModelBase
    {
        private IProductService _productService;
        private bool _isLoading;
        private ProductModel _like1Year;
        private ProductModel _like1Forever;
        private ProductModel _like2Year;
        private ProductModel _like2Forever;
        private IDatabaseService _databaseService;
        private UserModel _userModel;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private IDialogService _dialogService;

        public ICommand LikeUseServiceCommand { get; private set; }
        /// <summary>
        /// goij like 1 400 thoi han vinh vien
        /// </summary>
        public ProductModel Like2Forever
        {
            get => _like2Forever;
            set => SetProperty(ref _like2Forever, value);
        }

        /// <summary>
        /// goi like 1 400 like thoi han 1 nam
        /// </summary>
        public ProductModel Like2year
        {
            get => _like2Year;
            set => SetProperty(ref _like2Year, value);
        }
        /// <summary>
        /// goij like 1 300 thoi han vinh vien
        /// </summary>
        public ProductModel Like1Forever
        {
            get => _like1Forever;
            set => SetProperty(ref _like1Forever, value);
        }

        /// <summary>
        /// goi like 1 300 like thoi han 1 nam
        /// </summary>
        public ProductModel Like1year
        {
            get => _like1Year;
            set => SetProperty(ref _like1Year, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BuffEyesViewViewModel(INavigationService navigationService, IProductService productService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _productService = productService;
            IsLoading = true;
            LikeUseServiceCommand = new Command<ProductModel>(LikeUseService);
        }

        private async void LikeUseService(ProductModel product)
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
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
                _userModel = await _databaseService.GetAccountUser();
                if (_userModel != null)
                {
                    var lsProduct = await _productService.GetAllProduct();
                    if (lsProduct != null && lsProduct.Code > 0 && lsProduct.Data != null && lsProduct.Data.Any())
                    {
                        Like1year = lsProduct.Data.FirstOrDefault(x => x.ID == "c81cd058-8374-4c3f-9de7-4002d0d46ee0");
                        Like1Forever = lsProduct.Data.FirstOrDefault(x => x.ID == "72235dda-5689-4b88-ac7e-0a82143e45d6");
                        Like2year = lsProduct.Data.FirstOrDefault(x => x.ID == "9ec8a62e-ee56-4c8f-ace1-89bc20296bfa");
                        Like2Forever = lsProduct.Data.FirstOrDefault(x => x.ID == "4f1946ed-913f-431b-97ef-a3c5d5094708");
                    }

                    var lsRegisterProduct = await _productService.GetListRegisterProductForUser(_userModel.ID);
                    if (lsRegisterProduct != null && lsRegisterProduct.Code > 0 && lsRegisterProduct.Data != null && lsRegisterProduct.Data.Any())
                    {
                        foreach (var item in lsRegisterProduct.Data)
                        {
                            switch (item.ID_ProductType)
                            {
                                case "c81cd058-8374-4c3f-9de7-4002d0d46ee0":
                                    var dataEnd = DateTime.Parse(item.DateCreate).Add(TimeSpan.FromDays(365));
                                    var dateNow = DateTime.Now;
                                    if (dateNow < dataEnd)
                                    {
                                        Like1year.IsRegisterProduct = true;
                                        RaisePropertyChanged(nameof(Like1year));
                                    }
                                    break;
                                case "72235dda-5689-4b88-ac7e-0a82143e45d6":
                                    Like1Forever.IsRegisterProduct = true;
                                    RaisePropertyChanged(nameof(Like1Forever));
                                    break;
                                case "9ec8a62e-ee56-4c8f-ace1-89bc20296bfa":
                                    var dataEnd2 = DateTime.Parse(item.DateCreate).Add(TimeSpan.FromDays(365));
                                    var dateNow2 = DateTime.Now;
                                    if (dateNow2 < dataEnd2)
                                    {
                                        Like2year.IsRegisterProduct = true;
                                        RaisePropertyChanged(nameof(Like2year));
                                    }

                                    break;
                                case "4f1946ed-913f-431b-97ef-a3c5d5094708":
                                    Like2Forever.IsRegisterProduct = true;
                                    RaisePropertyChanged(nameof(Like2Forever));
                                    break;
                                default:
                                    break;
                            }
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