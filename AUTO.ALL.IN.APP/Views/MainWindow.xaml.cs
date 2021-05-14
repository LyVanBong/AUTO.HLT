using AUTO.ALL.IN.APP.ViewModels;
using CommunityToolkit.WinUI.Notifications;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace AUTO.ALL.IN.APP.Views
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
            Debug.WriteLine($"[{DateTime.Now}]: Bắt đầu dừng");
            ((Window)sender).Hide();
            var vm = DataContext as MainWindowViewModel;
            if (vm != null)
                vm.UpdateDatabase();
            Thread.Sleep(TimeSpan.FromMinutes(1));
            Debug.WriteLine($"[{DateTime.Now}]: Dừng chương trình");
        }
        /// <summary>
        /// notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new ToastContentBuilder().AddText("Thong bao").AddText("Đây là thông báo").Show();
        }
    }
}
