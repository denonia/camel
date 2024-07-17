using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Camel.Bancho.Services.Interfaces;

public interface IAuthService
{
   (User?, PasswordVerificationResult) AuthenticateUser(string userName, string passwordMd5);
}