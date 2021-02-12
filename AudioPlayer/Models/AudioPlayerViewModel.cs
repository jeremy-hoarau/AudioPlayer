using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public class AudioPlayerViewModel
    {
        public Playlist Playlist { get; set; }
        public int FirstSongIndex { get; set; } //first song to play when opening the player
        public List<Song> Songs { get; set; }
    }
}
