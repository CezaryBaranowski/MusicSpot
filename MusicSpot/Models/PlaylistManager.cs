using MusicSpot.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MusicSpot.Models
{
    public static class PlaylistManager
    {

        public static string PlaylistDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\MusicSpot\Playlists";

        public static void InitPlaylistsDirectory()
        {
            if (!Directory.Exists(PlaylistDirectory))
                Directory.CreateDirectory(PlaylistDirectory);
        }

        public static void AddEmptyPlaylist(string name)
        {
            Playlist newPlaylist = new Playlist(name);
            MusicViewModel.GetInstance().Playlists.Add(newPlaylist);
            MusicViewModel.GetInstance().PlaylistsNames.Add(newPlaylist.Name);
        }

        public static void AddPlaylist(Playlist newPlaylist)
        {
            MusicViewModel.GetInstance().Playlists.Add(newPlaylist);
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                MusicViewModel.GetInstance().PlaylistsNames.Add(newPlaylist.Name);
            });
            SavePlaylistToXML(newPlaylist);
        }

        public static void LoadPlaylist(Playlist newPlaylist)
        {
            MusicViewModel.GetInstance().Playlists.Add(newPlaylist);
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                MusicViewModel.GetInstance().PlaylistsNames.Add(newPlaylist.Name);
            });
        }

        public static void RemovePlaylist(Playlist playlist)
        {
            MusicViewModel.GetInstance().Playlists.Remove(playlist);
            MusicViewModel.GetInstance().PlaylistsNames.Remove(playlist.Name);
        }

        public static void AddSongToPlaylist(string playlistName, Song song)
        {
            var playlist = MusicViewModel.GetInstance().Playlists
                .FirstOrDefault(p => p.Name.Equals(playlistName));
            if (playlist != null)
            {
                playlist.Songs.Add(song);

                SavePlaylistToXML(playlist);
            }
        }

        public static void RemoveSongFromPlaylist(string playlistName, Song song)
        {
            var playlist = MusicViewModel.GetInstance().Playlists
                .FirstOrDefault(p => p.Name.Equals(playlistName));
            if (playlist != null)
            {
                var songToRemoveIndex =
                    playlist.Songs.IndexOf(playlist.Songs.FirstOrDefault(s => s.Path.Equals(song.Path)));
                playlist.Songs.RemoveAt(songToRemoveIndex);
                MusicViewModel.GetInstance().FilteredSongs.RemoveAt(songToRemoveIndex);
                SavePlaylistToXML(playlist);
            }
        }

        private static void SavePlaylistToXML(Playlist playlist)
        {
            var doc = new XDocument();
            var playlistElement = new XElement("playlist");
            playlistElement.Add(new XElement("name", playlist.Name));
            playlistElement.Add(new XElement("path", playlist.Path));
            var tracklistElement = new XElement("tracklist");
            playlistElement.Add(tracklistElement);
            foreach (var song in playlist.Songs)
            {
                tracklistElement.Add(new XElement("song",
                    new XElement("title", song.Title),
                    new XElement("path", song.Path)
                ));
            }
            doc.Add(playlistElement);

            doc.Save(playlist.Path);
        }

        private static Playlist ReadPlaylistFromXML(string path)
        {
            var document = XDocument.Load(path);
            Playlist playlist = new Playlist();
            var playlistName = document.Descendants("name").FirstOrDefault()?.Value;
            var songs = document.XPathSelectElements("/playlist/tracklist/song");
            playlist.Path = path;
            playlist.Name = playlistName;
            foreach (var song in songs)
            {
                var songTitle = song.Descendants("title").FirstOrDefault()?.Value;
                var songPath = song.Descendants("path").FirstOrDefault()?.Value;
                Song readSong = new Song() { Path = songPath, Title = songTitle };
                playlist.Songs.Add(readSong);
            }

            return playlist;
        }

        public static void InitPlaylists()
        {
            InitPlaylistsDirectory();

            IEnumerable<string> files = Directory.GetFiles(PlaylistDirectory, "*.*", SearchOption.AllDirectories);

            var playlistFiles = ((files).Where(f =>
                new[] { ".xml", ".txt" }.Contains(Path.GetExtension(f))));

            foreach (var playlistFile in playlistFiles)
            {
                var playlist = ReadPlaylistFromXML(playlistFile);
                if (playlist != null)
                {
                    LoadPlaylist(playlist);
                }
            }

        }

        public static bool CanRemoveSongFromPlaylist(object parameter)
        {
            if (!MusicViewModel.GetInstance().SelectedPlaylistName.Equals("None"))
                return true;
            return false;
        }

        public static void RemoveSongFromPlaylist(object parameter)
        {
            var currentlySelectedSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
            var currentlySelectedPlaylist = MusicViewModel.GetInstance().Playlists.
                FirstOrDefault(p => p.Name.Equals(MusicViewModel.GetInstance().SelectedPlaylistName));
            if (currentlySelectedPlaylist != null) PlaylistManager.RemoveSongFromPlaylist(currentlySelectedPlaylist.Name, currentlySelectedSong);
            var par = parameter;
        }

    }
}