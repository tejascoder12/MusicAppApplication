using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Response
{
    public class LoginResultDto : OperationResult
    {
        public string Token { get; set; }
        public UserResponseDto User { get; set; }
    }
}
