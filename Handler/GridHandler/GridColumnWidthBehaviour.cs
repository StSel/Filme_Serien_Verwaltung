using System;
using System.Windows;
using System.Windows.Controls;

namespace Handler.GridHandler
{
    public class GridColumnWidthBehaviour
    {
        public static bool GetGridColumnWidthProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetMinColProperty);
        }

        public static void SetSetMinColProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(SetMinColProperty, value);
        }

        private static readonly DependencyProperty SetMinColProperty =
            DependencyProperty.RegisterAttached("SetMinWidthAuto", typeof(bool), typeof(GridColumnWidthBehaviour), new UIPropertyMetadata(false, WireUpLoadedEvent));

        public static void WireUpLoadedEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (DataGrid)d;

            var doIt = (bool)e.NewValue;

            if(doIt)
            {
                grid.Loaded += SetMinWidths;
            }
        }

        public static void SetMinWidths(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)sender;

            foreach(var column in grid.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
