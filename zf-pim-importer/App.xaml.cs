using System;
using System.Windows;
using ZFPimImporter.Helpes;

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ZFPimImporter.Notifier;

namespace ZFPimImporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        App()
        {
            Console.WriteLine("STARTUP");
        
            var appPath = Assembly.GetExecutingAssembly().Location;
          
          
            
          
            CommandLineHelper.GetCommandLines();

            this.Deactivated += appDeactivated;
            this.Startup += appStartup;
            this.Exit += appExit;
            this.DispatcherUnhandledException += appDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
            createFileAssociation();

          
        }

    

        protected override void OnStartup(StartupEventArgs e)
        {
      
            base.OnStartup(e);
            //WPF Single Instance Application
     
        }
        
        void appDeactivated(object sender, EventArgs e)
        {
            Console.WriteLine("AppDeactivated");

            //Memory.ReEvaluatedWorkingSet();
        }

      
        

        #region Methods (5)

        // Private Methods (5) 

        private static void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //ExceptionLogger.LogExceptionToFile(e.Exception);
            //LogWindow.AddMessage(LogType.Error, e.Exception.Message);
            e.Handled = true;
        }

        static void appExit(object sender, ExitEventArgs e)
        {
            Console.WriteLine("AppExit");
        }

        void appStartup(object sender, StartupEventArgs e)
        {
        
       
            Console.WriteLine("AppStartup");

        }

  
        private static void createFileAssociation()
        {
            var appPath = Assembly.GetExecutingAssembly().Location;
         
            
            FileAssociationa.CreateFileAssociation(".zfpimi", "Pim", "Pim Data for Applications",
                appPath
            );
            
            FileAssociationa.CreateFileAssociation(".xlsx", "Excel", "Excel Tables for import",
                appPath
            );
        }

        static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            Console.WriteLine("exception:"+(ex));
         //   ExceptionLogger.LogExceptionToFile(ex);
        }

        #endregion Methods
    }
}