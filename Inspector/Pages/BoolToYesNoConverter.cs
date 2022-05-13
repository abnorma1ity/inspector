using System;
using System.Globalization;
using System.Windows.Data;

namespace Inspector.Pages
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool yes)
                return yes ? "Да" : "Нет";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static BoolToYesNoConverter Instance { get; } = new BoolToYesNoConverter();
    }

}
