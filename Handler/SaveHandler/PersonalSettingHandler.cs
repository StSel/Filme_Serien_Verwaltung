using System;
using System.IO;
using Interfaces.FileHandler;

namespace Handler.SaveHandler
{
    /*
        Handlerklasse für die Optionen
        */
    public class SettingsHandler
    {
        #region Konstanten

        private string cDatabase = Interfaces.GlobalResources.Database;
        private string cSectionAppearence = Interfaces.GlobalResources.SectionAppearence;
        private string cIniFile = Interfaces.GlobalResources.SettingIni;
        private string cSectionDatabase = Interfaces.GlobalResources.SectionDatabase;
        private string cDatabasePath = Interfaces.GlobalResources.DatabasePath;
        private string cDatabaseFile = Interfaces.GlobalResources.DatabaseFile;
        private string cSectionAPIKey = Interfaces.GlobalResources.SectionAPIKey;
        private string cApiKey = Interfaces.GlobalResources.ApiKey;
        private string cView = Interfaces.GlobalResources.View;

        #endregion

        #region Private Variablen
        private string database;
        private string dbpath;
        private string dbfile;
        private string apikey;
        private int view;

        #endregion

        #region Properties
        public int View
        {
            get { return view; }
            set { view = value; }
        }

        public string APIKey
        {
            get { return apikey; }
            set { apikey = value; }
        }

        public string DBFile
        {
            get { return dbfile; }
            set { dbfile = value; }
        }

        public string DBPath
        {
            get { return dbpath; }
            set { dbpath = value; }
        }

        public string DatabaseFormat
        {
            get { return database; }
            set { database = value; }
        }
        #endregion

        #region Konstruktor
        public SettingsHandler()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);
        }
        #endregion

        public void initStandardSettings()
        {
            database = "0";
            dbpath = Interfaces.GlobalResources.StandardDBPath;
            dbfile = "Database.xml";
            apikey = "0";
            view = 0;
        }

        public void saveSettings()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);

            IniFile MyFile = new IniFile(Interfaces.GlobalResources.StandardDBPath + cIniFile);

            MyFile.Write(cDatabase, database, cSectionDatabase);
            MyFile.Write(cDatabasePath, dbpath, cSectionDatabase);
            MyFile.Write(cDatabaseFile, dbfile, cSectionDatabase);
            MyFile.Write(cApiKey, apikey, cSectionAPIKey);
        }

        public void SaveView()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);

            IniFile MyFile = new IniFile(Interfaces.GlobalResources.StandardDBPath + cIniFile);

            MyFile.Write(cView, Convert.ToString(view), cSectionAppearence);
        }

        public void loadStoredSettings()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);

            IniFile MyFile = new IniFile(Interfaces.GlobalResources.StandardDBPath + cIniFile);

            database = MyFile.Read(cDatabase, cSectionDatabase);
            dbpath = MyFile.Read(cDatabasePath, cSectionDatabase);
            dbfile = MyFile.Read(cDatabaseFile, cSectionDatabase);
            apikey = MyFile.Read(cApiKey, cSectionAPIKey);
            view = Convert.ToInt16(MyFile.Read(cView, cSectionAppearence));
        }

        public void loadSettings(string filename)
        {
            if (File.Exists(Interfaces.GlobalResources.StandardDBPath + filename))
            {
                loadStoredSettings();
            }
            else
            {
                initStandardSettings();
            }
        }
    }
}