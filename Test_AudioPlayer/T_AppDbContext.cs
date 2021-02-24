using AudioPlayer.Data;
using AudioPlayer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Test_AudioPlayer
{
    public class T_AppDbContext
    {
        private string _userID = "094d66f4-bb8a-431d-aaa1-f8d608281e6d";

        #region setup functions
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AudioPlayerTest")
                .Options;
            var dbContext = new ApplicationDbContext(options, new Mock<IWebHostEnvironment>().Object);

            foreach (var playlist in dbContext.Playlists)
                dbContext.Playlists.Remove(playlist);
            foreach (var song in dbContext.Songs)
                dbContext.Songs.Remove(song);

            dbContext.SaveChanges();
            return dbContext;
        }

        private void FillWithPlaylists(ApplicationDbContext dbContext)
        {
            List<Playlist> playlistsData = new List<Playlist>() {
                new Playlist() {Name = "Name", ID = 1, SongsID = "25,27,28", UserID = _userID} ,
                new Playlist() {Name = "Name1", ID = 2, SongsID = "255,256,257,259", UserID = _userID} ,
                new Playlist() {Name = "Name155", ID = 85, SongsID = "278,279,280,281", UserID = "randomID"} ,
                new Playlist() {Name = "Name185", ID = 72, SongsID = "255,256,257,259", UserID = "notUserID"} ,
                new Playlist() {Name = "Name2", ID = 3, SongsID = "80,87,89,90", UserID = _userID} ,
                new Playlist() {Name = "Name28", ID = 86, SongsID = "80,87,89,90", UserID = "esdf-58sf-dd"} ,
            };
            foreach (Playlist playlist in playlistsData)
                dbContext.Playlists.Add(playlist);
            dbContext.SaveChanges();
        }

        private void FillWithPlaylistsAndSongs(ApplicationDbContext dbContext)
        {
            List<Playlist> playlistsData = new List<Playlist>() {
                new Playlist() {Name = "Name", ID = 1, SongsID = "25,27,28", UserID = _userID} ,
                new Playlist() {Name = "Name28", ID = 86, SongsID = "80,87", UserID = "esdf-58sf-dd"}
            };
            List<Song> songsData = new List<Song>()
            {
                new Song() {ID = 25, Path = "/FakeFolder/MusicTitle.mp3", Title="MusicTitle"},
                new Song() {ID = 27, Path = "/FakeFolder/MusicTitle1.mp3", Title="MusicTitle1"},
                new Song() {ID = 28, Path = "/FakeFolder/MusicTitle2.mp3", Title="MusicTitle2"},
                new Song() {ID = 80, Path = "/FakeFolder/MusicTitle3.mp3", Title="MusicTitle3"},
                new Song() {ID = 87, Path = "/FakeFolder/MusicTitle4.mp3", Title="MusicTitle4"},
            };

            foreach (Playlist playlist in playlistsData)
                dbContext.Playlists.Add(playlist);
            foreach (Song song in songsData)
                dbContext.Songs.Add(song);

            dbContext.SaveChanges();
        }
        #endregion

        [Fact]
        public void GetPlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            var Result = dbContext.GetPlaylist(1);
            Assert.NotNull(Result);
        }

        [Fact]
        public void GetUserPlaylists()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            List<Playlist> userPlaylists = new List<Playlist>() {
                new Playlist() {Name = "Name", ID = 1, SongsID = "25,27,28", UserID = _userID} ,
                new Playlist() {Name = "Name1", ID = 2, SongsID = "255,256,257,259", UserID = _userID} ,
                new Playlist() {Name = "Name2", ID = 3, SongsID = "80,87,89,90", UserID = _userID} ,
            };

            var Result = dbContext.GetUserPlaylists(_userID);
            userPlaylists.Should().BeEquivalentTo(Result);
        }

        [Fact]
        public void AddPlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            var Result = dbContext.AddPlaylist(new Playlist() {ID=24});
            Assert.True(Result);
        }

        [Fact]
        public void UserOwnsPlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            var Result = dbContext.UserOwnsPlaylist(_userID, 1);
            Assert.True(Result);

            Result = dbContext.UserOwnsPlaylist(_userID, 85);
            Assert.False(Result);
        }

        [Fact]
        public void DeletePlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            int PlaylistID = 1;

            var Result = dbContext.GetPlaylist(PlaylistID);
            Assert.NotNull(Result);

            dbContext.DeletePlaylist(PlaylistID);

            Result = dbContext.GetPlaylist(PlaylistID);
            Assert.Null(Result);
        }

        [Fact]
        public void RenamePlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            int PlaylistID = 1;
            string OldName = "Name";
            string NewName = "NewName";

            var Playlist = dbContext.GetPlaylist(PlaylistID);
            Assert.Equal(OldName, Playlist.Name);

            var Result = dbContext.RenamePlaylist(PlaylistID, NewName);
            Assert.True(Result);

            Playlist = dbContext.GetPlaylist(PlaylistID);
            Assert.Equal(NewName, Playlist.Name);
        }

        [Fact]
        public void SaveSong()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylists(dbContext);

            Song Song = new Song() { ID = 25, Path = "/FakeFolder/MusicTitle.mp3", Title = "MusicTitle" };

            var Result = dbContext.SaveSong(Song.Path, Song.Title);
            Assert.True(Result > 0);
        }

        [Fact]
        public void AddSongToPLaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylistsAndSongs(dbContext);

            int PlaylistID = 1;
            int SongID = 87;

            var SongsOfPlaylist = dbContext.GetSongsOfPlaylist(PlaylistID);
            foreach (var song in SongsOfPlaylist)
                Assert.NotEqual(SongID, song.ID);

            var Result = dbContext.AddSongToPlaylist(PlaylistID, SongID);
            Assert.True(Result);

            SongsOfPlaylist = dbContext.GetSongsOfPlaylist(PlaylistID);
            bool contains = false;
            foreach (var song in SongsOfPlaylist)
                if (song.ID == SongID)
                    contains = true;
            Assert.True(contains);
        }

        [Fact]
        public void GetSongsOfPlaylist()
        {
            ApplicationDbContext dbContext = GetDbContext();
            FillWithPlaylistsAndSongs(dbContext);

            int PlaylistID = 1;

            var songs = dbContext.GetSongsOfPlaylist(PlaylistID);
            Assert.NotNull(songs);
        }
    }
}
