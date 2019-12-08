using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext _context;

    public AuthRepository(DataContext context) {
      _context = context;
    }

    private (byte[], byte[]) CreatePasswordHash(string password)
    {
      using var hmac = new HMACSHA512();
      byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      byte[] passwordSalt = hmac.Key;
      return (passwordHash, passwordSalt);
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using var hmac = new HMACSHA512(passwordSalt);
      byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      return computedHash.SequenceEqual(passwordHash);
    }

    public async Task<User> Login(string username, string password)
    {
      var user = await _context.Users.FirstOrDefaultAsync(
        x => x.Username == username
      );
      if (user == null)
        return null;
      if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        return null;

      return user;
    }

    public async Task<User> Register(User user, string password)
    {
      (byte[] passwordHash, byte[] passwordSalt) = CreatePasswordHash(password);

      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;

      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<bool> UserExists(string username)
    {
      if (await _context.Users.AnyAsync(x => x.Username == username))
        return true;

      return false;
    }
  }
}
