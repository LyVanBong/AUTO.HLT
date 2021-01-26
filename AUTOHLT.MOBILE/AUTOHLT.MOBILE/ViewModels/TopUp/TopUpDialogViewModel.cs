using System;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.TopUp
{
    public class TopUpDialogViewModel : BindableBase, IDialogAware
    {
        public ICommand ClosePopupCommand { get; set; }
        public bool CanCloseDialog() => true;

        public TopUpDialogViewModel()
        {
            ClosePopupCommand = new Command(ClosePopup);
        }

        private void ClosePopup()
        {
            if (RequestClose != null)
            {
                RequestClose(null);
            }
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        public event Action<IDialogParameters> RequestClose;
    }
}