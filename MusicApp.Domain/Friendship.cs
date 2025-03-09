using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain
{
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blocked
    }

    public class Friendship
    {
        // The user who sends the friend request.
        [ForeignKey("Requester")]
        public int RequesterId { get; set; }
        public AppUser Requester { get; set; }

        // The user who receives the friend request.
        [ForeignKey("Addressee")]
        public int AddresseeId { get; set; }
        public AppUser Addressee { get; set; }

        [Required]
        public FriendshipStatus Status { get; set; }

        [Required]
        public DateTime RequestedOn { get; set; }

        public DateTime? AcceptedOn { get; set; }
    }
}
