using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZFPimImporter.DataTypes;
using ZFPimImporter.Modal;
using ZFPimImporter.Project;

namespace ZFPimImporter.IO
{
    public class DataSave
    {
        public static readonly string EXTENSION = ".zfpimi";

        public static ProjectData CurrentProject
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

        private static ProjectData _currentProject { get; set; } = new ProjectData();
        

        
        public static List<PimJson> Products => _currentProject.Products;
        public static List<SegmentJson> Segments  => _currentProject.Segments;


        public static PimOption Option { get; set; } = new PimOption();

        public static string ProjectName { get; set; } = "Standard";



        private void LoadSolutions( )
        {
            
        }

        private static bool initialised = false;

        public static void InitProject(  )
        {

            if (initialised) return;
            if (File.Exists(Option.LastProjectFile))
            {
                //Console.WriteLine("TRY TO LOAD PROJECTFILE:"+Option.LastProjectFile);
                LoadProject(Option.LastProjectFile);
            }
            else
            {
                _currentProject = new ProjectData();
                CheckProject(_currentProject);

            }

            initialised = true;
        }

        public static void CheckProject( ProjectData data)
        {

            for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
            {
                int appid = FixedStrings.ApplicationIDs[i];
              
                if (!data.AllSegments.TryGetValue(appid, out var segments))
                {
                    data.AllSegments[appid] = Starter.CreateSegmentDataOld(appid);
                }
                
                if (!data.AllSolutions.TryGetValue(appid, out var solutions))
                {
                    data.AllSolutions[appid] = new Dictionary<int, List<PimJson>>();
                }

                for (int j = 0; j < data.AllSegments[appid].Count; j++)
                {
                    int id = data.AllSegments[appid][j].id;
                    if (!data.AllSolutions[appid].TryGetValue(id, out var test))
                    {
                        data.AllSolutions[appid][id] = new List<PimJson>();
                    }
                    
                    continue;
                    if ( data.AllSolutions[appid][id].Count >  DataSave.CurrentProject.OptionImporter.RefreshPimsIds[FixedStrings.ApplicationIDs[i]].Length)
                    {
                
                        DataSave.CurrentProject.OptionImporter.RefreshPimsFull[FixedStrings.ApplicationIDs[i]] = new string[data.AllSolutions[appid][id].Count];
                        for (int q = 0; q < DataSave.CurrentProject.OptionImporter.RefreshPimsFull[FixedStrings.ApplicationIDs[i]].Length; q++)
                        {
                            DataSave.CurrentProject.OptionImporter.RefreshPimsFull[FixedStrings.ApplicationIDs[i]][q] = "select RefreshTopic";
                        }
                    }
                
                    if (data.AllSolutions[appid][id].Count > DataSave.CurrentProject.OptionImporter.RefreshPimsIds[FixedStrings.ApplicationIDs[i]].Length)
                    {
                        DataSave.CurrentProject.OptionImporter.RefreshPimsIds[FixedStrings.ApplicationIDs[i]] = new int[data.AllSolutions[appid][id].Count];
                    }

                    /*if (!DataSave.CurrentProject.OptionImporter.ChoosenSegments.TryGetValue(FixedStrings.ApplicationIDs[i], out var test2))
                    {
                        DataSave.CurrentProject.OptionImporter.ChoosenSegments[FixedStrings.ApplicationIDs[i]] = "";
                    }*/
                    
                }
                
                          
       
                //if(data.AllSegments[i].c)

            }

            /*
            for (int i = 0; i < UPPER; i++)
            {
                
            }*/
        }

