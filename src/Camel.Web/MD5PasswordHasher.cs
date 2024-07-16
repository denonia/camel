using System.Security.Cryptography;
using System.Text;
using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Camel.Web;

public class MD5PasswordHasher : IPasswordHasher<User>
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    
    public string HashPassword(User user, string password)
    {
        var md5Hash = MD5.HashData(Encoding.UTF8.GetBytes(password));
        var md5Str = Convert.ToHexString(md5Hash).ToLower();
        
        return _passwordHasher.HashPassword(user, md5Str);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var md5Hash = MD5.HashData(Encoding.UTF8.GetBytes(providedPassword));
        var md5Str = Convert.ToHexString(md5Hash).ToLower();
        
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, md5Str);
    }
}