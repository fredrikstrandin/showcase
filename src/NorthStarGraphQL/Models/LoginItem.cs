namespace NorthStarGraphQL.Models
{
    public class LoginCreateItem
    {
        public LoginCreateItem(string email, string password, List<ClaimItem> claims)
        {
            Email = email;
            Password = password;
            Claims = claims;
        }
        
        public string Email { get; set; }
        public string Password { get; }
        
        public List<ClaimItem> Claims { get; }
    }
}
