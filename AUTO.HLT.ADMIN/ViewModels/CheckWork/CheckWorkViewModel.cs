using AUTO.HLT.ADMIN.Databases;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AUTO.HLT.ADMIN.ViewModels.CheckWork
{
    public class CheckWorkViewModel : ViewModelBase
    {
        private bsoft_autohltEntities _dbAdminEntities;
        private ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result> _dataHistoryAuto;
        private string _searchHistory;


        public ICommand SearchHistoryCommand { get; private set; }
        private List<GetAllHistoryAutoLikeCommentAvatar_Result> _datAllHistoryAutoLikeCommentAvatarResults;
        public string SearchHistory
        {
            get => _searchHistory;
            set => _searchHistory = value;
        }

        public ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result> DataHistoryAuto
        {
            get => _dataHistoryAuto;
            set => SetProperty(ref _dataHistoryAuto, value);
        }

        public CheckWorkViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _dbAdminEntities = new bsoft_autohltEntities();
            SearchHistoryCommand = new DelegateCommand(SearchHistoryAuto);
        }

        private void SearchHistoryAuto()
        {
            if (SearchHistory != null)
            {
                DataHistoryAuto =
                    new ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result>(
                        _datAllHistoryAutoLikeCommentAvatarResults.Where(x => x.ID == SearchHistory));
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _datAllHistoryAutoLikeCommentAvatarResults = _dbAdminEntities.GetAllHistoryAutoLikeCommentAvatar()?.ToList();
            if (_datAllHistoryAutoLikeCommentAvatarResults != null && _datAllHistoryAutoLikeCommentAvatarResults.Any())
            {
                DataHistoryAuto = new ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result>(_datAllHistoryAutoLikeCommentAvatarResults);
            }
        }
    }
}