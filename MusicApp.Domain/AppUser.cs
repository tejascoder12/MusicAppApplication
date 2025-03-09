using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApp.Domain
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // The last time the user was active.
        public DateTime LastActive { get; set; }

        // Creation date is required and set when the user is created.
        [Required]
        public DateTime CreatedDate { get; set; }

        // ModifiedDate is updated on user profile changes.
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties for related entities
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<FavoriteSong> FavoriteSongs { get; set; } = new List<FavoriteSong>();
        public ICollection<WishlistSong> WishlistSongs { get; set; } = new List<WishlistSong>();

        // Friendships where this user is the sender of the friend request.
        public ICollection<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
        // Friendships where this user is the recipient.
        public ICollection<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
    }
}
