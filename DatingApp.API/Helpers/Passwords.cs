using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.API.Helpers
{
    public static class Passwords
    {
      public static (byte[], byte[]) CreatePasswordHash(string password)
      {
        using var hmac = new HMACSHA512();
        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        byte[] passwordSalt = hmac.Key;
        return (passwordHash, passwordSalt);
      }

      public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
      {
        using var hmac = new HMACSHA512(passwordSalt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
      }
    }
}
