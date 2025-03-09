using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain
{
    public class PlaylistSong
    {
        [ForeignKey("Playlist")]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}
