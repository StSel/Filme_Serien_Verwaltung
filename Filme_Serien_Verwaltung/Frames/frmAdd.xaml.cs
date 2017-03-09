using Data;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GUIApp.Frames
{
    public delegate void procFillMediaList();
    /// <summary>
    /// Interaktionslogik für frmAdd.xaml
    /// </summary>
    public partial class frmAdd : MetroWindow
    {

        private APIAccess _APIAcc;
        private ObservableCollection<VideoDumpObj> _MyList = new ObservableCollection<VideoDumpObj>();

        public ObservableCollection<VideoDumpObj> MyList { get { return _MyList; }}

        public event procFillMediaList FillMediaObjList;

        public frmAdd(APIAccess APIAccess)
        {
            _APIAcc = APIAccess;

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
                btnSearchClear.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSearchClear.Visibility = Visibility.Visible;
            }
        }

        private void tbSearchBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (tbSearchBox.Text == "" && tbSearchBox.Text != "Suchen...")
            {
                btnSearchClear.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSearchClear.Visibility = Visibility.Visible;
            }

            if(e.Key == System.Windows.Input.Key.Enter)
            {
                _MyList.Clear();
                _APIAcc.FillDumpList(_MyList, tbSearchBox.Text);
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
            if (btnSearchClear.Visibility == Visibility.Visible)
            {
                tbSearchBox.Clear();
                tbSearchBox.Focus();
            }
        }

        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            btnSearchClear.Visibility = Visibility.Hidden;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < dgridNew.Items.Count; i++)
            {
                VideoDumpObj item = dgridNew.Items[i] as VideoDumpObj;

                if(item.Checked == true)
                {
                    _APIAcc.IDList.Add(item.ID);
                }
            }
            FillMediaObjList();
        }
    }
}
