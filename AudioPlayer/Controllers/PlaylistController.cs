using AudioPlayer.Data;
using AudioPlayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AudioPlayer.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public PlaylistController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: PlaylistController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PlaylistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaylistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePlaylistViewModel model)
        {
            if (ModelState.IsValid)
            {
                Playlist playlist = new Playlist()
                {
                    Name = model.Title,
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    SongsID = ""
                };

                bool result = _appDbContext.AddPlaylist(playlist);

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The playlist creation failed.");
                }
            }
            return View(model);

        }

        // GET: PlaylistController/Edit/5
        [Route("/Playlist/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            //if (!_appDbContext.UserOwnsPlaylist(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                //return RedirectToAction("Index", "Home");

            EditPlaylistViewModel model = new EditPlaylistViewModel()
            {
                ID = id,
                Title = _appDbContext.GetPlaylist(id).Name,
                Songs = _appDbContext.GetSongsOfPlaylist(id)
            };

            return View(model);
        }

        // POST: PlaylistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditPlaylistViewModel model)
        {
            if (!_appDbContext.UserOwnsPlaylist(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                bool result = _appDbContext.RenamePlaylist(id, model.Title);

                if (result)
                {
                    return RedirectToAction("Edit", "Playlist", new { id = id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The operation failed.");
                }
            }
            return View(model);
        }

        // POST PlaylistControler/Edit/5/AddSong
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Playlist/Edit/{id}/AddSong")]
        public async Task<IActionResult> AddSong(int id, EditPlaylistViewModel model)
        {
            if (!_appDbContext.UserOwnsPlaylist(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                return RedirectToAction("Index", "Home");

            string fileName = model.File.FileName;
            string path = "Content/Music/" + fileName;
            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }

            int songID = _appDbContext.SaveSong(path, fileName.Substring(0, fileName.LastIndexOf('.')));
            if (songID != -1)
                _appDbContext.AddSongToPlaylist(id, songID);

            return RedirectToAction("Edit", "Playlist", new { id = id });
        }

        // GET: PlaylistController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!_appDbContext.UserOwnsPlaylist(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                return RedirectToAction("Index", "Home");

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_appDbContext.UserOwnsPlaylist(userID, id))
                _appDbContext.DeletePlaylist(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Playlist/Edit/{id}/DeleteSong/{songId}")]
        // POST: PlaylistController/Delete/5
        public ActionResult DeleteSong(int id, int songId)
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_appDbContext.UserOwnsPlaylist(userID, id))
                _appDbContext.DeleteSongFromPlaylist(id, songId);
            return RedirectToAction("Edit", "Playlist", new { id = id });
        }
    }
}
