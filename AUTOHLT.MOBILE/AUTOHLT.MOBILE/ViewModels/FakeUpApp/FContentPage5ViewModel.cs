using System;
using Prism.Navigation;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class FContentPage5ViewModel:ViewModelBase
    {
        private ContactsInfo contactsInfo;
        public ContactsInfo ContactsInfo
        {
            get { return this.contactsInfo; }
            set { this.contactsInfo = value; }
        }
        public FContentPage5ViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.contactsInfo = new ContactsInfo();
        }
    }
    public class ContactsInfo
    {
        private string firstName;
        private string middleName;
        private string lastName;
        private string contactNo;
        private string email;
        private string address;
        private DateTime? birthDate;
        private string groupName;

        public ContactsInfo()
        {

        }

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.firstName = value;
            }
        }

        public string MiddleName
        {
            get { return this.middleName; }
            set
            {
                this.middleName = value;
            }
        }
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
            }
        }

        public string ContactNumber
        {
            get { return contactNo; }
            set
            {
                this.contactNo = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
            }
        }

        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
            }
        }

        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
            }
        }
    }
}