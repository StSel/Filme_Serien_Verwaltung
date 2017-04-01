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
            if(listBox.Items.Count == 0)
            {
                scrViewMain.Visibility = System.Windows.Visibility.Hidden;
            }
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

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (listBox.Items.Count > 0)
            {
                scrViewMain.Visibility = System.Windows.Visibility.Visible;
                listBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Show Windows
        private void ShowEinstell()
        {
            _handlerSettings.SaveView();
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

            _handlerSettings.View = 2;
        }

        private void btnRasterView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnListView.IsChecked = false;
            btnDetailView.IsChecked = false;
            btnRasterView.IsChecked = true;
            _handlerSettings.View = 3;
        }

        private void btnDetailView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnListView.IsChecked = false;
            btnRasterView.IsChecked = false;
            btnDetailView.IsChecked = true;
            _handlerSettings.View = 1;

            tbControlMain.SelectedIndex = 0;
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

                if(item.Beschreibung != "")
                {
                    lblBeschreibungScrView.Visibility = System.Windows.Visibility.Visible;
                    lblBeschreibungScrView.Text = item.Beschreibung;
                } else
                {
                    lblBeschreibungScrView.Visibility = System.Windows.Visibility.Hidden;
                }

                lblRatingScrView.Text = "Rating: " + item.Rating.ToString();
                lblPopuScrView.Text = "Popularität " + item.Popularitaet.ToString();
                
                if(item.Typ == "Film")
                {
                    if(item.Release != null)
                    {
                        DateTime date = (DateTime)item.Release;
                        lblReleaseScrView.Text = "Release: " + date.ToShortDateString();
                    }

                    lblRuntimeScrView.Text = "Laufzeit: " + item.Runtime.ToString() + " Min";
                } else
                {
                    if(item.FirstAirDate != null)
                    {
                        lblReleaseScrView.Visibility = System.Windows.Visibility.Visible;
                        DateTime fdate = (DateTime)item.FirstAirDate;
                        lblReleaseScrView.Text = "Beginn: " + fdate.ToShortDateString();
                        if(item.LastAirDate != null)
                        {
                            DateTime ldate = (DateTime)item.LastAirDate;
                            lblReleaseScrView.Text = lblReleaseScrView.Text + "\n" +
                                                        "Ende: " + ldate.ToShortDateString();
                        }
                    } else
                    {
                        lblReleaseScrView.Visibility = System.Windows.Visibility.Hidden;
                    }
                    lblRuntimeScrView.Text = "Staffelanzahl: " + item.StaffelAnzahl.ToString();
                }

                lbGenres.Items.Clear();
                foreach(var genre in item.Genres)
                {
                    lbGenres.Items.Add(genre.Name);
                }

                stpnlScrViewImg.Children.Clear();
                if(item.Images.Count > 0)
                {
                    ScrViewImages.Visibility = System.Windows.Visibility.Visible;
                    foreach (var image in item.Images)
                    {
                        var bc = new System.Windows.Media.BrushConverter();
                        System.Windows.Controls.Grid lgrdImage = new System.Windows.Controls.Grid();
                        lgrdImage.Width = 150;
                        lgrdImage.Height = 150;
                        lgrdImage.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF0A0A0A");
                        lgrdImage.Opacity = 0.7;

                        var img = new System.Windows.Media.Imaging.BitmapImage(_apiAccess.GetFullImgPath("w92", image.FilePath));
                        var imgCtrl = new CachedImage.Image();
                        imgCtrl.Source = img;
                        imgCtrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        imgCtrl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        imgCtrl.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                        lgrdImage.Children.Add(imgCtrl);
                        stpnlScrViewImg.Children.Add(lgrdImage);
                    }
                } else
                {
                    ScrViewImages.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
    }
}
