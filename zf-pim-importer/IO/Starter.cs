using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZFPimImporter.DataTypes;

namespace ZFPimImporter.IO
{
    public static class Starter
    {

        /*
        private static bool ConvertData(int appId,List<SegmentJson> jsons)
        {
            
            bool needsave = false;
            string pathToSegmentsPath = Path.Combine( FixedStrings.APPLICATIONSEGMENTPATH,appId.ToString(),$"segments");
            var newjson = CreateSegmentDataOld(appId);
            for (int i = 0; i < jsons.Count; i++)
            {
                if (jsons[i].id == -1)
                {
                    var foundjson = newjson.Find(x => x.name == jsons[i].name);
                    if (foundjson != null)
                    {
                        needsave = true;
                        string oldjsonpath = Path.Combine(pathToSegmentsPath, jsons[i].path) + ".json";
                        FileInfo pathToSegmentsJsonPath = new FileInfo(oldjsonpath);
                        if (pathToSegmentsJsonPath.Exists)
                        {
                            string newjsonpath = Path.Combine(pathToSegmentsPath, foundjson.path) + ".json";
                            pathToSegmentsJsonPath.MoveTo(newjsonpath);
                        }
                        jsons[i].id = foundjson.id;
                        jsons[i].path = foundjson.path;
                    }
                }
            }
            if (jsons.Count != newjson.Count)
            {
                for (int i = 0; i < newjson.Count; i++)
                {
                    var foundjson = jsons.Find(x => x.id == newjson[i].id);
                    if (foundjson == null)
                    {
                        needsave = true;
                        jsons.Add(newjson[i]);
                    }
                }
            }

            if (needsave)
            {
                string pathToSegmentsJson = Path.Combine( FixedStrings.APPLICATIONSEGMENTPATH,appId.ToString(),$"segments.json");
                Console.WriteLine($"need to SAVE jsons in application:{appId}");
                JsonHandler.WriteJson(pathToSegmentsJson,jsons);
            }

            return needsave;
        }
        */

