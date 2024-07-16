using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.Converters;



public class SubheadlineTemplateSelector4 : DataTemplateSelector
{
        
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SegmentJson segment)
        {
            var element = container as FrameworkElement;

            if (element != null)
            {
                return segment.enabledlanguage ? 
                    element.FindResource("SubheadlineTextDataChangeBox4") as DataTemplate : 
                    element.FindResource("SubheadlineLanguageTextDataChangeBox4") as DataTemplate;
            }
        }

        return null;
    }
}

