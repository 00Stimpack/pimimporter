using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters;


public class SegmentsOrdinalIconConverter : IValueConverter
{
    public bool CheckIfIdIsInSolutions(int id)
    {
        for (var i = 0; i < Tabs.Solutions.Count; i++)
            if (Tabs.Solutions[i].id == id)
                return true;
        return false;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        switch (value)
        {
            case ListViewItem lvi :
                break;
            case DockPanel :
                break;
            
            case StackPanel lviStack:
                var pathSelected = (SegmentJson)lviStack.DataContext;
                if(pathSelected == null)return PackIconKind.None;
                if (DataSave.CurrentProject.OptionImporter.SegmentsID == pathSelected.id)
                {
                    return PackIconKind.Check;

                }
                return PackIconKind.None;

            case DataTemplate :

                break;
            case DataGridTemplateColumn :

                break;
            case GridViewColumn :

                break;
        }
        
 

        return PackIconKind.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}