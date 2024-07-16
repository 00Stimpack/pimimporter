using System;
using System.Globalization;
using System.Windows.Data;

namespace ZFPimImporter.Converters
{
    internal class IsEnabledSecondLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

