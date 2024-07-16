using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
//using System.Threading;
using ZFPimImporter.DataTypes;
namespace ZFPimImporter.IO
{
    public static class PathGenerator
    {


        
        public static void RemoveSegement(string segments)
        {
            //DeleteAll(Path.Combine(FixedStrings.APPPATH,CleanSegment(segments)));
        }    
        
   

        public static string GetSegmentJsonPath(string segment)
        {
            return Path.Combine(Path.Combine(FixedStrings.DATAPATH,"segments"), CleanSegment(segment)+".json");
        }
        
        public static string GetSegmentJsonPath(string targetPath,string segment)
        {
            return Path.Combine(Path.Combine(targetPath,"segments"), CleanSegment(segment)+".json");
        }
        public static string GetProductJsonPath(string path)
        {
            return Path.Combine(path, "products.json");
        }
        public static string GetProductJsonPath()
        {
           return GetProductJsonPath( FixedStrings.DATAPATH);
        }

        
        public static string GetLanguageJsonPath(string path)
        {
            return  Path.Combine(path, "languages.json");
        }
        public static string GetLanguageJsonPath()
        {
            return GetLanguageJsonPath( FixedStrings.DATAPATH);
        }
        
        public static void OverriderSegmentJson(string dataPath,List<PimJson> data)
        {
            if (!Tabs.isLoaded) return;
            FileInfo segmentJson = new FileInfo(Path.Combine(FixedStrings.DATAPATH,"segments",dataPath + ".json"));
            JsonHandler.WriteJson(segmentJson.FullName,data);
        }
        
        
        public static void OverriderLangJson(List<LanguageDataTools.LanguageData> data)
        {
         
            FileInfo segmentJson = new FileInfo(Path.Combine(FixedStrings.DATAPATH,"languages.json"));
            JsonHandler.WriteJson(segmentJson.FullName,data);
        }

        public static void SaveOptions()
        {
            SaveOptions(DataSave.CurrentProject.Option);
            //SaveImprterOptions(DataSave.CurrentProject.OptionImporter);

        }

        public static void SaveOptions(Option option)
        {
            SaveOptions(FixedStrings.DATAPATH, option);
        }
        public static void SaveOptions(string datapath,Option option)
        {
          
            if (!Tabs.isLoaded) return;
            string pathToOptionsJson = Path.Combine(datapath, "options.json");
            JsonHandler.WriteJson(pathToOptionsJson,option);
        }
        
        /*
        public static void SaveImprterOptions(string projectpath, OptionImporter option)
        {
            return;
            if (!Tabs.isLoaded) return;
            string pathToOptionsJson = Path.Combine(projectpath, "importer_options.json");
            JsonHandler.WriteJson(pathToOptionsJson,option);
        }
        public static void SaveImprterOptions(OptionImporter option)
        {
            SaveImprterOptions(FixedStrings.PROJECTPATH, option);

        }
        */
        
        
 
