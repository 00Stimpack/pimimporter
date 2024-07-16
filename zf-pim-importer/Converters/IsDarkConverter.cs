using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters
{
    internal class IsDarkConverter : IValueConverter
    {
        
      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DataSave.Option.DarkMode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   

    
}