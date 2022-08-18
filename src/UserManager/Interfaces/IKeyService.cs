using System.Threading.Tasks;

namespace UserManager.Interfaces
{
    public interface IKeyService
    {
        bool AddEmail(string email);
        bool ContainsEmail(string email);
        void Load();
    }
}