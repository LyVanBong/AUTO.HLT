using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.ViewModels;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.BuffService
{
    public class BuffDialogViewModel : ViewModelBase, IDialogAware
    {
        private string _number;
        private string _price;
        private string _content;
        private string _amount;
        private bool _isEnabledLogin;
        private string _textSubmit;


        public string TextSubmit
        {
            get => _textSubmit;
            set => SetProperty(ref _textSubmit, value);
        }

        public bool IsEnabledLogin
        {
            get => _isEnabledLogin;
            set => SetProperty(ref _isEnabledLogin, value);
        }

        public ICommand UseServiceCommand { get; private set; }
        public string Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public BuffDialogViewModel(INavigationService navigationService) : base(navigationService)
        {
            UseServiceCommand = new Command(UseService);
        }

        private void UseService()
        {

        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                var para = parameters.GetValue<string>("ServiceName");
                TextSubmit = para;

                Title = Resource._1000058 + " " + TextSubmit;
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}