using GUIApp.SaveHandler;
using MahApps.Metro.Controls;
using Data;
using System;

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

        public MainWindow()
        {
            InitializeComponent();

            _handlerSettings = new SettingsHandler();
            _handlerSettings.loadSettings(_settings);

            if(_handlerSettings.DBFile != "")
            {
                _dao = new DataAccessObject(Convert.ToInt16(_handlerSettings.DatabaseFormat), _handlerSettings.DBPath + "\\" + _handlerSettings.DBFile);
            }

            if(_handlerSettings.View != 0)
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

        private void btnEinstellungen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowEinstell();
        }

        private void UpdateMainWindow()
        {
            _handlerSettings.loadSettings(_settings);

            if(_dao == null)
            {
                _dao = new DataAccessObject(Convert.ToInt16(_handlerSettings.DatabaseFormat), _handlerSettings.DBPath + "\\" + _handlerSettings.DBFile);
            }
        }

        private void Ctrl_ClickRight(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void miEinstellClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowEinstell();
        }

        private void frmMain_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var deltaWidth = (e.NewSize.Width - e.PreviousSize.Width);
            pnlToolbar.Width += deltaWidth;
        }

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

        private void miAddClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowFrmAdd();
        }

        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _handlerSettings.View = 0;

            if(btnDetailView.IsChecked == true)
            {
                _handlerSettings.View = 1;
            } else if (btnListView.IsChecked == true)
            {
                _handlerSettings.View = 2;
            } else if (btnRasterView.IsChecked == true)
            {
                _handlerSettings.View = 3;
            }

            _handlerSettings.SaveView();
        }

        private void tbSearchBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(tbSearchBox.Text == "Suchen...")
            {
                tbSearchBox.Clear();
            }
        }

        private void tbSearchBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if(tbSearchBox.Text == "")
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

        private void frmMain_Initialized(object sender, EventArgs e)
        {
            btnSearchClear.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSearchClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(btnSearchClear.Visibility == System.Windows.Visibility.Visible)
            {
                tbSearchBox.Clear();
                tbSearchBox.Focus();
            }
        }

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
            frmAdd lfrmAdd = new frmAdd(_handlerSettings.APIKey);
            lfrmAdd.Owner = this;
            lfrmAdd.ShowDialog();
        }
        #endregion
    }
}
