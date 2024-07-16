using System;
using System.Globalization;
using System.Windows.Data;

namespace ZFPimImporter.Converters;

public class WindowWidthToTextBoxWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        double min = 80;
        double substractFromWidth = 146;
        double divide = 6.0f;
        double defaultValue = 230;
        
        if (parameter is string marginParams)
        {
            var margins = marginParams.Split(',');
            if (margins.Length == 4)
            {
                try
                {
                    // Convert string margins to double
                    min = double.Parse(margins[0], CultureInfo.InvariantCulture);
                    substractFromWidth = double.Parse(margins[1], CultureInfo.InvariantCulture);
                    divide= double.Parse(margins[2], CultureInfo.InvariantCulture);
                    defaultValue = double.Parse(margins[3], CultureInfo.InvariantCulture);

                    // Log conversion details

                    // Return the new margin
                }
                catch (FormatException e)
                {
                    // Log format exception details
                 
                }

            }
        }

        try
        {
            if (int.TryParse(value.ToString(),out var widthWindow))
            {
                var newValue =Math.Max(min, ((widthWindow-substractFromWidth) / divide) ) ;
               
                return newValue; 

            }

        }
        catch 
        {
            //
        }
        
        return defaultValue; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting from TextBox Width to Window Width is not supported.");
    }
}