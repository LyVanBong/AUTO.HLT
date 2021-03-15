using Prism.Mvvm;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
        }
    }
}