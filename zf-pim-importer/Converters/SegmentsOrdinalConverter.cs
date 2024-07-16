using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;

public class SegmentsOrdinalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;
        var ordinal = 0;

        if (lvi != null)
        {
            var lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;

            ordinal = lv.ItemContainerGenerator.IndexFromContainer(lvi) + 1;

            var json = (PimJson)lvi.DataContext;

            lvi.ToolTip = $"{json.id}\n{json.en.ProductName}\n{json.en.Subheadline}";
        }

        return ordinal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}