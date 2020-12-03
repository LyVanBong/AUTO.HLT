using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationView : ContentView
    {
        public NavigationView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextBindableProperty = BindableProperty.Create(nameof(Text), typeof(string), declaringType: typeof(NavigationView), defaultValue: null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: SetValueText);

        public string Text
        {
            get => (string)GetValue(TextBindableProperty);
            set => SetValue(TextBindableProperty, value);
        }

        private static void SetValueText(BindableObject bindable, object oldValue, object newValue)
        {
            var myText = (NavigationView)bindable;
            myText.TextTitle.Text = newValue.ToString();
        }
    }
}