using System.Collections.ObjectModel;
using Prism.Navigation;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.FakeApp
{
    public class FContentPage3ViewModel : ViewModelBase
    {
        public ObservableCollection<Person> Data { get; set; }
        public FContentPage3ViewModel(INavigationService navigationService) : base(navigationService)
        {
            Data = new ObservableCollection<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Steve", Height = 16 },
                new Person { Name = "Steve", Height = 120 },
                new Person { Name = "Steve", Height = 130 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Steve", Height = 140 },
                new Person { Name = "Steve", Height = 150 },
                new Person { Name = "Steve", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Joel", Height = 182 }
            };
        }
    }
    public class Person
    {
        public string Name { get; set; }

        public double Height { get; set; }
    }
}