using System;
using System.Collections.Generic;
using System.IO;
using ZFPimImporter.DataTypes;
using ZFPimImporter.Helpes;
using ZFPimImporter.IO;
using ZFPimImporter.Project;

namespace ZFPimImporter.Project
{
    public class ProjectHandler
    {
        private List<ProjectOverViewData>  _projectDataOverView = new  List<ProjectOverViewData>();


        public static readonly string EXTENSION = ".zfpimi";
        /*public ProjectData CurrentProject
        {
            get
            {
                return _currentProject;
            }
            set
            {
                _currentProject = value;
            }
        }

        private ProjectData _currentProject { get; set; } = new ProjectData();*/
      
        /*public ProjectHandler( ) { }
  
        public ProjectHandler(string path)
        {
            LoadProject(path);
        }

        public ProjectHandler( ProjectData project)
        {
            
        }




        public void SaveProject(ProjectData project)
        {
            
        }
        
        
                public void SaveProject(string name, ProjectData project)
        {
            CurrentProject = project;
            if(string.IsNullOrEmpty( project.ProjectID) )
                project.ProjectID =  Guid.NewGuid().ToString();
            
            var segmentPath  = Directory.GetParent(name).FullName;
            if (!Directory.Exists(segmentPath))
                Directory.CreateDirectory(segmentPath);
 
            if (!File.Exists(name))
                File.Delete(name);

            project.Arguments.Command = CommandLineHelper.Commands.CheckData;
            Serializer.WriteBin(name,project);
            Console.WriteLine("saved with command:"+project.Arguments.Command);
            CurrentProject = project;
        }


        public void LoadProject (string path)
        {
         
            if (!File.Exists(path) )
            {
                Console.WriteLine("couldnt find file");
                return;
            }
            if (!path.EndsWith(EXTENSION))
            {
                Console.WriteLine("not right extension");
                return;
            }
            Console.WriteLine("GOGOGO");
            CurrentProject = Serializer.LoadBin<ProjectData>(path);
        
        }

        public ProjectHandler(string name,ProjectData projectdata)
        {
            
            SaveProject(name, projectdata);
            
        }*/
    }
}