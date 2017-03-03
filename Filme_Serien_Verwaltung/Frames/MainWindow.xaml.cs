using GUIApp.SaveHandler;
using MahApps.Metro.Controls;
using Data;
using System;
using System.Windows.Data;
using System.Windows.Controls;
using Handler;

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

        private void ShowEinstell()
        {
            frmSettings frmEinstell = new frmSettings(_handlerSettings);
            frmEinstell.Owner = this;
            frmEinstell.UpdateMainWindow += new HandlerUpdateWindow(UpdateMainWindow);
            frmEinstell.ShowDialog();
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
    }
}
