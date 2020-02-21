using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;
using DatingApp.API.Helpers;

namespace DatingApp.API.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext _context;

    public AuthRepository(DataContext context) {
      _context = context;
    }

    public async Task<User> Login(string username, string password)
    {
      var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(
        x => x.Username == username
      );
      if (user == null)
        return null;
      if (!Passwords.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        return null;

      return user;
    }

    public async Task<User> Register(User user, string password)
    {
      (byte[] passwordHash, byte[] passwordSalt) = Passwords.CreatePasswordHash(password);

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
