using AudioPlayer.Models;
using AudioPlayer.Tools;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }


        #region Playlist
        internal bool AddPlaylist(Playlist playlist)
        {
            Playlists.Add(playlist);
            return SaveChanges() > 0;
        }

        internal List<Playlist> GetUserPlaylists(string userID)
        {
            return Playlists.Where(playlist => playlist.UserID == userID).ToList();
        }

        internal void DeletePlaylist(int id)
        {
            foreach (Song song in GetSongsOfPlaylist(id))
                DeleteSongFromPlaylist(id, song.ID);
            Playlists.Remove(Playlists.Find(id));
            SaveChanges();
        }

        internal bool UserOwnsPlaylist(string userID, int playlistID)
        {
            Playlist playlist = Playlists.Find(playlistID);
            if (playlist == null)
                return false;
            return playlist.UserID == userID;
        }

        internal Playlist GetPlaylist(int playlistID)
        {
            return Playlists.Find(playlistID);
        }

        internal bool RenamePlaylist(int playlistID, string newTitle)
        {
            Playlists.Find(playlistID).Name = newTitle;
            return SaveChanges() > 0;
        }
        #endregion

        #region Song
        internal int SaveSong(string path, string title)
        {
            Song song = new Song()
            {
                Path = path,
                Title = title
            };

            song = Songs.Add(song).Entity;
            if (SaveChanges() > 0)
                return song.ID;
            return -1;
        }

        internal bool AddSongToPlaylist(int playlistID, int songID)
        {
            List<int> songsID = Converter.StringToListOfInt(Playlists.Find(playlistID).SongsID);
            songsID.Add(songID);
            Playlists.Find(playlistID).SongsID = Converter.ListOfIntToString(songsID);
            return SaveChanges() > 0;
        }

        internal List<Song> GetSongsOfPlaylist(int playlistID)
        {
            List<int> songsID = Converter.StringToListOfInt(Playlists.Find(playlistID).SongsID);
            return Songs.Where(song => songsID.Contains(song.ID)).ToList();
        }

        internal void DeleteSongFromPlaylist(int playlistID, int songID)
        {
            List<int> songsID = Converter.StringToListOfInt(Playlists.Find(playlistID).SongsID);
            songsID.Remove(songID);
            Playlists.Find(playlistID).SongsID = Converter.ListOfIntToString(songsID);
            Song song = Songs.Find(songID);
            Songs.Remove(song);
            SaveChanges();

            if(!Songs.Where(s => s.Path == song.Path).Any())
            {
                File.Delete(song.Path);
            }
        }
        #endregion
    }
}
