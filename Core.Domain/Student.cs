using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Student : Person
    {
        private DateTime _birthDay;
        public DateTime Birthday
        {
            get => _birthDay;
            set
            {
                var birthdayPlus16Years = _birthDay.AddYears(16);
                if (birthdayPlus16Years.Date <= DateTime.Now.Date)
                    _birthDay = value;
                else if (_birthDay.Date >= DateTime.Now.Date)
                    throw new InvalidOperationException("De verjaardag mag niet in de toekomst liggen");
                else
                    throw new InvalidOperationException("De student moet minimaal 16 jaar zijn");
            }
        }
        public String PhoneNumber { get; set; }
        public City City { get; set; }
        public List<Package> Packages { get; set; }
    }
}