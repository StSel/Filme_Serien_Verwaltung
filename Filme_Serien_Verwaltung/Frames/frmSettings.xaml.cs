using Handler.SaveHandler;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace GUIApp.Frames
{
    public delegate void HandlerUpdateWindow();
    /// <summary>
    /// Interaktionslogik für frmSettings.xaml
    /// </summary>
    public partial class frmSettings : MetroWindow
    {
        public event HandlerUpdateWindow UpdateMainWindow;

        #region Private Variablen
        private SettingsHandler _settings;
        private int _indexDBFormat;
        private string _indexDBPath;
        private ObservableCollection<string> CbxContent { get; set; }

        #endregion

        public frmSettings(SettingsHandler ASettingHandler)
        {
            InitializeComponent();

            CbxContent = new ObservableCollection<string>
            {
                "XML-Datei",
                "SQLite-Datenbank",
                "Andere"
            };

            CbxDB.ItemsSource = CbxContent;
       
            _settings = ASettingHandler;

            CbxDB.SelectedIndex = Convert.ToInt16(_settings.DatabaseFormat);
            edtPathDB.Text = Interfaces.GlobalResources.StandardDBPath + GetDBFormat();
            edttmdAPI.Text = _settings.APIKey;

            _indexDBPath = edtPathDB.Text;
            _indexDBFormat = CbxDB.SelectedIndex;
        }

        private void btnAccept_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settings.DBPath = Handler.FileHandler.IOFunc.GetDirectoryName(edtPathDB.Text);
            _settings.DBFile = Handler.FileHandler.IOFunc.GetFilenameWithExt(edtPathDB.Text);
            _settings.APIKey = edttmdAPI.Text;
            _settings.saveSettings();
            UpdateMainWindow();

            Hide();
            Close();
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private async void frmSetting_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsVisible) return;

            if ((_indexDBFormat != CbxDB.SelectedIndex) || (_indexDBPath != edtPathDB.Text))
            {
                var MsgSettings = new MetroDialogSettings();
                MsgSettings.DialogMessageFontSize = 15;

                var msg = this.ShowMessageAsync("", "Nicht übernommene Änderungen gehen verloren! Wirklich abbrechen?", MessageDialogStyle.AffirmativeAndNegative, MsgSettings);

                e.Cancel = true;

                var msgResponse = await msg;

                if (msgResponse == MessageDialogResult.Affirmative)
                {
                    e.Cancel = false;
                    CbxDB.SelectedIndex = _indexDBFormat;
                    edtPathDB.Text = _indexDBPath;
                    Hide();
                    Close();
                }
            }
            else
            {
                e.Cancel = false;
                Hide();
                Close();
            }
        }

        private void CbxDB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (CbxDB.SelectedIndex)
            {
                case 0: //XML Format
                    {
                        _indexDBFormat = CbxDB.SelectedIndex;
                        _settings.DatabaseFormat = "0";
                        edtPathDB.Text = Interfaces.GlobalResources.StandardDBPath + "Database.xml";
                        break;
                    }
                case 1: // SQLite Format
                    {
                        _indexDBFormat = CbxDB.SelectedIndex;
                        edtPathDB.Text = Interfaces.GlobalResources.StandardDBPath + "Database.sqlite";
                        _settings.DatabaseFormat = "1";
                        break;
                    }
                case 2: //Anderes
                    {
                        break;
                    }
            }
        }

        private void btnFileOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string DBPath = edtPathDB.Text;

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.InitialDirectory = Interfaces.GlobalResources.StandardDBPath;
            dlg.IsFolderPicker = true;
            Visibility = System.Windows.Visibility.Hidden;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DBPath = dlg.FileName + GetDBFormat();
            }

            Visibility = System.Windows.Visibility.Visible;
            edtPathDB.Text = DBPath;
        }

        private string GetDBFormat()
        {
            string result = "##";

            switch (CbxDB.SelectedIndex)
            {
                case 0: //XML Format
                    {
                        result = "Database.xml";
                        break;
                    }
                case 1: // SQLite Format
                    {
                        result = "Database.sqlite";
                        break;
                    }
                case 2: //Anderes
                    {
                        result = "##";
                        break;
                    }
            }

            return result;
        }
    }
}
