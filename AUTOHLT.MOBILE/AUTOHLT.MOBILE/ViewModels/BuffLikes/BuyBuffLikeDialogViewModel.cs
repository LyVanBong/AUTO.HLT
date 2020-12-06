using System;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AUTOHLT.MOBILE.ViewModels.BuffLikes
{
    public class BuyBuffLikeDialogViewModel : BindableBase, IDialogAware
    {
        public BuyBuffLikeDialogViewModel()
        {

        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public event Action<IDialogParameters> RequestClose;
    }
}