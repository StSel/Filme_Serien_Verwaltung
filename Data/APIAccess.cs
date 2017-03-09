using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace Data
{
    public class APIAccess
    {

        private TMDbClient _client;
        private MetroWindow _parent;
        private List<int> _IDList = new List<int>();

        public List<int> IDList
        {
            get
            {
                return _IDList;
            }

            set
            {
                _IDList = value;
            }
        }

        public APIAccess(MetroWindow Parent)
        {
            _parent = Parent;
        }

        async public void Initialize(string APIKey)
        {
            try
            {
                _client = new TMDbClient(APIKey);
            }
            catch (Exception e)
            {
                var MsgSettings = new MetroDialogSettings();
                MsgSettings.DialogMessageFontSize = 15;

                var msg = _parent.ShowMessageAsync("Fehler bei APIKey Erkennung.", e.Message, MessageDialogStyle.Affirmative, MsgSettings);
                var msgResponse = await msg;
            }

        }

        public void FillDumpList(ObservableCollection<VideoDumpObj> List, string Search)
        {
            SearchContainer<SearchMovie> movResults = _client.SearchMovieAsync(Search, 0, true).Result;

            foreach(SearchMovie result in movResults.Results)
            {
                DateTime releasedate = DateTime.Now;

                if (result.ReleaseDate != null)
                {
                    releasedate = Convert.ToDateTime(result.ReleaseDate);
                }

                List.Add(InitMyObj(result.Id, result.Title, result.MediaType.ToString(), releasedate));
            }

            SearchContainer<SearchTv> seasonResults = _client.SearchTvShowAsync(Search).Result;

            foreach(SearchTv result in seasonResults.Results)
            {
                DateTime releasedate = DateTime.Now;

                if (result.FirstAirDate != null)
                {
                    releasedate = Convert.ToDateTime(result.FirstAirDate);
                }

                List.Add(InitMyObj(result.Id, result.Name, result.MediaType.ToString(), releasedate));
            }
        }

        public async void FillMediaList(ObservableCollection<MediaObject> AMediaList)
        {
            for(int i = 0; i < _IDList.Count; i++)
            {
                Movie movie = _client.GetMovieAsync(_IDList[i]).Result;
                if(movie != null)
                {
                    AMediaList.Add(InitMediaObj(movie.Id, movie.Title, movie.Overview, movie.Genres, Convert.ToDateTime(movie.ReleaseDate), movie.PosterPath, movie.VoteAverage, movie.Images, "Film", 
                        null, null, null, movie.Popularity));
                } else
                {
                    TvShow show = _client.GetTvShowAsync(_IDList[i]).Result;
                    if(show != null)
                    {
                        AMediaList.Add(InitMediaObj(show.Id, show.Name, show.Overview, show.Genres, null, show.PosterPath, show.VoteAverage, show.Images, "Serie", show.FirstAirDate,
                            show.LastAirDate, show.NumberOfSeasons, show.Popularity));
                    } else
                    {
                        var MsgSettings = new MetroDialogSettings();
                        MsgSettings.DialogMessageFontSize = 15;

                        var msg = _parent.ShowMessageAsync("Fehler", "Film/Serie nicht gefunden", MessageDialogStyle.Affirmative, MsgSettings);
                        var msgResponse = await msg;
                    }
                }
            }
        }

        private MediaObject InitMediaObj(int id, string title, string beschreibung, List<Genre> genres, DateTime? release, string poster, double rating, Images images, string typ,
            DateTime? firstairdate, DateTime? lastairdate, int? staffelanz, double popu)
        {
            MediaObject newObj = new MediaObject();

            newObj.ID = id;
            newObj.Titel = title;
            newObj.Beschreibung = beschreibung;
            newObj.Genres = genres;
            newObj.Release = release;
            newObj.PosterPath = poster;
            newObj.Rating = rating;
            newObj.Images = images;
            newObj.Typ = typ;
            newObj.FirstAirDate = firstairdate;
            newObj.LastAirDate = lastairdate;
            newObj.StaffelAnzahl = staffelanz;
            newObj.Popularitaet = popu;

            return newObj;
        }

        private VideoDumpObj InitMyObj(int id, string title, string type, DateTime release)
        {
            VideoDumpObj newObj = new VideoDumpObj();
            newObj.ID = id;
            newObj.Title = title;
            newObj.Type = type;
            newObj.Release = release;

            return newObj;
        }
    }
}
