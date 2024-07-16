using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters;


public class SolutionConfidentialConverter : IValueConverter
{
    
    private static PackIconKind StandardOn = PackIconKind.BookLock;
    private static PackIconKind StandardOnActive = PackIconKind.BookLockOutline;
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;

        if (lvi != null)
        {

            var json = (PimJson)lvi.DataContext;

            if (json != null)
            {
                if (DataSave.CurrentProject.Option.AllConfidentialActive)
                {
                    return StandardOnActive;
                }
                return StandardOn;
            }
        }

        return PackIconKind.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}