using Microsoft.Extensions.Options;
using MusicApp.Business.Options;
using MusicApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Business.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly TokenGenerationOptions _options;

        public TokenService(
            ITokenGenerator tokenGenerator,
            IOptions<TokenGenerationOptions> options)
        {
            _tokenGenerator = tokenGenerator;
            _options = options.Value;
        }

        public string GetToken(AppUser user, string audience)
        {
            // Use the token generator and pass the user and audience
            return _tokenGenerator.GenerateToken(user, audience ?? _options.Audience);
        }
    }
}
