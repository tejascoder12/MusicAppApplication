using MusicApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Business.Services.Token
{
    public interface ITokenService
    {
        string GetToken(AppUser user, string audience);
    }
}
