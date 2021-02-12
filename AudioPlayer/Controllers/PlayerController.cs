using AudioPlayer.Data;
using AudioPlayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public PlayerController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult AudioPlayer(int id, int firstSongIndex)
        {
            AudioPlayerViewModel model = new AudioPlayerViewModel()
            {
                Playlist = _appDbContext.GetPlaylist(id),
                Songs = _appDbContext.GetSongsOfPlaylist(id),
                FirstSongIndex = firstSongIndex,
            };
            return View(model);
        }
    }
}
