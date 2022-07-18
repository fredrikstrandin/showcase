using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using UserManager.Interfaces;

namespace UserManager.Services
{
    public class PasswordService : IPasswordService
    {
        public byte[] GenerateSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public string CreateHash(string password, byte[] salt, int numberOfRounds = 100041)
        {
            // derive a 512-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: numberOfRounds,
                numBytesRequested: 512 / 8));
        }

        public bool CompareHash(string password, string hash, byte[] salt)
        {
            string compareHashe = CreateHash(password, salt);
            return hash == compareHashe;
        }
    }
}
