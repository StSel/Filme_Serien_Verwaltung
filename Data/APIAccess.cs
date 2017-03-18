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
        private List<VideoDumpObj> _dumpList = new List<VideoDumpObj>();

        public List<VideoDumpObj> DumpList
        {
            get
            {
                return _dumpList;
            }
            set
            {
                _dumpList = value;
            }
        }

        public MetroWindow Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public APIAccess()
        {
            
        }

        async public void Initialize()
        {
            try
            {
                _client = new TMDbClient("86ee40fbf94740af931fb70267593de7");
                _client.DefaultLanguage = "de";
                _client.GetConfig();
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

                List.Add(InitMyObj(result.Id, result.Title, "Film", releasedate));
            }

            SearchContainer<SearchTv> seasonResults = _client.SearchTvShowAsync(Search).Result;

            foreach(SearchTv result in seasonResults.Results)
            {
                DateTime releasedate = DateTime.Now;

                if (result.FirstAirDate != null)
                {
                    releasedate = Convert.ToDateTime(result.FirstAirDate);
                }

                List.Add(InitMyObj(result.Id, result.Name, "Serie", releasedate));
            }
        }

        public async void FillMediaList(ObservableCollection<MediaObject> AMediaList)
        {
            foreach(var item in _dumpList)
            {
                if(item.Type == "Film")
                {
                    Movie movie = _client.GetMovieAsync(item.ID, MovieMethods.Images).Result;

                    AMediaList.Add(InitMediaObj(movie.Id, movie.Title, movie.Overview, movie.Genres, Convert.ToDateTime(movie.ReleaseDate), _client.GetImageUrl("original", movie.PosterPath).ToString(), movie.VoteAverage, 
                        movie.Images.Posters, "Film", null, null, null, movie.Popularity, movie.Runtime));
                    
                } else if(item.Type == "Serie")
                {
                    TvShow show = _client.GetTvShowAsync(item.ID).Result;

                    string lBeschreibung = "";
                    if(show.Overview != null)
                    {
                        lBeschreibung = show.Overview;
                    }

                    List<ImageData> limgData = new List<ImageData>();
                    if(show.Images != null)
                    {
                        limgData = show.Images.Posters;
                    }

                    AMediaList.Add(InitMediaObj(show.Id, show.Name, lBeschreibung, show.Genres, null, _client.GetImageUrl("original", show.PosterPath).ToString(), show.VoteAverage, limgData, "Serie", show.FirstAirDate,
                        show.LastAirDate, show.NumberOfSeasons, show.Popularity, null));
                } else
                {
                    var MsgSettings = new MetroDialogSettings();
                    MsgSettings.DialogMessageFontSize = 15;

                    var msg = _parent.ShowMessageAsync("Fehler", "Film/Serie nicht gefunden", MessageDialogStyle.Affirmative, MsgSettings);
                    var msgResponse = await msg;
                }
            }
        }

        public Uri GetFullImgPath(string aSize, string APath)
        {
           return _client.GetImageUrl(aSize, APath, true);
        }

        private MediaObject InitMediaObj(int id, string title, string beschreibung, List<Genre> genres, DateTime? release, string poster, double rating, List<ImageData> images, string typ,
            DateTime? firstairdate, DateTime? lastairdate, int? staffelanz, double popu, int? runtime)
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
            newObj.Popularitaet = Math.Round(popu, 2);
            newObj.Runtime = runtime;

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
