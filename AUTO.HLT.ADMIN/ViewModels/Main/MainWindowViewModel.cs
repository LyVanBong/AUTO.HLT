using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Models.Main;
using Prism.Commands;
using Prism.Mvvm;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private List<UserModel> _dataUsers;
        private int _stt;
        private string _id;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public List<UserModel> DataUsers
        {
            get => _dataUsers;
            set => SetProperty(ref _dataUsers, value);
        }

        public ICommand SaveAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }

        public int STT
        {
            get => _stt;
            set => SetProperty(ref _stt,value);
        }

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public MainWindowViewModel()
        {
            SaveAccountCommand = new DelegateCommand(SaveAccount);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
        }

        private void DeleteAccount()
        {
            MessageBox.Show("test 1");
        }

        private void SaveAccount()
        {
            MessageBox.Show("test 2");
        }
    }
}