        public static void RefreshProjectPath()
        {
            PathGenerator.CreateStandardPath();
            if (!File.Exists(FixedStrings.PIMOPTIONSPATH))
            {
                Option = new PimOption();
                Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,Option);
            }
            else
            {
                try
                {
                   Option = Serializer.LoadBin<PimOption>(FixedStrings.PIMOPTIONSPATH);
                }
                catch (Exception e)
                {
                    Option = new PimOption();
                    Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,Option);
                }
            }
            RefreshTitle();


        }

     

        public static void SaveProject2(string pathToFile)
        {
            
            /*
            ProjectData data = new ProjectData()
            {
                Option = DataSave.CurrentProject.Option, 
                OptionImporter = DataSave.CurrentProject.OptionImporter, 
                Products = Products.ToList(),
                ProjectID = Guid.NewGuid().ToString(), 
                Segments = Segments.ToList(),
                LanguageData = DataSave.CurrentProject.LanguageData.ToList(),
                AllSolutions = new List<List<PimJson>>(){Tabs.Solutions.ToList()}
            };
            */
          
            /*ProjectData data = new ProjectData()
            {
                Option = DataSave.CurrentProject.Option, 
                OptionImporter = DataSave.CurrentProject.OptionImporter, 
                Products = Products.ToList(),
                ProjectID = Path.GetFileNameWithoutExtension(pathToFile), 
                Segments = Segments.ToList(),
                LanguageData = DataSave.CurrentProject.LanguageData.ToList(),
                AllSolutions = CurrentProject.AllSolutions,
                AllSegments = CurrentProject.AllSegments,

            };*/
            SaveProject(pathToFile);
            
            
            

        }
        
        public static void CheckLastProject()
        {
            RefreshProjectPath();

            if (!File.Exists(DataSave. Option.LastProjectFile))
            {
                string filename = Path.GetFileName( Option.LastProjectFile);
                string pathToCheck = Path.Combine(FixedStrings.SAVEDPROJECTS, filename);
                if (File.Exists(pathToCheck))
                {
                    Option.LastProjectFile = pathToCheck;

                }
            }
            
            if (File.Exists( Option.LastProjectFile))
            {
                LoadProject( Option.LastProjectFile);
            }
            else
            {
                
                if (!Directory.Exists(FixedStrings.PROJECTPATH))
                    Directory.CreateDirectory(FixedStrings.PROJECTPATH);
               
                Option.LastProjectFile = FixedStrings.STORAGEPATH;
                if (!File.Exists(  Option.LastProjectFile ))
                {
                    _currentProject = new ProjectData();
                    CheckProject(_currentProject);
                    SaveProject( Option.LastProjectFile);
                    LoadProject( Option.LastProjectFile);
                }
                else
                {
                    LoadProject( Option.LastProjectFile);
                }
            }

            //RefreshTitle();
         
      
            

        }

        public static void RefreshTitle()
        {
            
      
            ProjectName = Path.GetFileNameWithoutExtension(Option.LastProjectFile);
            if ( Option.LastProjectFile != null && Option.LastProjectFile.EndsWith(Path.GetFileName(FixedStrings.STORAGEPATH)))
            {
               //ProjectName = "Standard";
            }
            
        }
        
        private void ValidateAllSolutions(List<List<PimJson>> solutions)
        {
            
            for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
            {
                        
            }
            /*for (int i = 0; i < FixedStrings.ApplicationIDs; i++)
            {
                
            }*/
        }
        
        
        public static void CurrentProjectToJsonBack(string pathRoot = "")
        {
           
        }

        public static void  CurrentProjectToJson(string pathRoot = "")
        {
            PathGenerator.CreateStandardPath();
            string productpath = "";
            string data = "";
         
            

            if (string.IsNullOrEmpty(pathRoot))
            {
                productpath = FixedStrings.PRODUCTPATH;
                data = FixedStrings.DATAPATH;
                
            }
            else
            {
                
                productpath = Path.Combine(pathRoot, "products");
                data = Path.Combine(pathRoot, "data");
                var segment = Path.Combine(pathRoot, "data", "segments");

                if (!Directory.Exists(data))
                    Directory.CreateDirectory(data);
                if (!Directory.Exists(productpath))
                    Directory.CreateDirectory(productpath);
                if (!Directory.Exists(segment))
                    Directory.CreateDirectory(segment);

            }
            var segments = _currentProject.Segments;
            Starter.CheckSegments(data,productpath,segments);
            //Console.WriteLine("CurrentProjectToJson\n");
            
            var products = _currentProject.Products.ToList();
            var solutions = _currentProject.AllSegments.Values.ToList();

            if (!Directory.Exists(Path.Combine(pathRoot, "projects")))
                Directory.CreateDirectory(Path.Combine(pathRoot, "projects"));

            PathGenerator.SaveOptions(data,DataSave.CurrentProject.Option);
            //PathGenerator.SaveImprterOptions(Path.Combine(pathRoot,"projects"),CurrentProject.OptionImporter);
            
            if (segments.Count != solutions.Count)
            {
                //Console.WriteLine($"SEGEMENTCOUNT:{segments.Count} SOLUTIONS:{solutions.Count}  Missmatch!!!");
                return;
            }
            for (int i = 0; i < segments.Count; i++)
            {
                if(!string.IsNullOrEmpty(segments[i].path) && solutions[i].Any())
                    JsonHandler.WriteJson(PathGenerator.GetSegmentJsonPath(data,segments[i].path), solutions[i]);
                //Console.WriteLine($"GetSegmentJsonPath:{ PathGenerator.GetSegmentJsonPath(data,segments[i].path)}");

            }
        
            JsonHandler.WriteJson( Path.Combine(productpath, "products.json"),products);

        }


        


        public static void RecursiveDelete(DirectoryInfo baseDir)
        {

            try
            {
                if (!baseDir.Exists)
                    return;

                foreach (var dir in baseDir.EnumerateDirectories())
                {
                    RecursiveDelete(dir);
                }
                baseDir.Delete(true);
            }
            catch (Exception e)
            {
                //Console.WriteLine("PIMERROR:"+e);
            }
   
        }


        internal static void CopyFolderAndOverwrite(string sourceFolder, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            DirectoryInfo sourceDir = new DirectoryInfo(sourceFolder);
            DirectoryInfo destinationDir = new DirectoryInfo(destinationFolder);

            foreach (FileInfo file in sourceDir.GetFiles())
            {
                ////Console.WriteLine("EXT:"+file.Extension);
                if (file.Extension == ".data")
                {
                    
                }
                string destPath = Path.Combine(destinationDir.FullName, file.Name);
                file.CopyTo(destPath, true); // true means overwrite
            }

            foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
            {
                string destPath = Path.Combine(destinationDir.FullName, subDir.Name);
                CopyFolderAndOverwrite(subDir.FullName, destPath);
            }
        }


        public static void SaveProject(string name, ProjectData project)
        {
        
            var segmentPath  = Directory.GetParent(name).FullName;
            if (!Directory.Exists(segmentPath))
                Directory.CreateDirectory(segmentPath);
 
            if (!File.Exists(name))
                File.Delete(name);
            
            Serializer.WriteBin(name,project);
            //Console.WriteLine("saved with command:"+project.Arguments.Command);
            DataSave.RefreshProjectPath();
            //Option.LastProjectFile = name;
            Serializer.WriteBin(FixedStrings.PIMOPTIONSPATH,Option);
        }

        public static void SaveProject(string name)
        {
    
            SaveProject(name, CurrentProject);
   
        }


        public static void LoadProject (string path)
        {

            //Console.WriteLine("path:"+path);

            try
            {
                if (!File.Exists(path) )
                {
                    //Console.WriteLine("couldnt find file");
                    return;
                }
                if (!path.EndsWith(EXTENSION) && !path.EndsWith(Path.GetFileName(FixedStrings.STORAGEPATH)))
                {
                    //Console.WriteLine("not right extension");
                    return;
                }
                CurrentProject = Serializer.LoadBin<ProjectData>(path);
                DataSave.Option.LastProjectFile = path;
                //Console.WriteLine("GOGOGO SegmentID:"+CurrentProject.OptionImporter.SegmentIndex);

                foreach (var segment in CurrentProject.AllSolutions.Values)
                {
                    foreach (var sets in segment.Values)
                    {
                        foreach (var set in sets)
                        {
                            //Console.WriteLine("set name:"+set.en.ProductName + " path:"+set.path);

                        }

                    }
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine("FAILED LOAD PROJECT:"+e.ToString());
            }

        }

        public static void SwitchApplication(int applicationID,string dataPath = "",string savedPath = "")
        {
            try
            {
                List<SegmentJson> segments = new List<SegmentJson>();
        
                DirectoryInfo saveDir = new DirectoryInfo(Path.Combine(savedPath,applicationID.ToString(), "segments"));
                JsonHandler.WriteJson(Path.Combine(dataPath,"segments.json"),segments);
                
                
                if (string.IsNullOrEmpty(dataPath))
                {
                    dataPath = FixedStrings.DATAPATH;
                }
                
                
                DirectoryInfo dataDir = new DirectoryInfo(Path.Combine(dataPath, "segments"));
                RecursiveDelete(dataDir);
                dataDir.Create();
                

                if (string.IsNullOrEmpty(savedPath))
                {
                    savedPath = FixedStrings.APPLICATIONSEGMENTPATH;
                }



                
                //Console.WriteLine($"SwitchApplication dataDir:{dataDir} saveDir:{saveDir.FullName}");
            
                //Console.WriteLine($"dataDir:{dataDir}");
                
                //TODO NEED TO COPY HERE!!
                //Console.WriteLine($"TODO NEED TO COPY HERE!!");

                
                try
                {
                    //CurrentProject.Option.ChoosenSegment = CurrentProject.OptionImporter.ChoosenSegments[applicationID];
                }
                catch (Exception e)
                {
                    //Console.WriteLine($"SwitchApplication ERROR:{e}");
              
                }
                

            }
            catch (Exception e)
            {
                //Console.WriteLine("SwitchApplication Second ERROR:"+e);
              
            }
         
        }
    }
}