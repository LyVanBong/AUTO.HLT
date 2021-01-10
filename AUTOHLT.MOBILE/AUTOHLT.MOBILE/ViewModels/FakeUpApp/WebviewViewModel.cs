using Prism.Navigation;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class WebviewViewModel : ViewModelBase
    {
        private string _uriWebApp;

        public string UriWebApp
        {
            get => _uriWebApp;
            set => SetProperty(ref _uriWebApp, value);
        }

        public WebviewViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                Title = parameters["Title"].ToString();
                UriWebApp = parameters["Uri"].ToString();
            }
        }
    }
}