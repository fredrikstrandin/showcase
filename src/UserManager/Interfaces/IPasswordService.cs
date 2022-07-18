namespace UserManager.Interfaces
{
    public interface IPasswordService
    {
        bool CompareHash(string password, string hash, byte[] salt);
        string CreateHash(string password, byte[] salt, int numberOfRounds = 100041);
        byte[] GenerateSalt();
    }
}