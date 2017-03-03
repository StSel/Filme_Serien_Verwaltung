using DataAccess.DatabaseAccess;
using Interfaces;
using System;

namespace Data
{
    public class DataAccessObject
    {
        private IDataBaseAccess IDAO;

        public DataAccessObject(int Access, string AConnPath)
        {
            switch (Access)
            {
                case 0:
                    {
                        IDAO = new XMLAccess(AConnPath);
                        break;
                    }
                case 1:
                    {
                        IDAO = new SQLiteDBAccess();
                        break;
                    }
                case 2:
                    {
                        break;
                    }
            }
        }

        public string GetBeschreibung()
        {
            return IDAO.GetBeschreibung();
        }

        public void GetConnection()
        {
            IDAO.GetConnection();
        }

        public string GetGenres()
        {
            return IDAO.GetGenres();
        }

        public int GetID()
        {
            return IDAO.GetID();
        }

        public double GetRating()
        {
            return IDAO.GetRating();
        }

        public string GetTitle()
        {
            return IDAO.GetTitle();
        }

        public int GetTyp()
        {
            return IDAO.GetTyp();
        }

    }
}
