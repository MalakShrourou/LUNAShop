using eCommerceAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(Role role, int userId, bool rememberMe)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role), "Role cannot be null.");

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.NameEN)
            };

            // Add permissions as claims
            foreach (Permission permission in role.Permissions)
            {
                claims.Add(new Claim("Permission", permission.Action)); // Ensure Action is the correct property
            }

            // Ensure your secret is long enough for security
            string secretKey = _configuration["JWT:Secret"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT Secret key is not configured.");

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = rememberMe ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
