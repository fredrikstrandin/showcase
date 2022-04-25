namespace CustomerManager.Model
{
    public class CustomerCreateItem : CustomerItem
    {
        public CustomerCreateItem(string personalNumber, string firstName, string lastName, string email, string hash, byte[] salt, string street, string zip, string city, int monthlyIncome)
            : base(null, personalNumber, firstName, lastName, email, street, zip, city, monthlyIncome)
        {
            Hash = hash;
            Salt = salt;
        }

        public string Hash { get; }
        public byte[] Salt { get; }
    }
}
