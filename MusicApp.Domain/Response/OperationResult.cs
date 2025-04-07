using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Response
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        // Added status code property to represent HTTP or custom status codes
        public int StatusCode { get; set; }

    }

    public static class ResponseStatusCodes
    {
        // Common HTTP status codes (or your custom status codes)
        public const int Success = 200;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int NotFound = 404;
        public const int InternalServerError = 500;
    }
}
