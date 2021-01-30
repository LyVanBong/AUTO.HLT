using System.Windows;

namespace AUTO.HLT.ADMIN.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static LoginWindow Login { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            Login = this;
        }
    }
}