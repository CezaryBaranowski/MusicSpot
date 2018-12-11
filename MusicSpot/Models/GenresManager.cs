using MusicSpot.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MusicSpot.Models
{
    public static class GenresManager
    {
        public static void InitGenres()
        {
            MusicViewModel.GetInstance().MyMusicGenres = new ObservableCollection<string> { "All" };

            foreach (var song in MusicViewModel.GetInstance().Songs)
            {
                var genres = song.Genres as IEnumerable<string>;
                if (genres.Any())
                {
                    genres = genres.Except(MusicViewModel.GetInstance().MyMusicGenres);
                    foreach (var genre in genres)
                    {
                        MusicViewModel.GetInstance().MyMusicGenres.Add(genre);
                    }
                }
            }
        }
    }
}