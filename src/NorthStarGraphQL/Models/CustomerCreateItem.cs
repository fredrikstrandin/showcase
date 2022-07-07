namespace NorthStarGraphQL.Models;

public class CustomerCreateItem
{
    public CustomerCreateItem(string id, string firstName, string lastName, string email, string street, string zip, string city, int monthlyIncome)
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
    public string Email { get; }
    public string Street { get; }
    public string Zip { get; }
    public string City { get; }
    public int? MonthlyIncome { get; }
}
