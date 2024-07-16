using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Converters;

public class NameTemplateSelector : DataTemplateSelector
{
        
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SegmentJson segment)
        {
            var element = container as FrameworkElement;

            if (element != null)
            {
                var templateNew = segment.enabledlanguage ? element.FindResource("NameLanguageTextDataChangeBox") as DataTemplate : element.FindResource("NameTextDataChangeBox") as DataTemplate;
                return templateNew;
                
                if (segment.id == DataSave.CurrentProject.OptionImporter.SegmentsID)
                {
                    var template = segment.enabledlanguage ? element.FindResource("NameLanguageTextDataChangeBoxSelected") as DataTemplate : element.FindResource("NameTextDataChangeBoxSelected") as DataTemplate;
                    return template;
                }
                else
                {
                    var template = segment.enabledlanguage ? element.FindResource("NameLanguageTextDataChangeBox") as DataTemplate : element.FindResource("NameTextDataChangeBox") as DataTemplate;
                    return template;
                }
                    
            }
        }

        return null;
    }
}