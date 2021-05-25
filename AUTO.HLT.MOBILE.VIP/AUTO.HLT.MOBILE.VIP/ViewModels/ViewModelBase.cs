using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;

namespace AUTO.HLT.MOBILE.VIP.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible,IApplicationLifecycleAware
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual void OnResume()
        {
            
        }

        public virtual void OnSleep()
        {
        }
    }
}
