using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;

namespace Data
{
    public class MediaObject
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public List<Genre> Genres { get; set; }
        public DateTime? Release { get; set; }
        public string PosterPath { get; set; }
        public double Rating { get; set; }
        public Images Images { get; set; }
        public string Typ { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public DateTime? LastAirDate { get; set; }
        public int? StaffelAnzahl { get; set; }
        public double Popularitaet { get; set; }
    }
}