using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Inspector.Pages
{
    public class ModeToТехникаConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ViewMode mode = (ViewMode)values[0];
                Техника selected = (Техника)values[1];
                Техника editable = (Техника)values[2];

                if (mode == ViewMode.Edit || mode == ViewMode.Add)
                    return editable;
                else
                    return selected;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static ModeToТехникаConverter Instance { get; } = new ModeToТехникаConverter();
    }

    [MarkupExtensionReturnType(typeof(ModeToТехникаConverter))]
    public class ModeToТехникаExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ModeToТехникаConverter.Instance;
        }
    }
}
