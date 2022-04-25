namespace CustomerManager.Model
{
    public class CustomerCreateRequest
    {
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //Kommer användas som kontakt med kunden och inloggning
        public string Email { get; set; }
        //Så att vi kan skapa en användare och användaren kan följa sina lån
        public string Password { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        //Antar att hela kronor räcker.
        public int MonthlyIncome { get; set; }
    }
}
