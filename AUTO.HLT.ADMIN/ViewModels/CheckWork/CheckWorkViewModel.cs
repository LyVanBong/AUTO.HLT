using AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Services.AutoLikeCommentAvatar;

namespace AUTO.HLT.ADMIN.ViewModels.CheckWork
{
    public class CheckWorkViewModel : ViewModelBase
    {
        private ObservableCollection<HistoryAutoModel> _dataHistoryAuto;
        private string _searchHistory;


        public ICommand SearchHistoryCommand { get; private set; }
        private List<HistoryAutoModel> _datAllHistoryAutoLikeCommentAvatarResults;
        public string SearchHistory
        {
            get => _searchHistory;
            set => _searchHistory = value;
        }

        public ObservableCollection<HistoryAutoModel> DataHistoryAuto
        {
            get => _dataHistoryAuto;
            set => SetProperty(ref _dataHistoryAuto, value);
        }
        private IAutoAvatarService _autoAvatarService;
        public CheckWorkViewModel(IRegionManager regionManager, IAutoAvatarService autoAvatarService) : base(regionManager)
        {
            _autoAvatarService = autoAvatarService;
            SearchHistoryCommand = new DelegateCommand(SearchHistoryAuto);
        }

        private void SearchHistoryAuto()
        {
            if (SearchHistory != null)
            {
                DataHistoryAuto =
                    new ObservableCollection<HistoryAutoModel>(
                        _datAllHistoryAutoLikeCommentAvatarResults.Where(x => x.ID == SearchHistory));
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var data = await _autoAvatarService.GetAllHistoryAuto();
            if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
            {
                _datAllHistoryAutoLikeCommentAvatarResults = data.Data;
                DataHistoryAuto = new ObservableCollection<HistoryAutoModel>(_datAllHistoryAutoLikeCommentAvatarResults);
            }
        }
    }
}