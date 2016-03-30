using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Armstrong.GSM
{
    public class BooleanOrConverter : MarkupExtension, IMultiValueConverter
    {
        #region MarkupExtension
        private static readonly Lazy<BooleanOrConverter> _instance =
            new Lazy<BooleanOrConverter>(() => new BooleanOrConverter());

        public static BooleanOrConverter Instance
        {
            get { return _instance.Value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
        #endregion

        #region Conversion
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.OfType<bool>().Any(x => x == true);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetTypes == null) return null;
            var result = new object[targetTypes.Length];
            for (int index = 0; index < targetTypes.Length; ++index)
                result[index] = DependencyProperty.UnsetValue;
            return result;
        }
        #endregion
    }
}
