namespace ZFPimImporter.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


public class WindowWidthMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Ensure the value is a double and the parameter is a string
        if (parameter is string marginParams)
        {
            // Parse the margin parameters
            double left = 0;
            double top = 0;
            double right = 0;
            double bottom= 0;
            var margins = marginParams.Split(',');
            if (margins.Length == 4)
            {
                try
                {
                    // Convert string margins to double
                     left = double.Parse(margins[0], CultureInfo.InvariantCulture);
                     top = double.Parse(margins[1], CultureInfo.InvariantCulture);
                     right = double.Parse(margins[2], CultureInfo.InvariantCulture);
                     bottom = double.Parse(margins[3], CultureInfo.InvariantCulture);

                    // Log conversion details

                    // Return the new margin
                }
                catch (FormatException e)
                {
                    // Log format exception details
                    //Console.WriteLine($"Error parsing margin parameters: {marginParams}. Exception: {e.Message}");
                    return new Thickness(0);
                }
                
                if (int.TryParse(value.ToString(),out var widthWindow))
                {
                    var newValue =Math.Max(80, ((widthWindow-400) / 3f) ) ;
                    //Console.WriteLine($"Converting margin with parameters: {marginParams}. newValue: {newValue}");
                    return new Thickness(left, top, right, bottom);
               
                }
                return new Thickness(left, top, right, bottom);

            }
            //Console.WriteLine($"Invalid margin parameters count: {margins.Length}. Expected 4.");
            return new Thickness(0);
        }
        //Console.WriteLine($"Invalid conversion types. Expected double for value and string for parameter. Received: {value.GetType()} and {parameter?.GetType()}");
        return new Thickness(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack is not implemented in MarginConverter.");
    }
}

// Usage example in XAML (assuming the converter is defined as a resource):
// <Window.Resources>
//     <local:MarginConverter x:Key="MarginConverter" />
// </Window.Resources>
// <MenuItem Margin="{Binding Path=ActualWidth, RelativeSource={RelativeSource AncestorType={x:Type Window}}, Converter={StaticResource MarginConverter}, ConverterParameter='140,0,180,0'}"/>
