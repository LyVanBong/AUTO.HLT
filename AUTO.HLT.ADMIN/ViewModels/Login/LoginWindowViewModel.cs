using AUTO.HLT.ADMIN.Views.Login;
using AUTO.HLT.ADMIN.Views.Main;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace AUTO.HLT.ADMIN.ViewModels.Login
{
    public class LoginWindowViewModel : BindableBase
    {
        public ICommand LoginCommand { get; private set; }

        public LoginWindowViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            var main = new MainWindow();
            main.Show();
            LoginWindow.Login.Close();
        }
    }
}