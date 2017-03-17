using MahApps.Metro.Controls;
using Data;
using System;
using System.Collections.ObjectModel;
using Handler.SaveHandler;

namespace GUIApp.Frames
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string _settings = Interfaces.GlobalResources.SettingIni;
        private SettingsHandler _handlerSettings;
        private DataAccessObject _dao;
        private APIAccess _apiAccess;

        private ObservableCollection<MediaObject> _dumplist = new ObservableCollection<MediaObject>();
        private ObservableCollection<MediaObject> _MediaList = new ObservableCollection<MediaObject>();

        public ObservableCollection<MediaObject> MediaList { get { return _dumplist; } }

        public MainWindow()
        {
            InitializeComponent();

            _handlerSettings = new SettingsHandler();
            _handlerSettings.loadSettings(_settings);

            if(_handlerSettings.DBFile != "")
            {
                _dao = new DataAccessObject(Convert.ToInt16(_handlerSettings.DatabaseFormat), _handlerSettings.DBPath + "\\" + _handlerSettings.DBFile);
            }

            SetView();
            SetAPIKey();
            SetMenuItemBackupVisibility();
        }

        private void FillMediaList()
        {
            _dumplist.Clear();
            _apiAccess.FillMediaList(_dumplist);

            foreach (var item in _dumplist)
            {
                if(!(_MediaList.Contains(item)))
                {
                    _MediaList.Add(item);
                }
            }

            listBox.ItemsSource = _MediaList;
        }

        private void SetView()
        {
            if (_handlerSettings.View != 0)
            {
                switch (_handlerSettings.View)
                {
                    case 1:
                        {
                            btnDetailView.IsChecked = true;
                            break;
                        }
                    case 2:
                        {
                            btnListView.IsChecked = true;
                            break;
                        }
                    case 3:
                        {
                            btnRasterView.IsChecked = true;
                            break;
                        }
                }
            }
        }

        private void SetAPIKey()
        {
            _apiAccess = new APIAccess();
            _apiAccess.Initialize();
        }

        private bool GetBackupVisibility()
        {
            if(_handlerSettings.DatabaseFormat != "")
            {
                if (_handlerSettings.DatabaseFormat == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void SetMenuItemBackupVisibility()
        {
            if (GetBackupVisibility())
            {
                miExportBackup.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                miExportBackup.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnSearchClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(btnSearchClear.Visibility == System.Windows.Visibility.Visible)
            {
                tbSearchBox.Clear();
                tbSearchBox.Focus();
            }
        }

        #region MainWindow
        private void frmMain_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var deltaWidth = (e.NewSize.Width - e.PreviousSize.Width);
            pnlToolbar.Width += deltaWidth;
        }

        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _handlerSettings.View = 0;

            if (btnDetailView.IsChecked == true)
            {
                _handlerSettings.View = 1;
            }
            else if (btnListView.IsChecked == true)
            {
                _handlerSettings.View = 2;
            }
            else if (btnRasterView.IsChecked == true)
            {
                _handlerSettings.View = 3;
            }

            _handlerSettings.SaveView();
        }

        private void frmMain_Initialized(object sender, EventArgs e)
        {
            btnSearchClear.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateMainWindow()
        {
            _handlerSettings.loadSettings(_settings);

            if (_dao == null)
            {
                _dao = new DataAccessObject(Convert.ToInt16(_handlerSettings.DatabaseFormat), _handlerSettings.DBPath + "\\" + _handlerSettings.DBFile);
            }

            if (_apiAccess == null)
            {
                _apiAccess = new APIAccess();
                _apiAccess.Initialize();
            }

            SetMenuItemBackupVisibility();
        }

        private void frmMain_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Add)
            {
                ShowFrmAdd();
            }
        }
        #endregion

        #region Show Windows
        private void ShowEinstell()
        {
            frmSettings frmEinstell = new frmSettings(_handlerSettings);
            frmEinstell.Owner = this;
            frmEinstell.UpdateMainWindow += new HandlerUpdateWindow(UpdateMainWindow);
            frmEinstell.ShowDialog();
        }

        private void ShowFrmAdd()
        {
            frmAdd lfrmAdd = new frmAdd(_apiAccess);
            lfrmAdd.Owner = this;
            lfrmAdd.FillMediaObjList += new procFillMediaList(FillMediaList);
            lfrmAdd.ShowDialog();
        }
        #endregion

        #region SearchBox
        private void tbSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tbSearchBox.Text == "")
            {
                tbSearchBox.Text = "Suchen...";
            }

            if (tbSearchBox.Text == "" || tbSearchBox.Text == "Suchen...")
            {
                btnSearchClear.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                btnSearchClear.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void tbSearchBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (tbSearchBox.Text == "" && tbSearchBox.Text != "Suchen...")
            {
                btnSearchClear.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                btnSearchClear.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void tbSearchBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tbSearchBox.Text == "Suchen...")
            {
                tbSearchBox.Clear();
            }
        }
        #endregion

        #region View Buttons
        private void btnListView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnRasterView.IsChecked = false;
            btnDetailView.IsChecked = false;
            btnListView.IsChecked = true;
        }

        private void btnRasterView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnListView.IsChecked = false;
            btnDetailView.IsChecked = false;
            btnRasterView.IsChecked = true;
        }

        private void btnDetailView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnListView.IsChecked = false;
            btnRasterView.IsChecked = false;
            btnDetailView.IsChecked = true;
        }
        #endregion

        #region Menu Buttons
        private void Ctrl_ClickRight(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void miEinstellClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowEinstell();
        }

        private void miAddClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowFrmAdd();
        }

        private void miImport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //
        }

        private void miExport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //
        }

        private void miExportBackup_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //
        }
        #endregion

        private void listBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MediaObject item = listBox.SelectedItem as MediaObject;

            if(item != null)
            {
                lblTitleScrView.Text = item.Titel;
                lblBeschreibungScrView.Text = item.Beschreibung;
                lblRatingScrView.Text = "Rating: " + item.Rating.ToString();
                lblPopuScrView.Text = "Popularität " + item.Popularitaet.ToString();

                lbGenres.Items.Clear();
                foreach(var genre in item.Genres)
                {
                    lbGenres.Items.Add(genre.Name);
                }
            }
        }
    }
}
