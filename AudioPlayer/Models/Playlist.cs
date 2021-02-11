namespace AudioPlayer.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string SongsID { get; set; } = "";
        public string UserID { get; set; }
    }
}
