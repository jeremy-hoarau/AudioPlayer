using System.Collections.Generic;

namespace AudioPlayer.Models
{
    public class AudioPlayerViewModel
    {
        public Playlist Playlist { get; set; }
        public int FirstSongIndex { get; set; } //first song to play when opening the player
        public string[] SongsPaths { get; set; }
        public string[] SongsTitles { get; set; }
    }
}
