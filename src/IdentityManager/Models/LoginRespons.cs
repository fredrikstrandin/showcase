namespace IdentityManager.Models
{
    public class LoginRespons
    {
        public LoginRespons(string id, Exception error)
        {
            Id = id;
            Error = error;
        }

        public string Id { get; }
        public Exception Error { get; }
    }
}
