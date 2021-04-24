using System;
using System.ComponentModel;
using System.Diagnostics;
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
            ((Window) sender).Hide();
            Debug.WriteLine($"[{DateTime.Now}]: Dừng chương trình lại nhẽ");
        }
    }
}
