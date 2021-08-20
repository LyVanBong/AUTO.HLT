using System.Windows.Controls;

namespace AUTO.ALL.IN.APP.Views
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : UserControl
    {
        public AddUserView()
        {
            InitializeComponent();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!int.TryParse(textBox.Text, out var result))
            {
                textBox.Text = 5 + "";
            }
        }
    }
}