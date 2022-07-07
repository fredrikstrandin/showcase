using System;

namespace CustomerManager.Model;

public class CustomerRespons
{
    public CustomerRespons(string id, Exception error)
    {
        Id = id;
        Error = error;
    }

    public string Id { get; }
    public Exception Error { get; }
}
