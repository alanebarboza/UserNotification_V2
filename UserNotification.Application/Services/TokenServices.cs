using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserNotification.Application.Authentication;
using UserNotification.Application.Authentication.Interfaces;
using UserNotification.Domain.Commands;

namespace UserNotification.Application.Services
{
    public sealed class TokenServices : ITokenServices
    {
        public string CreateToken(LoginCommand command)
        {
            var handler = new JwtSecurityTokenHandler();
            var secrets = Encoding.ASCII.GetBytes(Settings.Secrets);
            var tokenDescript = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[] { new Claim(ClaimTypes.Name, command.Nick) }
                ),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secrets),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = handler.CreateToken(tokenDescript);

            return handler.WriteToken(token);
        }
    }
}
