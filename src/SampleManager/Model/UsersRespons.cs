using System;

namespace UserManager.Model;

public class UsersRespons
{
    public UsersRespons(Exception error)
    {
        Error = error;
    }

    public Exception Error { get; }
}
