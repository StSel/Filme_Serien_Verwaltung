
using System;
using System.Globalization;
using System.Windows.Data;

namespace Handler
{
    public class ContextMenuHeightOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double popupHeight = (double)value;
            double buttonHeight = (double)parameter;

            return popupHeight / 2 + buttonHeight / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
