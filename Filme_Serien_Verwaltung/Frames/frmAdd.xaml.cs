using Data;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Data.APIAccess;

namespace GUIApp.Frames
{
    /// <summary>
    /// Interaktionslogik für frmAdd.xaml
    /// </summary>
    public partial class frmAdd : MetroWindow
    {
        private APIAccess _APIAcc;
        private string _APIKey;
        private ObservableCollection<MyObject> _MyList = new ObservableCollection<MyObject>();

        public ObservableCollection<MyObject> MyList { get { return _MyList; }}

        public frmAdd(string APIKey)
        {
            _APIKey = APIKey;

            InitializeComponent();
        }

        private void tbSearchBox_LostFocus(object sender, RoutedEventArgs e)
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

            if(e.Key == System.Windows.Input.Key.Enter)
            {
                _MyList.Clear();
                _APIAcc.FillList(_MyList, tbSearchBox.Text);
                dgridNew.ItemsSource = _MyList;
            }
        }

        private void tbSearchBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (tbSearchBox.Text == "Suchen...")
            {
                tbSearchBox.Clear();
            }
        }

        private void btnSearchClear_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearchClear.Visibility == System.Windows.Visibility.Visible)
            {
                tbSearchBox.Clear();
                tbSearchBox.Focus();
            }
        }

        private void MetroWindow_Initialized(object sender, System.EventArgs e)
        {
            btnSearchClear.Visibility = Visibility.Hidden;

            _APIAcc = new APIAccess(this);
            _APIAcc.Initialize(_APIKey);
        }

        private void frmAdd_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var deltaWidth = (e.NewSize.Width - e.PreviousSize.Width);
           // dgridNew.Width += deltaWidth;
        }
    }
}
