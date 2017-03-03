using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDataBaseAccess
    {
        void GetConnection();
        int GetID();
        string GetTitle();
        double GetRating();
        string GetGenres();
        int GetTyp();
        string GetBeschreibung();
    }
}
