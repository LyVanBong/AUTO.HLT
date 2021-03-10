using Prism.Navigation;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Manage
{
    public class ManageViewModel : ViewModelBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ManageViewModel(INavigationService navigationService) : base(navigationService)
        {
            IsLoading = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }
    }
}