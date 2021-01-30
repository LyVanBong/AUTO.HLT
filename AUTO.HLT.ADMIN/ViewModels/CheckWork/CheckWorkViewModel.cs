using AUTO.HLT.ADMIN.Databases;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace AUTO.HLT.ADMIN.ViewModels.CheckWork
{
    public class CheckWorkViewModel : ViewModelBase
    {
        private bsoft_autohltEntities _dbAdminEntities;
        private ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result> _dataHistoryAuto;

        public ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result> DataHistoryAuto
        {
            get => _dataHistoryAuto;
            set => SetProperty(ref _dataHistoryAuto, value);
        }

        public CheckWorkViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _dbAdminEntities = new bsoft_autohltEntities();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var data = _dbAdminEntities.GetAllHistoryAutoLikeCommentAvatar().ToList();
            DataHistoryAuto = new ObservableCollection<GetAllHistoryAutoLikeCommentAvatar_Result>(data);
        }
    }
}