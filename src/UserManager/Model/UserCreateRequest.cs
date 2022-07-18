namespace UserManager.Model
{
    public class UserCreateRequest
    {
        public UserCreateRequest(string id, string firstName, string lastName, string email, string street, string zip, string city)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Street = street;
            Zip = zip;
            City = city;
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
    }
}
