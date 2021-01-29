using Prism.Mvvm;
using Prism.Regions;

namespace AUTO.HLT.ADMIN.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        private string _title;
        protected IRegionManager RegionManager { get; private set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}