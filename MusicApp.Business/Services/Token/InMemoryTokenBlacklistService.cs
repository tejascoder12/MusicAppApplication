using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Business.Services.Token
{
    public class InMemoryTokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklistedTokens = new();

        public void BlacklistToken(string token, DateTime expiry)
        {
            _blacklistedTokens[token] = expiry;
        }

        public bool IsTokenBlacklisted(string token)
        {
            if (_blacklistedTokens.TryGetValue(token, out var expiry))
            {
                if (DateTime.UtcNow > expiry)
                {
                    _blacklistedTokens.TryRemove(token, out _);
                    return false;
                }
                return true;
            }
            return false;
        }
    }

}
