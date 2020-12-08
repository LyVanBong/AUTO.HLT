using System.Windows.Input;
using Plugin.FacebookClient;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Interactive
{
    public class InteractiveViewModel : ViewModelBase
    {
        private bool _isLoading;


        public ICommand InteractiveCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public InteractiveViewModel(INavigationService navigationService) : base(navigationService)
        {
            InteractiveCommand=new Command(Interactive);
            IsLoading = true;
        }

        private async void Interactive()
        {
            await CrossFacebookClient.Current.LoginAsync(new string[] { "email" });
            CrossFacebookClient.Current.OnLogin += (s, a) =>
            {
                switch (a.Status)
                {
                    case FacebookActionStatus.Completed:
                        //Logged in succesfully
                        break;
                }
            };
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }
    }
}