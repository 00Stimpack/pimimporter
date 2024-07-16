using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ZFPimImporter.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Refresh<T>(this ObservableCollection<T> value)
        {
            CollectionViewSource.GetDefaultView(value).Refresh();
        }
    }
    
}
