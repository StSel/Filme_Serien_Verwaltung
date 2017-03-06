using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Data
{
    public class APIAccess
    {
        private TMDbClient _client;
        private MetroWindow _parent;

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

        public void FillList(ObservableCollection<MyObject> List, string Search)
        {
            SearchContainer<SearchMovie> movResults = _client.SearchMovieAsync(Search).Result;

            foreach(SearchMovie result in movResults.Results)
            {
                string release = "##";

                if (result.ReleaseDate != null)
                {
                    DateTime releasedate = Convert.ToDateTime(result.ReleaseDate);
                    release = releasedate.ToShortDateString();
                }

                List.Add(InitMyObj(result.Title, result.MediaType.ToString(), release));
            }

            SearchContainer<SearchTv> seasonResults = _client.SearchTvShowAsync(Search).Result;

            foreach(SearchTv result in seasonResults.Results)
            {
                string release = "##";

                if(result.FirstAirDate != null)
                {
                    DateTime releasedate = Convert.ToDateTime(result.FirstAirDate);
                    release = releasedate.ToShortDateString();
                }

                List.Add(InitMyObj(result.Name, result.MediaType.ToString(), release));
            }
        }

        private MyObject InitMyObj(string title, string type, string release)
        {
            MyObject newObj = new MyObject();
            newObj.Title = title;
            newObj.Type = type;
            newObj.Release = release;

            return newObj;
            }
    }
}
