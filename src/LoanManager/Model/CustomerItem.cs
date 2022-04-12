using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class CustomerItem
    {
        public CustomerItem(string personalNumber, string firstName, string lastName, string zip, string street, int monthlyIncome)
        {
            PersonalNumber = personalNumber;
            FirstName = firstName;
            LastName = lastName;
            Zip = zip;
            Street = street;
            MonthlyIncome = monthlyIncome;
        }

        public string PersonalNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Zip { get; }
        public string Street { get; }
        public int MonthlyIncome { get; }
    }
}
