using AUTOHLT.MOBILE.FakeModules.Views;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.ViewModels;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.FakeModules.ViewModels
{
    public class FProfileViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialogService;
        private IDatabaseService _databaseService;
        public ICommand GoToFeatureCommand { get; private set; }
        public string VersionApp => AppInfo.VersionString + " (" + AppInfo.BuildString + ")";

        public FProfileViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            GoToFeatureCommand = new AsyncCommand<string>(async (key) => await GoToFeature(key));
        }

        private async Task GoToFeature(string key)
        {
            var num = int.Parse(key);
            switch (num)
            {
                case 0:
                    await NavigationService.NavigateAsync(nameof(AccountInformationPage));
                    break;

                case 1:
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;

                case 2:
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Ứng AUTOHLT đang sử dụng ngôn ngữ tiếng việt", "OK");
                    break;

                case 3:
                    await NavigationService.NavigateAsync(nameof(FIntroducePage));
                    break;

                case 4:
                    var urlAppInStore = Device.RuntimePlatform == Device.Android ? @"https://play.google.com/store/apps/details?id=com.bsoftgroup.auto.vip" : @"https://apps.apple.com/vn/app/autovip/id1557805046";
                    await Launcher.TryOpenAsync(urlAppInStore);
                    break;

                case 5:
                    var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000042, "OK", "Cancel");
                    if (res)
                    {
                        Preferences.Clear();
                        await _databaseService.DeleteAccontUser();
                        await NavigationService.NavigateAsync("/LoginPage");
                    }
                    break;

                default:
                    break;
            }
        }
    }
}