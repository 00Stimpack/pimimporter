using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ZFPimImporter.DataTypes;
using ZFPimImporter.IO;
using ZFPimImporter.Modal;
using ZFPimImporter.Project;

namespace ZFPimImporter.Helpes
{
    
    
    public class CommandLineHelper
    {
       
    
        public enum Commands
        {
            Nothing,
            UpdateData,
            StartApplication,
            UpdateAndStart,
            OpenImporter,
            UpdateAll,
            CheckData,
            Install,
            
        }

        public static string ProjectPathes = ".\\distribute\\";
        public static string InstallPath = ".\\installpath.txt";

        public static List<(string, string)> PathesToUpdate = new List<(string, string)>();
        public static List<string> PathesToUpdateFiles = new List<string>();

        public static string SetWorkingDir()
        {

            string path = "";
            try
            {
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
         
                path = System.IO.Path.GetDirectoryName(strExeFilePath);
                Directory.SetCurrentDirectory(path);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The specified directory does not exist. {0}", e);
            }
            return path;
        }

        public static void LoadProjectFile(string pathto)
        {
            DataSave.LoadProject(pathto);
            
            //ProjectHandler projectHandler = new ProjectHandler(pathto);
            //ProjectPage.ProjectHandler = projectHandler;
            
        }

        public static CommandLineArguments CommandLineArguments = new CommandLineArguments()
        {

            Path = "",
        };

        public static void UpdateAll(string path)
        {
            for (int i = 0; i < PathesToUpdate.Count; i++)
            {
              
                LoadProjectFile(path);
            }
        }
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public static void CheckUpdaten()
        {
            FileInfo dirInfo = new FileInfo(Path.Combine(ProjectPathes,InstallPath));
            if (!dirInfo.Exists)
            {
                Console.WriteLine("no InstallPath txt:"+ProjectPathes);
                return;
            }

            const Int32 BufferSize = 128;
            
            DirectoryInfo install = new DirectoryInfo(Path.Combine(ProjectPathes,"install"));

            using ( var fileStream = File.OpenRead(dirInfo.ToString()))
            {

                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    
                    while (streamReader.ReadLine()  is { } line)
                    {
                        FileInfo targetpath = new FileInfo(line);
                        string topath = targetpath.Directory.FullName.ToString();
                        string name = targetpath.Name.Substring(0, targetpath.Name.Length - targetpath.Name.Length);
                        if (!Directory.Exists(topath))
                        {
                            Console.WriteLine("target path not found:"+topath.ToString());
                            continue;
                        }

                        FileInfo projectFile = new FileInfo(Path.Combine(ProjectPathes, targetpath.Name));
                        if (!projectFile.Exists)
                        {
                            Console.WriteLine("projectFile path not found:"+projectFile.ToString());
                            continue;
                        }
                
                        LoadProjectFile(projectFile.FullName.ToString());
                        DataSave.CurrentProjectToJson(topath);
              
                        Console.WriteLine("update from;"+install.ToString() + " to:"+targetpath.Directory.ToString());
             


                    }
                }
                
            }
       
       

            
        }

