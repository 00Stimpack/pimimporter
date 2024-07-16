using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ZFPimImporter.DataTypes
{
  
    [Serializable]
    public class Option 
    {
        public float FadeEnterTime { get; set; } = 0.25f;
        public float FadeLeaveTime { get; set; } = 0.25f;
        public float ZoomInTime { get; set; } = 1f;
        public float ZoomOutTime { get; set; } = 1f;
        public int RefreshTimer { get; set; } = 900;
        public bool CustomerMode { get; set; } = false;
        public string ChoosenSegment { get; set; }
        public string SecondLanguage { get; set; } = LanguageType.EN.ToString();
        public int RefreshPimId { get; set; } = 0;
        public bool AllDetailsActive { get; set; } = true;
        public bool AllMoreActive { get; set; } = true;
        public bool AllSpecsActive { get; set; } = true;
        public bool AllImagesActive { get; set; } = true;
        public bool AllPresentationsActive { get; set; } = true;
        public bool AllVideosActive { get; set; } = true;
        public bool SkipStartScreen { get; set; } = false;
        public int StartScreenMode { get; set; } = 3;
        public Tabs.ScreenSaverMode ScreenSaverMode { get; set; } = Tabs.ScreenSaverMode.Disabled;
        public List<int> PresentationPimIds { get; set; } = new List<int>();
        public bool UseConverter { get; set; } = true;
        public int MaxParallelTask { get; set; } = 10;
        public int ResolutionConverter { get; set; } = 300;
        public bool VideoScreenSaverActive { get; set; } = true;
        
        
        public bool AllConfidentialActive { get; set; } = true;
        public string HomeScreenButtonBorderColor { get; set; } = "#00abe7";
        public string BackgroundImage { get; set; } = string.Empty;

        public string Template { get; set; } = "white";

        

        public Option() { }

        // Special constructor for deserialization
       protected Option(SerializationInfo info, StreamingContext context)
        {
            // Deserialize each property from the SerializationInfo
            ZoomInTime = info.GetSingle("ZoomInTime");
            ZoomOutTime = info.GetSingle("ZoomOutTime");
            RefreshTimer = info.GetInt32("RefreshTimer");
            CustomerMode = info.GetBoolean("CustomerMode");
            ChoosenSegment = info.GetString("ChoosenSegment");
            SecondLanguage = info.GetString("SecondLanguage");
            RefreshPimId = info.GetInt32("RefreshPimId");
            AllDetailsActive = info.GetBoolean("AllDetailsActive");
            AllMoreActive = info.GetBoolean("AllMoreActive");
            AllSpecsActive = info.GetBoolean("AllSpecsActive");
            AllImagesActive = info.GetBoolean("AllImagesActive");
            AllPresentationsActive = info.GetBoolean("AllPresentationsActive");
            AllVideosActive = info.GetBoolean("AllVideosActive");
            SkipStartScreen = info.GetBoolean("SkipStartScreen");
            StartScreenMode = info.GetInt32("StartScreenMode");
            ScreenSaverMode = (Tabs.ScreenSaverMode)info.GetValue("ScreenSaverMode", typeof(Tabs.ScreenSaverMode));
            PresentationPimIds = (List<int>)info.GetValue("PresentationPimIds", typeof(List<int>));
            UseConverter = info.GetBoolean("UseConverter");
            MaxParallelTask = info.GetInt32("MaxParallelTask");
            ResolutionConverter = info.GetInt32("ResolutionConverter");
            VideoScreenSaverActive = info.GetBoolean("VideoScreenSaverActive");
            
            try
            {
                FadeEnterTime = info.GetSingle("FadeEnterTime");
            }
            catch (SerializationException)
            {
                FadeEnterTime = 0.25f;
            }
            
            try
            {
                FadeLeaveTime = info.GetSingle("FadeLeaveTime");

            }
            catch (SerializationException)
            {
                FadeLeaveTime = 0.25f;
            }
            
            
            try
            {
                AllConfidentialActive  = info.GetBoolean("AllConfidentialActive");
            }
            catch (SerializationException)
            {
                AllConfidentialActive = false;
            }
            
            try
            {
                HomeScreenButtonBorderColor = info.GetString("HomeScreenButtonBorderColor");
            }
            catch (SerializationException)
            {
                HomeScreenButtonBorderColor = "#00abe7";
            }
            
            try
            {
                BackgroundImage = info.GetString("BackgroundImage");
            }
            catch (SerializationException)
            {
                BackgroundImage =  string.Empty;
            }
            try
            {
                Template = info.GetString("Template");
            }
            catch (SerializationException)
            {
                Template =  "white";
            }
        }

        public virtual  void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Serialize each property into the SerializationInfo
            info.AddValue("FadeEnterTime", FadeEnterTime);
            info.AddValue("FadeLeaveTime", FadeLeaveTime);
            info.AddValue("ZoomInTime", ZoomInTime);
            info.AddValue("ZoomOutTime", ZoomOutTime);
            info.AddValue("RefreshTimer", RefreshTimer);
            info.AddValue("CustomerMode", CustomerMode);
            info.AddValue("ChoosenSegment", ChoosenSegment);
            info.AddValue("SecondLanguage", SecondLanguage);
            info.AddValue("RefreshPimId", RefreshPimId);
            info.AddValue("AllDetailsActive", AllDetailsActive);
            info.AddValue("AllMoreActive", AllMoreActive);
            info.AddValue("AllSpecsActive", AllSpecsActive);
            info.AddValue("AllImagesActive", AllImagesActive);
            info.AddValue("AllPresentationsActive", AllPresentationsActive);
            info.AddValue("AllVideosActive", AllVideosActive);
            info.AddValue("SkipStartScreen", SkipStartScreen);
            info.AddValue("StartScreenMode", StartScreenMode);
            info.AddValue("ScreenSaverMode", ScreenSaverMode);
            info.AddValue("PresentationPimIds", PresentationPimIds);
            info.AddValue("UseConverter", UseConverter);
            info.AddValue("MaxParallelTask", MaxParallelTask);
            info.AddValue("ResolutionConverter", ResolutionConverter);
            info.AddValue("VideoScreenSaverActive", VideoScreenSaverActive);
            
            info.AddValue("AllConfidentialActive", AllConfidentialActive);
            info.AddValue("HomeScreenButtonBorderColor", HomeScreenButtonBorderColor);
            info.AddValue("BackgroundImage", BackgroundImage);
        }
    }


    [Serializable]
    public class PimOption 
    {
        
        public string LastProjectFile { get; set; } = Path.Combine(FixedStrings.STORAGEPATH);
        public string TableName { get; set; } = "Confidential";
        
        public bool ShowSnackbar { get; set; } = true;
        public bool DarkMode { get; set; } = false;
        public bool AutoPublish { get; set; } = false;
        public bool AutoSave { get; set; } = false;
        public bool IsLanguage { get; set; } = false;
        public bool UseExcelStorage { get; set; } = true;
        public bool DevMode { get; set; } = false;


        public string[] ConfidentialGroups { get; set; } = new string[] { "All Customers" , "Selected Customers" , "Internal"};



        // Default constructor
        public PimOption() { }

        // Special constructor for deserialization
        protected PimOption(SerializationInfo info, StreamingContext context)
        {
            LastProjectFile = info.GetString("LastProjectFile");
            TableName = info.GetString("TableName");
            ShowSnackbar = info.GetBoolean("ShowSnackbar");
            DarkMode = info.GetBoolean("DarkMode");
            AutoPublish = info.GetBoolean("AutoPublish");
            AutoSave = info.GetBoolean("AutoSave");
            IsLanguage = info.GetBoolean("IsLanguage");
            UseExcelStorage = info.GetBoolean("UseExcelStorage");
            DevMode = info.GetBoolean("DevMode");

            try
            {
                ConfidentialGroups = (string[])info.GetValue("ConfidentialGroups", typeof(string[]));
            }
            catch (SerializationException)
            {
                ConfidentialGroups = new string[] { "Internal" , "Website" , "Public - not on Website"};
            }

        }

        // Method to serialize data
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LastProjectFile", LastProjectFile);
            info.AddValue("TableName", TableName);
            info.AddValue("ShowSnackbar", ShowSnackbar);
            info.AddValue("DarkMode", DarkMode);
            info.AddValue("AutoPublish", AutoPublish);
            info.AddValue("AutoSave", AutoSave);
            info.AddValue("IsLanguage", IsLanguage);
            info.AddValue("UseExcelStorage", UseExcelStorage);
            info.AddValue("DevMode", DevMode);
            
            info.AddValue("ConfidentialGroups", ConfidentialGroups);

        }
    }

    [Serializable]
    public class OptionImporter 
    {
        public int SegmentsID { get; set; } = 0;
        public int SegmentIndex { get; set; } = 0;
        public Dictionary<int, string[]> RefreshPimsFull { get; set; } = new Dictionary<int, string[]>();
        public Dictionary<int, int[]> RefreshPimsIds { get; set; } = new Dictionary<int, int[]>();
        public bool RefreshPimEnable { get; set; } = false;
        public int RefreshTimer { get; set; } = 900;

        // Default constructor
        public OptionImporter() { }

        // Special constructor for deserialization
        protected OptionImporter(SerializationInfo info, StreamingContext context)
        {
            SegmentsID = info.GetInt32("SegmentsID");
            SegmentIndex = info.GetInt32("SegmentIndex");
            RefreshPimsFull = (Dictionary<int, string[]>)info.GetValue("RefreshPimsFull", typeof(Dictionary<int, string[]>));
            RefreshPimsIds = (Dictionary<int, int[]>)info.GetValue("RefreshPimsIds", typeof(Dictionary<int, int[]>));
            RefreshPimEnable = info.GetBoolean("RefreshPimEnable");
            RefreshTimer = info.GetInt32("RefreshTimer");
        }

        // Method to serialize data
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SegmentsID", SegmentsID);
            info.AddValue("SegmentIndex", SegmentIndex);
            info.AddValue("RefreshPimsFull", RefreshPimsFull);
            info.AddValue("RefreshPimsIds", RefreshPimsIds);
            info.AddValue("RefreshPimEnable", RefreshPimEnable);
            info.AddValue("RefreshTimer", RefreshTimer);
        }
    }
}