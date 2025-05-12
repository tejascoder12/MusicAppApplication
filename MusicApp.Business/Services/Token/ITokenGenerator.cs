using MusicApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Business.Services.Token
{
    public  interface ITokenGenerator
    {
        public string GenerateToken(AppUser user, string audience);
    }
}
