using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;

namespace ZFPimImporter.Views
{
    public partial class OptionsPage : UserControl
    {
     
        public OptionsPage()
        {
            InitializeComponent();
            SetOptionSlider();
            //Tabs.OptionsLoaded = true;

        }
        
        
        
        private void SetOptionSlider()
        {
            //FadeEnterSlider.Value = Option.FadeEnterTime;
          //  FadeLeaveSlider.Value = Option.FadeLeaveTime;
          //  ZoomInSlider.Value = Option.ZoomInTime;
           // ZoomOutSlider.Value = Option.ZoomOutTime;


        }
        /*private void FadeEnterTime_OnSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!OptionsLoaded)
                return;
            Option.FadeEnterTime =(float) e.NewValue;
            PathGenerator.SaveOptions(Option);
        }
   
        private void ZoomInTime_OnSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!OptionsLoaded)
                return;
            Option.ZoomInTime =(float) e.NewValue;
            PathGenerator.SaveOptions(Option);
        }
        
        private void ZoomOutTime_OnSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!OptionsLoaded)
                return;
            Option.ZoomOutTime =(float) e.NewValue;
            PathGenerator.SaveOptions(Option);
        }

        private void FadeLeaveTime_OnSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!OptionsLoaded)
                return;
            Option.FadeLeaveTime =(float) e.NewValue;
            PathGenerator.SaveOptions(Option);
        }*/
    }
}