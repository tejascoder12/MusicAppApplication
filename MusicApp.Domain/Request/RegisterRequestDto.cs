using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain.Request
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
