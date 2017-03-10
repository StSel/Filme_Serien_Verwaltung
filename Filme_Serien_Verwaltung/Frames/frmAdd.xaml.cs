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
            _APIAcc.Parent = this;

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

                if(_MyList.Count > 0)
                {
                    foreach(DataGridColumn col in dgridNew.Columns)
                    {
                        switch(col.DisplayIndex)
                        {
                            case 0:
                                {
                                    col.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                                    break;
                                }
                            case 1:
                                {
                                    col.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells);
                                    break;
                                }
                            case 2:
                                {
                                    col.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                                    break;
                                }
                            case 3:
                                {
                                    col.Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);
                                    break;
                                }
                        }
                    }

                    Dispatcher.Invoke(new Action(delegate ()
                    {
                        dgridNew.SelectedIndex = 0;
                        dgridNew.Focus();
                    }
                    ), System.Windows.Threading.DispatcherPriority.Background);
                }
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
            tbSearchBox.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            FillLists();

            DialogResult = true;
        }

        private void tbSearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchBox.Text == "Suchen...")
            {
                tbSearchBox.Clear();
            }
        }

        private void btnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            FillLists();
        }

        private void FillLists()
        {
            _APIAcc.DumpList.Clear();
            foreach (VideoDumpObj item in dgridNew.ItemsSource)
            {
                if (item.Checked == true)
                {
                    _APIAcc.DumpList.Add(item);
                }
            }

            FillMediaObjList();

            _MyList.Clear();
            tbSearchBox.Focus();
        }
    }
}
