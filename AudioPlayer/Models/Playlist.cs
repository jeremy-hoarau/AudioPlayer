using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioPlayer.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SongsID { get; set; }
        public int UserID { get; set; }
    }
}
