using System;

namespace UserManager.Model;

public class UserRespons
{
    public UserRespons(string id, Exception error)
    {
        Id = id;
        Error = error;
    }

    public string Id { get; }
    public Exception Error { get; }
}
