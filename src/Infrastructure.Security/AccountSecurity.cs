using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleDotnetCleanArchitecture.Infrastructure.Security
{
    public class AccountSecurity : IAccountSecurity
    {
        private readonly string _secretKey;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountSecurity(IPasswordHasher<Account> passwordHasher, string secretKey)
        {
            _secretKey = secretKey;
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(Account user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyHashedPassword(Account user, string passwordHash, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public AccountToken GenerateJwtToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            if (user.Roles != null)
            {
                foreach (var roleName in user.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            if (user.Claims != null)
            {
                foreach (var claimName in user.Claims)
                    claims.Add(new Claim(claimName.ToString(), "true"));
            }

            var expireDate = DateTime.UtcNow.AddHours(1);

            // Configurar o token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(
                                       new SymmetricSecurityKey(key),
                                                          SecurityAlgorithms.HmacSha256Signature)
            };

            // Gerar o token
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var userToken = new AccountToken(user.UserName, tokenString, expireDate);

            user.Tokens.Add(userToken);

            return userToken;
        }
    }
}
