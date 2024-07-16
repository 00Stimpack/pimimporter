using System;
using System.Globalization;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;


namespace ZFPimImporter.Converters;

public class ProductConfidentialConverter
{

    private static PackIconKind StandardOn = PackIconKind.Abacus;
    private static PackIconKind StandardOnActive = PackIconKind.About;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;

        if (lvi != null)
        {
            //var lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;

            var json = (PimJson)lvi.DataContext;

            if (json != null)
                return PackIconKind.TextBoxPlus;
        }

        return PackIconKind.None;
    }
    
    public object Convert23(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;
        if (lvi != null)
        {

            var json = (PimJson)lvi.DataContext;

            if (json != null)
            {
                if (json.is_confidential)
                {
                    if (DataSave.CurrentProject.Option.AllConfidentialActive)
                    {
                        return StandardOnActive;
                    }
                    return StandardOn;
                }
            }
        }
        return PackIconKind.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}