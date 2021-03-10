using System.Collections.ObjectModel;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using Prism.Navigation;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Manage
{
    public class ManageViewModel : ViewModelBase
    {
        private bool _isLoading;
        private ILicenseKeyService _licenseKeyService;
        private IDatabaseService _databaseService;
        private ObservableCollection<AgecyLicenseModel> _lsMyLicense;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<AgecyLicenseModel> LsMyLicense
        {
            get => _lsMyLicense;
            set => SetProperty(ref _lsMyLicense, value);
        }

        public ManageViewModel(INavigationService navigationService, ILicenseKeyService licenseKeyService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _licenseKeyService = licenseKeyService;
            IsLoading = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var data = await _licenseKeyService.GetLicenseForAgecy();
            if (data != null && data.Code>0&& data.Data != null)
            {
                LsMyLicense = new ObservableCollection<AgecyLicenseModel>(data.Data);
            }
            IsLoading = false;
        }
    }
}