using System.ComponentModel;
using System.Windows;
using AUTO.HLT.ADMIN.ViewModels.Main;

namespace AUTO.HLT.ADMIN.Views.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
          
        }
    }
}