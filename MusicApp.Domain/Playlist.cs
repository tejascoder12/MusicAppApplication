using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Foreign key to the owning user.
        [ForeignKey("User")]
        public int AppUserId { get; set; }
        public AppUser User { get; set; }

        // Many-to-many relationship with Song via the join entity.
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    }
}
