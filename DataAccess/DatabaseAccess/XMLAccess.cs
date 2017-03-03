using Interfaces;
using Interfaces.FileHandler;
using System;


namespace DataAccess.DatabaseAccess
{
    public class XMLAccess : IDataBaseAccess
    {
        private XMLFile _xmlFile;

        public XMLAccess(string APath)
        {
            _xmlFile = new XMLFile(APath);
        }

        public string GetBeschreibung()
        {
            throw new NotImplementedException();
        }

        public void GetConnection()
        {
            throw new NotImplementedException();
        }

        public string GetGenres()
        {
            throw new NotImplementedException();
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public double GetRating()
        {
            throw new NotImplementedException();
        }

        public string GetTitle()
        {
            throw new NotImplementedException();
        }

        public int GetTyp()
        {
            throw new NotImplementedException();
        }
    }
}
