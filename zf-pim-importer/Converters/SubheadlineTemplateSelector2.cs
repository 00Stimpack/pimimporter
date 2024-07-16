using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;

public class SubheadlineTemplateSelector2 : DataTemplateSelector
{
        
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SegmentJson segment)
        {
            var element = container as FrameworkElement;

            if (element != null)
            {
                return segment.enabledlanguage ? 
                    element.FindResource("SubheadlineTextDataChangeBox2") as DataTemplate : 
                    element.FindResource("SubheadlineLanguageTextDataChangeBox2") as DataTemplate;
            }
        }

        return null;
    }
}