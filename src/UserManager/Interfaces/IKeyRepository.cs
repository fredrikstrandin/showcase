using System.Collections.Generic;

namespace UserManager.Interfaces
{
    public interface IKeyRepository
    {
        IEnumerable<string> LoadEmails();
    }
}