using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace Data
{
    public class MediaObject : IEquatable<MediaObject>
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public string Genres { get; set; }
        public DateTime? Release { get; set; }
        public string PosterPath { get; set; }
        public double Rating { get; set; }
        public List<ImageData> Images { get; set; }
        public string Typ { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public DateTime? LastAirDate { get; set; }
        public int? StaffelAnzahl { get; set; }
        public double Popularitaet { get; set; }
        public int? Runtime { get; set; }

        public bool Equals(MediaObject other)
        {
            return ((other.ID == ID) && (other.Titel == Titel));
        }
    }
}