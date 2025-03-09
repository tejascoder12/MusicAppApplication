using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

        [Required]
        [MaxLength(500)]
        public string MessageContent { get; set; }

        [Required]
        public DateTime SentTime { get; set; }
    }
}
