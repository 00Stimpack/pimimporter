using System;
using System.Globalization;
using System.Windows.Data;
using ZFPimImporter.DataTypes;

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters
{
    

    public class EnabledLanguageToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is bool enabledLanguage)
                {
                    // Use the ConverterParameter to pass the correct context
                    if (parameter is FrameworkElement element && element.DataContext is SegmentJson segment)
                    {
                        return segment.name;
                    }
                }
            }
            catch (Exception)
            {
                // Log or handle the exception as required
            }
            return null; // Return a default value or null
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    

    
}
