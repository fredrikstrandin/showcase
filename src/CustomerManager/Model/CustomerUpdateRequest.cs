namespace CustomerManager.Model
{
    public class UserUpdateRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Adress { get { return $"{Street}\n{Zip} {City}"; } }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
    }

}
