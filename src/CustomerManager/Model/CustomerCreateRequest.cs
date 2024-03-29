﻿namespace CustomerManager.Model
{
    public class CustomerCreateRequest
    {
        public CustomerCreateRequest(string id, string firstName, string lastName, string email, string street, string zip, string city, int monthlyIncome)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Street = street;
            Zip = zip;
            City = city;
            MonthlyIncome = monthlyIncome;
        }

        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        //Kommer användas som kontakt med kunden och inloggning
        public string Email { get; }
        //Så att vi kan skapa en användare och användaren kan följa sina lån
        public string Street { get; }
        public string Zip { get; }
        public string City { get; }
        //Antar att hela kronor räcker.
        public int MonthlyIncome { get; }
    }
}
