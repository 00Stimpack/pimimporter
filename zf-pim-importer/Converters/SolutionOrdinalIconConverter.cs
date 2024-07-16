using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters;


public class SolutionOrdinalIconConverter : IValueConverter
{
    public bool CheckIfIdIsInSolutions(int id)
    {
        for (var i = 0; i < Tabs.Solutions.Count; i++)
            if (Tabs.Solutions[i].id == id)
                return true;
        return false;
    }

    private static PackIconKind StandardOn = PackIconKind.BookLock;
    private static PackIconKind StandardOnActive = PackIconKind.BookLockOutline;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var lvi = value as ListViewItem;

        if (lvi != null)
        {
            //var lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;
            if(parameter != null)
            {
                if (int.TryParse(parameter as string, out var foundType))
                {
                    switch (foundType)
                    {
                        case 0:
                            var json = (PimJson)lvi.DataContext;
                            
                            if (json != null)
                            {
                                if (json.is_confidential)
                                {
                                    if ( DataSave.CurrentProject.Option.AllConfidentialActive)
                                    {
                                        return StandardOn;
                                    }
                                    return StandardOnActive;
                                }


                                return PackIconKind.None;

                            }
                            
                            if (json != null)
                                if (CheckIfIdIsInSolutions(json.id))
                                    return PackIconKind.Abacus;
                            return PackIconKind.None;
                        case 1:
                            return PackIconKind.None;
                        default:
                            return PackIconKind.None;
                    }
                }
            }
            
            var jsonmore = (PimJson)lvi.DataContext;
            if (jsonmore != null)
                if (DataSave.CurrentProject.Option.PresentationPimIds.Contains(jsonmore.id))
                    return PackIconKind.Airplay;
        }

        return PackIconKind.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}