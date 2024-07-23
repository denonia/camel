using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (User?, PasswordVerificationResult) AuthenticateUser(string userName, string passwordMd5)
    {
        var user = _dbContext.Users
            .SingleOrDefault(u => u.UserName == userName);
        var hasher = new PasswordHasher<User>();

        if (user == null)
            return (null, PasswordVerificationResult.Failed);
        
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, passwordMd5);

        return (user, result);
    }
}