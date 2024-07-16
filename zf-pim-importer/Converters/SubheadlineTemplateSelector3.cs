using System;
using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;

public class SubheadlineTemplateSelector3 : DataTemplateSelector
{
        
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SegmentJson segment)
        {
            var element = container as FrameworkElement;

            
            if (element != null)
            {
                //element.Width = 330;

                return segment.enabledlanguage ? 
                    element.FindResource("SubheadlineTextDataChangeBox3") as DataTemplate : 
                    element.FindResource("SubheadlineLanguageTextDataChangeBox3") as DataTemplate;
            }
        }

        return null;
    }
}
