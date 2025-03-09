using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Domain
{
    public class FavoriteSong
    {
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}