        private static List<SegmentJson> CreateSegmentData1()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "Depot", 
                    namelanguage = "Depot", 
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}},
                new SegmentJson()
                {
                    id = 1,
                    name = "City Buses", 
                    namelanguage = "City Buses", 
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = -250}
                },
                new SegmentJson()
                {
                    id = 2,
                    name = "Intercity Buses",
                    namelanguage =  "Intercity Buses", 
                    enabled = true, 
                    subheadlinelanguage  = new []{ "" ,"","" ,"",""},
                    subheadline  = new []{ "" ,"","" ,"",""},
                    translate = new Translate(){x = 480,y = -250}
                },
                new SegmentJson()
                {
                    id = 3,
                    name = "Inner City Rail", 
                    namelanguage =   "Inner City Rail", 
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = 200}
                },
                new SegmentJson()
                {
                    id = 4,
                    name = "Autonomous Shuttles",
                    namelanguage = "Autonomous Shuttles",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 5,
                    name = "Intercity Trains", 
                    namelanguage = "Intercity Trains",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 500,y = 270}
                },
            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }

        
        private static List<SegmentJson> CreateSegmentData2()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                   
                new SegmentJson()
                {
                    id = 0,
                    name = "Automated",
                    namelanguage = "Automated",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}},
                new SegmentJson()
                {
                    id = 1,
                    name = "Connected", 
                    namelanguage = "Connected",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = -250}
                },
                new SegmentJson()
                {
                    id = 2,
                    name = "Electric", 
                    namelanguage = "Electric",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 480,y = -250}
                },
                new SegmentJson()
                {
                    id = 3,
                    name = "Truck", 
                    namelanguage = "Truck",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = 200}
                },
                new SegmentJson()
                {
                    id = 4,
                    name = "Connected Intelligence",
                    namelanguage = "Connected Intelligence",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 5,
                    name = "Trailer",
                    namelanguage = "Trailer",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },

            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }

        
        
        private static List<SegmentJson> CreateSegmentData3()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "Electric Mobility",
                    namelanguage = "Electric Mobility",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}},
                new SegmentJson()
                {
                    id = 1,
                    name = "Automated Driving", 
                    namelanguage = "Automated Driving",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = -250}
                },
                new SegmentJson()
                {
                    id = 2,
                    name = "Vehicle Motion Control",
                    namelanguage = "Vehicle Motion Control",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 480,y = -250}
                },
                new SegmentJson()
                {
                    id = 3,
                    name = "Integrated Safety", 
                    namelanguage = "Integrated Safety",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = 200}
                },
                new SegmentJson()
                {
                    id = 4,
                    name = "Software / Digitalization",
                    namelanguage = "Software / Digitalization",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 5,
                    name = "Sustainability",
                    namelanguage = "Sustainability",

                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 6,
                    name = "Electric Mobility 2",
                    namelanguage = "Electric Mobility 2",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}},
                new SegmentJson()
                {
                    id = 7,
                    name = "Automated Driving 2", 
                    namelanguage = "Automated Driving 2",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = -250}
                },
                new SegmentJson()
                {
                    id = 8,
                    name = "Vehicle Motion Control 2",
                    namelanguage = "Vehicle Motion Control 2",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 480,y = -250}
                },
                new SegmentJson()
                {
                    id = 9,
                    name = "Integrated Safety 2", 
                    namelanguage = "Integrated Safety 2", 
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = 200}
                },
                new SegmentJson()
                {
                    id = 10,
                    name = "Software/Digitalization2",
                    namelanguage = "Software/Digitalization2",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 11,
                    name = "Sustainability 2",
                    namelanguage = "Sustainability 2",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },

            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }


        private static List<SegmentJson> CreateSegmentData4()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "SdV Panel",
                    namelanguage = "SdV Panel",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}
                    
                },
                
          
            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }

        private static List<SegmentJson> CreateSegmentData5()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "Shuttle Panel",
                    namelanguage = "Shuttle Panel",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}
                    
                }
            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }

        private static List<SegmentJson> CreateSegmentData6()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "Decarbonization",
                    namelanguage = "Decarbonization",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}},
                new SegmentJson()
                {
                    id = 1,
                    name = "Intelligent Chassis", 
                    namelanguage = "Intelligent Chassis", 
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = -250}
                },
                new SegmentJson()
                {
                    id = 2,
                    name = "Automation",
                    namelanguage = "Automation",
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 480,y = -250}
                },
                new SegmentJson()
                {
                    id = 3,
                    name = "Digitalization", 
                    namelanguage = "Digitalization", 
                    enabled = true, 
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = -480,y = 200}
                },
                new SegmentJson()
                {
                    id = 4,
                    name = "Truck",
                    namelanguage = "Truck",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 5,
                    name = "Trailer",
                    namelanguage = "Trailer",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
                new SegmentJson()
                {
                    id = 6,
                    name = "Bus",
                    namelanguage = "Bus",
                    enabled = true,
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 50,y = 0}
                },
           
            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }
        private static List<SegmentJson> CreateSegmentData7()
        {
            List<SegmentJson> jsons = new List<SegmentJson>()
            {
                new SegmentJson()
                {
                    id = 0,
                    name = "Autonomous Transport Systems",
                    namelanguage = "Autonomous Transport Systems",
                    enabled = true,  
                    subheadlinelanguage  = new []{"" ,"","" ,"",""},
                    subheadline  = new []{"" ,"","" ,"",""},
                    translate = new Translate(){x = 100,y = 270}}

            };
            for (int i = 0; i < jsons.Count; i++)
            {
                jsons[i].path = PathGenerator.CleanSegment(jsons[i].id.ToString());
            }
            return jsons;
        }
   
        


    
        
        



        public static List<SegmentJson> CreateSegmentDataOld(int screenMode)
        {
            return screenMode switch
            {
                1 => CreateSegmentData1(),
                2 => CreateSegmentData2(),
                3 => CreateSegmentData3(),
                4 => CreateSegmentData4(),
                5 => CreateSegmentData5(),
                6 => CreateSegmentData6(),
                7 => CreateSegmentData7(),
                _ => new List<SegmentJson>()
            };
        }

   
       


        public static void CheckSegments(string datapath,string productPath,List<SegmentJson> segmentJsons)
        {
            CheckPathes(datapath,productPath);
            JsonHandler.WriteJson(Path.Combine(datapath,$"segments.json"),segmentJsons);
        }


        public static void CheckPathes(string data, string product)
        {
            Console.WriteLine($"checkPathes data:{data} product:{product}");
            if (!Directory.Exists(data))
                Directory.CreateDirectory(data);
            if (!Directory.Exists(Path.Combine(data,$"segments")))
                Directory.CreateDirectory(Path.Combine(data,"segments"));
            
            if (!Directory.Exists(product))
                Directory.CreateDirectory(product);
        }

      

        public static void CheckApplications(string dataPath = "")
        {
       
            bool needSave = false;
            bool needSaveSegments = false;

            for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
            {
                int appID = FixedStrings.ApplicationIDs[i];
                if (!DataSave.CurrentProject.AllSegments.TryGetValue(appID, out var project))
                {
                    DataSave.CurrentProject.AllSegments[appID] = new List<SegmentJson>();
                }
                DataSave.CurrentProject.AllSegments[appID] ??= new List<SegmentJson>();
                
                if (DataSave.CurrentProject.AllSegments[appID].Count == 0)
                {
                    DataSave.CurrentProject.AllSegments[appID] =  CreateSegmentDataOld(appID);
                    needSave = true;
           
                }
     
                
               
                if(!DataSave.CurrentProject.OptionImporter.RefreshPimsIds.TryGetValue(appID,  out var refreshids))
                {
                    DataSave.CurrentProject.OptionImporter.RefreshPimsIds.Add(appID, new int[]{});
                    DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID] = new string[DataSave.CurrentProject.AllSegments[appID].Count];
                    for (int j = 0; j < DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID].Length; j++)
                    {
                        DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID][j] = "select RefreshTopic";
                    }
                    needSave = true;
                }
           
                
                /*if (DataSave.CurrentProject.AllSegments[appID].Count >  DataSave.CurrentProject.OptionImporter.RefreshPimsIds[appID].Length)
                {
                
                    DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID] = new string[DataSave.CurrentProject.AllSegments[appID].Count];
                    for (int j = 0; j < DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID].Length; j++)
                    {
                        DataSave.CurrentProject.OptionImporter.RefreshPimsFull[appID][j] = "select RefreshTopic";
                    }
                    needSave = true;
                }*/
                
                /*if (DataSave.CurrentProject.AllSegments[appID].Count > DataSave.CurrentProject.OptionImporter.RefreshPimsIds[appID].Length)
                {
                    DataSave.CurrentProject.OptionImporter.RefreshPimsIds[appID] = new int[DataSave.CurrentProject.AllSegments[appID].Count];
                    needSave = true;
                }*/

                /*if (!DataSave.CurrentProject.OptionImporter.ChoosenSegments.TryGetValue(appID, out var test))
                {
                    DataSave.CurrentProject.OptionImporter.ChoosenSegments[appID] = "";
                    needSave = true;
                }*/
            }



            if (needSaveSegments)
            {
                
            }

            if (needSave)
            {

                // DataSave.CurrentProject.AllSegments[appID]
                JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH, "options.json"),DataSave.CurrentProject.OptionImporter);

                /*string pathToOptionsJson = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"projects"), "importer_options.json");
                PathGenerator.SaveAllJson();*/
                
            }
        }

        public static void ResetAllData()
        {
      
            
        
           
            
            //JsonHandler.WriteJson(Path.Combine(FixedStrings.DATAPATH,$"segments.json"), CreateSegmentDataOld(DataSave.CurrentProject.Option.StartScreenMode));

            
            string dataPath = "";//FixedStrings.APPLICATIONSEGMENTPATH;
            
  
            bool needSave = false;
          
            for (int i = 0; i < FixedStrings.ApplicationIDs.Length; i++)
            {
                
                /*string pathToSegmentsJson = Path.Combine(dataPath,FixedStrings.ApplicationIDs[i].ToString(),$"segments.json");
                string pathToSegmentsPath = Path.Combine(dataPath,FixedStrings.ApplicationIDs[i].ToString(),$"segments");
                
                //PathGenerator.RefreshSegmentJson(FixedStrings.DATAPATH, dataPath);
             
                //Console.WriteLine($"dataPath:{pathToSegmentsJson}");
                DirectoryInfo pathToSegmentsPathInfo = new DirectoryInfo(pathToSegmentsPath);*/

                //FileInfo jsonFileInfo = new FileInfo(pathToSegmentsJson);
                /*List<SegmentJson> jsons = new List<SegmentJson>();
                if (!Directory.Exists(Path.Combine(dataPath,FixedStrings.ApplicationIDs[i].ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(dataPath, FixedStrings.ApplicationIDs[i].ToString()));
                }
                    
                Console.WriteLine("CREATING SEGMENT:"+jsonFileInfo.FullName);
                jsons = CreateSegmentDataOld(FixedStrings.ApplicationIDs[i]);
                JsonHandler.WriteJson(jsonFileInfo.FullName,jsons);
                
                if (pathToSegmentsPathInfo.Exists)
                {
                    PathGenerator.DeleteAll(pathToSegmentsPathInfo.ToString());
                }
                pathToSegmentsPathInfo.Create();
                for (int j = 0; j < jsons.Count; j++)
                {
                    FileInfo segmentFileInfo = new FileInfo(pathToSegmentsJson);
                    List<SegmentJson> app_jsons = new List<SegmentJson>();
                    app_jsons = CreateSegmentDataOld(FixedStrings.ApplicationIDs[i]);
                    JsonHandler.WriteJson(pathToSegmentsJson,jsons);
                }*/
           
                if(!DataSave.CurrentProject.OptionImporter.RefreshPimsIds.TryGetValue(FixedStrings.ApplicationIDs[i],  out var refreshids))
                {
                    DataSave.CurrentProject.OptionImporter.RefreshPimsIds.Add(FixedStrings.ApplicationIDs[i], new int[]{});
                 
                }
 
            }

    
           // string pathToOptionsJson = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"projects"), "importer_options.json");
          //  JsonHandler.WriteJson(pathToOptionsJson,DataSave.CurrentProject.OptionImporter);


        }
        
        public static List<SegmentJson> CheckSegments(int id)
        {
            
            List<SegmentJson> jsons = new List<SegmentJson>();
            if (DataSave.CurrentProject.AllSegments.TryGetValue(id, out var segments))
            {
                return segments;
            }
          
            
      
            return jsons;

        }
        public static List<SegmentJson> CheckSegmentsss()
        {
            
            string pathToSegmentsJson = Path.Combine(FixedStrings.DATAPATH,$"segments.json");
            List<SegmentJson> jsons = new List<SegmentJson>();
            if (File.Exists(pathToSegmentsJson))
            {
                jsons = JsonHandler.LoadJsonaa<List<SegmentJson>>(pathToSegmentsJson);
                if (jsons == null || jsons == default)
                {
                    jsons = new List<SegmentJson>();
                    JsonHandler.WriteJson(pathToSegmentsJson,jsons);
                }
                
            }
            else
            {
                jsons = CreateSegmentDataOld(DataSave.CurrentProject.Option.StartScreenMode);
                JsonHandler.WriteJson(pathToSegmentsJson,jsons);
            }

            
      
            return jsons;

        }

    }
}