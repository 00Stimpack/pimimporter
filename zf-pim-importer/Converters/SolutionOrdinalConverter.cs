using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;




public class SolutionOrdinalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;
        var ordinal = 0;

        if (lvi != null)
        {
            var lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;

            ordinal = lv.ItemContainerGenerator.IndexFromContainer(lvi) + 1;
            lvi.ToolTip = " ";

            var itsTen = false;
            if (ordinal == 1)
            {
                lvi.Name = "ItsFirst";
                itsTen = true;
                return ordinal;
            }

            if (ordinal % 10 == 1)
            {
                lvi.Name = "ItsTen";
                itsTen = true;
            }

            var json = (PimJson)lvi.DataContext;
            if (json == null) return ordinal;

            lvi.ToolTip = $"{json.id}\n{json.en.ProductName}\n{json.en.Subheadline}";

            if (json.path != "-1") return ordinal;
            var name = "ItsEmpty";
            if (itsTen)
                name += "ItsTen";
            lvi.Name = name;
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}
