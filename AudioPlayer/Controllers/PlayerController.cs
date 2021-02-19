using AudioPlayer.Data;
using AudioPlayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace AudioPlayer.Controllers
{
    public class PlayerController : CustomController
    {
        private readonly ApplicationDbContext _appDbContext;

        public PlayerController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult AudioPlayer(int id, int firstSongIndex)
        {
            if (!_appDbContext.UserOwnsPlaylist(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                return RedirectToAction("Index", "Home");

            List<Song> songs = _appDbContext.GetSongsOfPlaylist(id);
            string[] songsPaths = new string[songs.Count];
            string[] songsTitles = new string[songs.Count];

            for(int i = 0; i < songs.Count; i++)
            {
                songsPaths[i] = songs[i].Path;
                songsTitles[i] = songs[i].Title;
            }

            AudioPlayerViewModel model = new AudioPlayerViewModel()
            {
                Playlist = _appDbContext.GetPlaylist(id),
                SongsPaths = songsPaths,
                SongsTitles = songsTitles,
                FirstSongIndex = firstSongIndex,
            };
            return View(model);
        }
    }
}
