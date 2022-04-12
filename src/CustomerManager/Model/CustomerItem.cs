using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManager.Model
{
    public class CustomerItem
    {
        public CustomerItem(string id, string personalNumber, string firstName, string lastName, string email, string street, string zip, string city, int monthlyIncome)
        {
            Id = id;
            PersonalNumber = personalNumber;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Street = street;
            Zip = zip;
            City = city;
            MonthlyIncome = monthlyIncome;
        }

        public string Id { get; }
        public string PersonalNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Adress { get { return $"{Street}\n{Zip} {City}"; } }
        public string Street { get; }
        public string Zip { get; }
        public string City { get; }
        public int? MonthlyIncome { get; }
    }
}
