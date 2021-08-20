using AUTO.DLL.MOBILE.Models.Facebook;
using AUTO.DLL.MOBILE.Services.Facebook;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Resources.Languages;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTOHLT.MOBILE.ViewModels.Pokes
{
    public class PokesViewModel : ViewModelBase
    {
        private bool _isLoading = true;
        private ObservableRangeCollection<PokesFriendsModel> _pokesData;
        private IFacebookService _facebook = new FacebookeService();
        private IDialogService _dialogService;
        private IPageDialogService _pageDialogService;
        private bool _isCheckPoke;

        public ICommand PokesFriendCommand { get; private set; }
        public ICommand SelectAllFriendsCommand { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableRangeCollection<PokesFriendsModel> PokesData
        {
            get => _pokesData;
            set => SetProperty(ref _pokesData, value);
        }

        public PokesViewModel(INavigationService navigationService, IDialogService dialogService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _dialogService = dialogService;
            PokesFriendCommand = new AsyncCommand<object>(async (friend) => await PokesFriend(friend));
            SelectAllFriendsCommand = new AsyncCommand(async () => await SelectAllFriends());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                await InitData();
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

        private async Task InitData()
        {
            var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
            if (!string.IsNullOrEmpty(cookie))
            {
                var getPoke = await _facebook.GetFriendPoke(cookie);
                if (getPoke != null && getPoke.Any())
                {
                    PokesData = new ObservableRangeCollection<PokesFriendsModel>(getPoke);
                }
            }
            else
            {
                var result = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                    "Bạn cần kết nối với facebook của mình để sử dụng tinh năng này !", "OK", "Cancel");
                if (result)
                {
                    _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, async (res) =>
                    {
                        var lspoke =
                            await _facebook.GetFriendPoke(Preferences.Get(AppConstants.CookieFacebook, ""));
                        if (lspoke != null && lspoke.Any())
                        {
                            PokesData = new ObservableRangeCollection<PokesFriendsModel>(lspoke);
                        }
                    });
                }
            }
        }

        private async Task SelectAllFriends()
        {
            if (PokesData != null && PokesData.Any())
            {
                _isCheckPoke = !_isCheckPoke;
                foreach (var friend in PokesData)
                {
                    friend.IsPokes = _isCheckPoke;
                }
            }
        }

        private async Task PokesFriend(object obj)
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                if (obj is PokesFriendsModel)
                {
                    var friend = obj as PokesFriendsModel;
                    var isPoke = await _facebook.PokeFriend(Preferences.Get(AppConstants.CookieFacebook, ""), friend);
                    if (isPoke)
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo",
                            "Chọc bạn " + friend.FullName + " thành công !", "OK");
                        PokesData.Remove(friend);
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo",
                            "Chọc bạn " + friend.FullName + " chưa thành công !", "OK");
                    }
                }
                else
                {
                    if (PokesData != null && PokesData.Any())
                    {
                        var lsPoke = PokesData.Where(x => x.IsPokes).ToList();
                        if (lsPoke.Any())
                        {
                            var numPoke = 0;
                            foreach (var fr in lsPoke)
                            {
                                var isPoke = await _facebook.PokeFriend(Preferences.Get(AppConstants.CookieFacebook, ""), fr);
                                if (isPoke)
                                {
                                    numPoke++;
                                    PokesData.Remove(fr);
                                }
                            }

                            if (numPoke > 0)
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo",
                                    "Đã chọc " + numPoke + " bạn bè thành công", "OK");
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo",
                                    "Lỗi vui lòng thực hiện lại", "OK");
                            }
                        }
                    }
                    else
                    {
                        await InitData();
                    }
                }
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
    }
}