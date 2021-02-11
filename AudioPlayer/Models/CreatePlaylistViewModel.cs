using System.ComponentModel.DataAnnotations;

namespace AudioPlayer.Models
{
    public class CreatePlaylistViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
    }
}
