using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;

public class EnabledLanguageToSubheadlineConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var segment = (SegmentJson)parameter;
        if ((bool)value)
        {
            return segment.subheadlinelanguage?.FirstOrDefault() ?? string.Empty;
        }
        else
        {
            return segment.subheadline?.FirstOrDefault() ?? string.Empty;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}