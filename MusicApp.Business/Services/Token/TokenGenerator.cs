using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Business.Options;
using MusicApp.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Business.Services.Token
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly TokenGenerationOptions _options;
        private readonly IConfiguration _configuration;

        public TokenGenerator(IOptions<TokenGenerationOptions> options, IConfiguration configuration)
        {
            _options = options.Value;
            _configuration = configuration;
            _options.SecretKey = _configuration["Jwt:Key"] ?? _options.SecretKey;
            _options.Issuer = _configuration["Jwt:Issuer"] ?? _options.Issuer;
            _options.Audience = _configuration["Jwt:Audience"] ?? _options.Audience;
            if (int.TryParse(_configuration["Jwt:ExpireMinutes"], out int expiry))
            {
                _options.ExpiryMinutes = expiry;
            }
        }

        public string GenerateToken(AppUser user, string audience)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
