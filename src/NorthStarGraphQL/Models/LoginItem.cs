namespace NorthStarGraphQL.Models
{
    public class LoginCreateItem
    {
        public LoginCreateItem(string email, string nickname, string password, List<ClaimItem> claims)
        {
            Email = email;
            Nickname = nickname;
            Password = password;
            Claims = claims;
        }
        
        public string Email { get; set; }
        public string Nickname { get; }
        public string Password { get; }
        
        public List<ClaimItem> Claims { get; }
    }
}
