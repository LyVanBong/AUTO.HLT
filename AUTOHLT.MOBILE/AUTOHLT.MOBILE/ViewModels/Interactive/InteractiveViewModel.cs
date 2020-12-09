using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Plugin.FacebookClient;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Interactive
{
    public class InteractiveViewModel : ViewModelBase
    {
        private bool _isLoading;

        private string _tokenFacebook;

        public string TokenFacebook
        {
            get => _tokenFacebook;
            set => SetProperty(ref _tokenFacebook, value);
        }
        public ICommand InteractiveCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public InteractiveViewModel(INavigationService navigationService) : base(navigationService)
        {
            InteractiveCommand = new Command(Interactive);
            IsLoading = true;
        }

        private async void Interactive()
        {
            try
            {
                await CrossFacebookClient.Current.LoginAsync(new string[] { "email" });
                CrossFacebookClient.Current.OnLogin += (s, a) =>
                {
                    switch (a.Status)
                    {
                        case FacebookActionStatus.Completed:
                            TokenFacebook = CrossFacebookClient.Current.ActiveToken + "\n" +
                                            CrossFacebookClient.Current.ActiveUserId + "\n" +
                                            CrossFacebookClient.Current.TokenExpirationDate + "\n" +
                                            CrossFacebookClient.Current.IsLoggedIn;
                            break;
                    }
                };
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }
    }
}