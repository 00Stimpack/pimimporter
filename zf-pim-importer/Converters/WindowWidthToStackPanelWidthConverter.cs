using System;
using System.Globalization;
using System.Windows.Data;

namespace ZFPimImporter.Converters;

public class WindowWidthToStackPanelWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        try
        {
            if (int.TryParse(value.ToString(),out var widthWindow))
            {
                var newValue =Math.Max(100, widthWindow / 5 ) ;
                return newValue; // Example: Window width minus 100, but not less than 230

            }

        }
        catch 
        {
            //
        }
      

        return 230; // Default width if conversion fails
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting from TextBox Width to Window Width is not supported.");
    }
}




public class WindowWidthToMenuWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //ConverterParameter='80,146,6,230'

        try
        {
            if (int.TryParse(value.ToString(),out var widthWindow))
            {
                var newValue =Math.Max(80, ((widthWindow-400) / 3f) ) ;
                return newValue; 
            }

        }
        catch 
        {
            //
        }
      

        return 230; // Default width if conversion fails
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting from TextBox Width to Window Width is not supported.");
    }
}