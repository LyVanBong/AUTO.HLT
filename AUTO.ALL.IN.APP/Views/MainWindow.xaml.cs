using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.WinUI.Notifications;

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
            ((Window)sender).Hide();
            Debug.WriteLine($"[{DateTime.Now}]: Dừng chương trình lại nhẽ");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new ToastContentBuilder().AddText("Thong bao").AddText("Đây là thông báo").Show();
        }
    }
}
