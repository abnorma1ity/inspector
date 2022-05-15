using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Inspector.Pages
{
    public class ModeToВыдачаConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ViewMode mode = (ViewMode)values[0];
                Выдача selected = (Выдача)values[1];
                Выдача editable = (Выдача)values[2];

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

        public static ModeToВыдачаConverter Instance { get; } = new ModeToВыдачаConverter();
    }

    [MarkupExtensionReturnType(typeof(ModeToВыдачаConverter))]
    public class ModeToВыдачаExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ModeToВыдачаConverter.Instance;
        }
    }
}
