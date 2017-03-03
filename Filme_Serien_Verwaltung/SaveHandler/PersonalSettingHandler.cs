using System;
using System.IO;
using GUIApp.StyleHandler;
using Interfaces.FileHandler;

namespace GUIApp.SaveHandler
{
    /*
        Handlerklasse für die Optionen
        */
    public class SettingsHandler
    {
        #region Konstanten

        private string cDatabase = Interfaces.GlobalResources.Database;
        private string cTheme = Interfaces.GlobalResources.Theme;
        private string cAccent = Interfaces.GlobalResources.Accent;
        private string cSectionAppearence = Interfaces.GlobalResources.SectionAppearence;
        private string cIniFile = Interfaces.GlobalResources.SettingIni;
        private string cSectionDatabase = Interfaces.GlobalResources.SectionDatabase;
        private string cDatabasePath = Interfaces.GlobalResources.DatabasePath;
        private string cDatabaseFile = Interfaces.GlobalResources.DatabaseFile;

        #endregion

        #region Private Variablen

        private int selectedTheme;
        private int selectedAccent;
        private string database;
        private string dbpath;
        private string dbfile;

        #endregion

        #region Properties
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

        public int SelectedTheme
        {
            get { return selectedTheme; }
            set { selectedTheme = value; }
        }

        public int SelectedAccent
        {
            get { return selectedAccent; }
            set { selectedAccent = value; }
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
            selectedTheme = (int)StyleManager.getAppTheme();
            selectedAccent = (int)StyleManager.getAppAccent();
            database = "0";
            dbpath = Interfaces.GlobalResources.StandardDBPath;
            dbfile = "Database.xml";
        }

        public void saveSettings()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);

            IniFile MyFile = new IniFile(Interfaces.GlobalResources.StandardDBPath + cIniFile);

            MyFile.Write(cDatabase, database, cSectionDatabase);
            MyFile.Write(cDatabasePath, dbpath, cSectionDatabase);
            MyFile.Write(cDatabaseFile, dbfile, cSectionDatabase);
        }

        public void loadStoredSettings()
        {
            if (!Directory.Exists(Interfaces.GlobalResources.StandardDBPath))
                Directory.CreateDirectory(Interfaces.GlobalResources.StandardDBPath);

            IniFile MyFile = new IniFile(Interfaces.GlobalResources.StandardDBPath + cIniFile);

            database = MyFile.Read(cDatabase, cSectionDatabase);
            dbpath = MyFile.Read(cDatabasePath, cSectionDatabase);
            dbfile = MyFile.Read(cDatabaseFile, cSectionDatabase);
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