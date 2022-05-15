using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Inspector.Pages
{
    public class ModeToСотрудникConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ViewMode mode = (ViewMode)values[0];
                Сотрудник selected = (Сотрудник)values[1];
                Сотрудник editable = (Сотрудник)values[2];

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

        public static ModeToСотрудникConverter Instance { get; } = new ModeToСотрудникConverter();
    }

    [MarkupExtensionReturnType(typeof(ModeToСотрудникConverter))]
    public class ModeToСотрудникExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ModeToСотрудникConverter.Instance;
        }
    }
}
