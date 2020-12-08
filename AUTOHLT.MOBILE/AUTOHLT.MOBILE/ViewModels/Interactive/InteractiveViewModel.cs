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
                //await CrossFacebookClient.Current.LoginAsync(new string[] { "email" });

                var fb = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday" }, new string[] { "email", "user_birthday" });
           var test=     await CrossFacebookClient.Current.QueryDataAsync("me/friends", new string[] { "user_friends" }, new Dictionary<string, string>()
                {
                    {"fields", "id, first_name, last_name, middle_name, name, email, picture"}
                });

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