using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.DataContexts;
using UserManager.Interfaces;

namespace UserManager.Services;

public class KeyService : IKeyService
{
    private readonly IKeyRepository _keyRepository;
    private HashSet<string> _emails = new HashSet<string>();

    public KeyService(IKeyRepository keyRepository)
    {
        _keyRepository = keyRepository;
    }

    public void Load()
    {
        foreach (var item in _keyRepository.LoadEmails())
        {
            _emails.Add(item);
        }
    }
    public bool ContainsEmail(string email)
    {
        return _emails.Contains(email);
    }

    public bool AddEmail(string email)
    {
        return _emails.Add(email);
    }
}
