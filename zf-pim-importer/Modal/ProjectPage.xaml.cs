using System;
using System.Windows;
using ZFPimImporter.Helpes;
using ZFPimImporter.Project;

namespace ZFPimImporter.Modal
{
    public partial class ProjectPage : Window
    {
        private static ProjectHandler _projectHandler = new ProjectHandler();

        private Tabs _tabs;
        
        public static ProjectHandler ProjectHandler
        {
            get
            {
                return _projectHandler;
            }
            set
            {
                _projectHandler = value;
            }
        }

        public static CommandLineHelper.Commands _command;
  
        public static CommandLineHelper.Commands Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
            }
        }
        
        public ProjectPage()
        {
            InitializeComponent();
        }
        public ProjectPage(Tabs tabs)
        {
            _tabs = tabs;
            InitializeComponent();
        }

        private void BtnAbortData_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSaveData_OnClick(object sender, RoutedEventArgs e)
        {
           
        }
    }

    
}