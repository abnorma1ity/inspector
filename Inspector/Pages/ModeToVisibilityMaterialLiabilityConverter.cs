using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Inspector.Pages
{
    public class ModeToVisibilityMaterialLiabilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ViewMode mode = (ViewMode)values[0];
            Выдача selected = (Выдача)values[1];

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

        public static ModeToVisibilityMaterialLiabilityConverter Instance { get; } = new ModeToVisibilityMaterialLiabilityConverter();
    }
    [MarkupExtensionReturnType(typeof(ModeToVisibilityMaterialLiabilityConverter))]
    public class ModeToVisibilityMaterialLiabilityExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ModeToVisibilityMaterialLiabilityConverter.Instance;
        }
    }
}
