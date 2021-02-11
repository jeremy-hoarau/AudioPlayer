using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Models
{
    public class EditPlaylistViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public IFormFile File { get; set; }

        public List<Song> Songs { get; set; }
    }
}