        public static void SwitchSegmentLanguage(LanguageType type, string dataPath = "")
        {
            try
            {
                var langdata = DataSave.CurrentProject.LanguageData[(int)type];
                if (string.IsNullOrEmpty(dataPath))
                    dataPath = FixedStrings.DATAPATH;
                List<string> segmentPathes = new List<string>(){dataPath};
                for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
                {
                   // segmentPathes.Add( Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH,FixedStrings.ApplicationIDs[i].ToString()));
                }

                Console.WriteLine("TODO NEED TO CHANGE HERE ASWELL!!!!!!!!!:" );
                for (int k = 0; k < segmentPathes.Count; k++)
                {
                    
                    string[] fileEntries = Directory.GetFiles(Path.Combine(segmentPathes[k], "segments"), "*.json");
                    Console.WriteLine("foundCOunt:" + fileEntries.Length + " path:"+Path.Combine(segmentPathes[k], "segments"));
                    for (int i = 0; i < fileEntries.Length; i++)
                    {
              
                        var jsons = JsonHandler.LoadJsonaa<List<PimJson>>(fileEntries[i]);
                        if (jsons == default)
                        {
                            jsons = new List<PimJson>();
                         
                        }
                        
                        Console.WriteLine("jsons.Count:"+jsons.Count);
                        for (int j = 0; j < jsons.Count; j++)
                        {

                            if (langdata.Data.TryGetValue(jsons[j].id, out PimData pim))
                            {
                                Console.WriteLine("Subheadline:"+pim.Subheadline);
                                jsons[j].de = pim;
                                //jsons[j].en = pim;
                            }
                            else
                            {
                                jsons[j].de = jsons[j].en;
                            }
                        }

                        try
                        {
                            JsonHandler.WriteJson(fileEntries[i],jsons);
                      

                        }
                        catch (Exception e)
                        {
                            // ignored
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
      
            
          
        }

        public static void RefreshSegmentDatass(List<PimJson> newData, string dataPath = "")
        {
            try
            {
                if (string.IsNullOrEmpty(dataPath))
                    dataPath = FixedStrings.DATAPATH;
                
                Console.WriteLine("dataPath:"+dataPath);
                List<string> segmentPathes = new List<string>(){dataPath};
                /*for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
                {
                    segmentPathes.Add( Path.Combine(FixedStrings.APPLICATIONSEGMENTPATH,FixedStrings.ApplicationIDs[i].ToString()));
                }*/

                Console.WriteLine("TODO NEED TO CHANGE HERE ASWELL!!!!!!!!!:" );

                for (int k = 0; k < segmentPathes.Count; k++)
                {
                    string [] fileEntries = Directory.GetFiles(Path.Combine(segmentPathes[k],"segments"),"*.json");
                    Console.WriteLine("RefreshSegmentData foundCOunt:" + fileEntries.Length + " path:"+Path.Combine(segmentPathes[k], "segments"));

                    for (int i = 0; i < fileEntries.Length; i++)
                    {
                        var jsons = JsonHandler.LoadJsonaa<List<PimJson>>(fileEntries[i]);
                        if (jsons == default)
                        {
                            jsons = new List<PimJson>();
                         
                        }
                        
                        Console.WriteLine("jsons.Count:"+jsons.Count);

                        for (int j = 0; j < jsons.Count; j++)
                        {
                            var found = newData.Find(x => x.id == jsons[j].id);
                            if(found != null)
                            {
                                jsons[j] = found;
                            }
                        }

                        try
                        {
                            JsonHandler.WriteJson(fileEntries[i],jsons);

                        }
                        catch (Exception e)
                        {
                            // ignored
                        }
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine($@"RefreshSegmentDataEXCEPTION:{e}");
            }
         
          
        }

        
        public static List<PimJson> RefreshSegmentJson(string root,string dataPath)
        {
            List<PimJson> jsons = new List<PimJson>();
            FileInfo segmentJson = new FileInfo(Path.Combine(root,"segments",dataPath + ".json"));
            
            if (segmentJson.Exists)
            {
                /*jsons = JsonHandler.LoadJsonaa<List<PimJson>>(segmentJson.FullName);
                switch (jsons)
                {
                    case null:
                        jsons = new List<PimJson>();
                        JsonHandler.WriteJson(segmentJson.FullName,jsons);
                        break;
                    
                }*/
            }
            else
            {
                if (!Directory.Exists(segmentJson.DirectoryName))
                    Directory.CreateDirectory(segmentJson.DirectoryName);
                JsonHandler.WriteJson(segmentJson.FullName,jsons);
            }

            return jsons;
        }

        public static List<PimJson> RefreshSegmentJson(string dataPath)
        {
            if (!Tabs.isLoaded) new List<PimJson>();
           return RefreshSegmentJson(FixedStrings.DATAPATH, dataPath);
        }

        static string pattern = @"[\\/:*?""<>|]";

        public static void CreateStandardPath()
        {
            if (!Directory.Exists(FixedStrings.PROJECTPATH))
                Directory.CreateDirectory(FixedStrings.PROJECTPATH);
            
            if (!Directory.Exists(FixedStrings.PUBLISHDATAPATH))
                Directory.CreateDirectory(FixedStrings.PUBLISHDATAPATH);
            
            if (!Directory.Exists(FixedStrings.SAVEDPROJECTS))
                Directory.CreateDirectory(FixedStrings.SAVEDPROJECTS);
            if (!Directory.Exists(FixedStrings.LOGSDATAPATH))
                Directory.CreateDirectory(FixedStrings.LOGSDATAPATH);         
            if (!Directory.Exists(FixedStrings.DATAPATH))
                Directory.CreateDirectory(FixedStrings.DATAPATH);
            if (!Directory.Exists(Path.Combine(FixedStrings.DATAPATH,$"segments")))
                Directory.CreateDirectory(Path.Combine(FixedStrings.DATAPATH,"segments"));
            
            if (!Directory.Exists(FixedStrings.EXCELSTORAGE))
                Directory.CreateDirectory(FixedStrings.EXCELSTORAGE);
        
        }

        
        public static string CleanSegment(string segment)
        {
            return segment.Replace(" ", "_").Replace("/", "");
        }
        public static string CleanTopic(string topic)
        {
            return topic.
                   Replace("*","_").
                   Replace("?","_").
                   Replace(":","_").
                   Replace("/","_").
                   Replace("<","_").
                   Replace(">","_").
                   Replace("|","_").
                   Replace(" – ","_").
                   Replace(" - ", "_").
                   Replace(" ", "_");
        }

        internal static void DeleteAll(string pathTo)
        {
            if(!Directory.Exists(pathTo))
                return;
            DirectoryInfo di = new DirectoryInfo(pathTo);
            foreach (FileInfo file in di.GetFiles("*", SearchOption.AllDirectories)) 
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}