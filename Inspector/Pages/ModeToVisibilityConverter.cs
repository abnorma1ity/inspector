using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Inspector.Pages
{
    public class ModeToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ViewMode mode = (ViewMode)values[0];
            Техника selected = (Техника)values[1];

            Boolean vis = false;
            if (mode == ViewMode.Edit || mode == ViewMode.Add)
                vis = true;
            else
                vis = selected != null;

            return vis ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static ModeToVisibilityConverter Instance { get; } = new ModeToVisibilityConverter();
    }
    [MarkupExtensionReturnType(typeof(ModeToVisibilityConverter))]
    public class ModeToVisibilityExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ModeToVisibilityConverter.Instance;
        }
    }
}