        public static void CheckVersion()
        {   
            List<string> pathes = new List<string>();
            FileInfo dirInfo = new FileInfo(Path.Combine(ProjectPathes,InstallPath));
            if (!dirInfo.Exists)
            {
                Console.WriteLine("no InstallPath txt:"+ProjectPathes);
                return;
            }
            
            DirectoryInfo install = new DirectoryInfo(Path.Combine(ProjectPathes,"install"));
            if (!install.Exists)
            {
                Console.WriteLine("no install found:"+install.ToString());
                return;
            }
            
            const Int32 BufferSize = 128;


            using ( var fileStream = File.OpenRead(dirInfo.ToString()))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    while (streamReader.ReadLine() is { } line)
                    {
                        FileInfo targetpath = new FileInfo(line);
                        string topath = targetpath.Directory.Parent.FullName.ToString();
                        string name = targetpath.Name.Substring(0, targetpath.Name.Length - targetpath.Name.Length);
                        if (!Directory.Exists(topath))
                        {
                            Console.WriteLine("target path not found:"+topath.ToString());
                            continue;
                        }
                        Console.WriteLine("copy from;"+install.ToString() + " to:"+targetpath.Directory.ToString());
                        CopyFilesRecursively(install.ToString(), targetpath.Directory.ToString());


                    }
                    // Process line


                }
            }
  
            


           
        }
        
        
        public static   List<string> CheckUpdateFile(string path)
        {   
            List<string> pathes = new List<string>();
            FileInfo dirInfo = new FileInfo(path);
            if (!dirInfo.Exists) return pathes;
            

         
            const Int32 BufferSize = 128;
         
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {
                String line;
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    FileInfo targetpath = new FileInfo(line);
                    string topath = targetpath.Directory.ToString();
                    string name = targetpath.Name.Substring(0, targetpath.Name.Length - targetpath.Name.Length);
                    if (!Directory.Exists(topath)) return pathes;
                    FileInfo testfile = new FileInfo(Path.Combine(ProjectPathes, targetpath.Name));
                    if (!testfile.Exists)continue;
                    PathesToUpdate.Add((path, testfile.ToString()));
                    
                }
                // Process line
            }

            
            
            return pathes;
        }

        public static bool ProjectLoaded = false;
        public static void CheckProjectFile(string path)
        {
       
            Console.WriteLine("CHHECKING:"+path);
            FileInfo dirInfo = new FileInfo(path);
        
            if (!dirInfo.Exists) return;
            if (!path.EndsWith(ProjectHandler.EXTENSION) && !path.EndsWith(Path.GetFileName(FixedStrings.STORAGEPATH)))
            {
                Console.WriteLine("not right extension:"+path);
                return;
            }
            SetWorkingDir();
            Console.WriteLine(path);
            LoadProjectFile(path);
            string topath = dirInfo.Directory.FullName.ToString();
            DataSave.CurrentProjectToJson(topath);
            ProjectPage.Command = Commands.CheckData;
            ProjectLoaded = true;
        }
        
        
        public  static void GetCommandLines()
        {
                

            

            
             
                Dictionary<string, string> CommandLineArgumentsDict = new Dictionary<string, string>()
                {
                    {"--path","asd"},
                    {"--command","asd"},
                    {"--install","asad"},
                    {"--updatefile","asd"},
                };
  
                
                string[] args = System.Environment.GetCommandLineArgs ();

          
                var argtemp = args.ToList();
                argtemp.Add(" ");
                if(args.Length > 1)
                    CheckProjectFile(args[1]);

                for (int i = 0; i < args.Length; i++) 
                {
                  
                 
                    Console.WriteLine("ARGSSSS:"+args[i]);
                    if (CommandLineArgumentsDict.TryGetValue(args[i], out var argument))
                    {
                        
                        Console.WriteLine(args[i+1]);
                        switch (args[i])
                        {
                            case "--command":
                                Console.WriteLine(args[i+1]);
                                switch (@args[i+1])
                                {
                                    case "start" :
                                        CommandLineArguments.Command = Commands.StartApplication;
                                        break;
                                    case "open" :
                                        CommandLineArguments.Command = Commands.OpenImporter;
                                        break;
                                    case "check" :
                                        CommandLineArguments.Command = Commands.CheckData;
                                        break;
                                    case "update" :
                                        CommandLineArguments.Command = Commands.UpdateAll;
                                        //CheckUpdaten();
                                        //PathesToUpdateFiles.AddRange(CheckUpdateFile(args[i+1]));
                                        
                                        //System.Windows.Application.Current.Shutdown();   
                                        break;
                                    case "install" :
                                        Console.WriteLine("INSTALL AS COMMAND");
                                        /*CheckVersion();
                                        System.Windows.Application.Current.Shutdown();  */ 
                                        CommandLineArguments.Command = Commands.Install;
                                        break;
                                }
                                break;
                            case "--path":
                                if (ProjectLoaded) break;
                                CheckProjectFile(args[i+1]);
                            
                                break;
                            case "--updatefile":
                                if (ProjectLoaded) break;
                               // PathesToUpdateFiles.AddRange(CheckUpdateFile(args[i+1]));
                                break;
                        }
                    }
                }

                if (CommandLineArguments.Command == Commands.UpdateAll)
                {
                    
                }
                else if (CommandLineArguments.Command == Commands.Install)
                {
                  //  CheckVersion();
                }
                
        }
    }
}