using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.FakeModules.Views.HelperUs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTO.HLT.MOBILE.VIP.FakeModules.ViewModels.Main
{
    public class FMainViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialogService;
        private IDatabaseService _databaseService;
        public ICommand FeatureCommand { get; private set; }
        public FMainViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            FeatureCommand = new AsyncCommand<string>(async (key) => await FeatureApp(key));
        }

        private async Task FeatureApp(string key)
        {
            try
            {
                switch (key)
                {
                    case "0":
                        break;
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        await NavigationService.NavigateAsync("/FAboutUsPage");
                        break;
                    case "5":
                        await NavigationService.NavigateAsync("/TermsConditionPage");
                        break;
                    case "6":
                        await NavigationService.NavigateAsync("/InviteFriendPage");
                        break;
                    case "7":
                        await NavigationService.NavigateAsync("/HelperUsPage");
                        break;
                    case "8":
                        await _pageDialogService.DisplayAlertAsync("Thông báo",
                            "Ứng dụng đang sử dụng ngôn ngữ tiếng việt", "OK");
                        break;
                    case "9":
                        if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Bạn muốn đăng xuất tài khoản",
                            "Ok", "Cancel"))
                        {
                            await _databaseService.DeleteAccontUser();
                            Preferences.Clear(AppConstants.SavePasswd);
                            await NavigationService.NavigateAsync("/LoginPage", null, false, true);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}