using System;
using System.Collections.Generic;
using ZFPimImporter.DataTypes;
using ZFPimImporter.Helpes;
using System.Runtime.Serialization;

namespace ZFPimImporter.Project
{
    

    [Serializable]
    public class ProjectData 
    {
        public CommandLineArguments Arguments = new CommandLineArguments() { Command = CommandLineHelper.Commands.Nothing };
        public string ProjectID { get; set; } = "ID1";
        public OptionImporter OptionImporter { get; set; } = new OptionImporter();
        public Option Option { get; set; } = new Option();
        public Dictionary<int, List<SegmentJson>> AllSegments { get; set; } = new Dictionary<int, List<SegmentJson>>();
        public Dictionary<int, Dictionary<int, List<PimJson>>> AllSolutions = new Dictionary<int, Dictionary<int, List<PimJson>>>();
        public List<PimJson> Products { get; set; } = new List<PimJson>();
        public List<SegmentJson> Segments { get; set; } = new List<SegmentJson>();
        public List<LanguageDataTools.LanguageData> LanguageData { get; set; } = new List<LanguageDataTools.LanguageData>();
        public string Version { get; set; } = FixedStrings.VERSION;

        // Default constructor
        public ProjectData() { }

        // Special constructor for deserialization
        protected ProjectData(SerializationInfo info, StreamingContext context)
        {
            Arguments = (CommandLineArguments)info.GetValue("Arguments", typeof(CommandLineArguments));
            ProjectID = info.GetString("ProjectID");
            OptionImporter = (OptionImporter)info.GetValue("OptionImporter", typeof(OptionImporter));
            Option = (Option)info.GetValue("Option", typeof(Option));
            AllSegments = (Dictionary<int, List<SegmentJson>>)info.GetValue("AllSegments", typeof(Dictionary<int, List<SegmentJson>>));
            AllSolutions = (Dictionary<int, Dictionary<int, List<PimJson>>>)info.GetValue("AllSolutions", typeof(Dictionary<int, Dictionary<int, List<PimJson>>>));
            Products = (List<PimJson>)info.GetValue("Products", typeof(List<PimJson>));
            Segments = (List<SegmentJson>)info.GetValue("Segments", typeof(List<SegmentJson>));
            LanguageData = (List<LanguageDataTools.LanguageData>)info.GetValue("LanguageData", typeof(List<LanguageDataTools.LanguageData>));
            Version = info.GetString("Version");
        }

        // Method to serialize data
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Arguments", Arguments);
            info.AddValue("ProjectID", ProjectID);
            info.AddValue("OptionImporter", OptionImporter);
            info.AddValue("Option", Option);
            info.AddValue("AllSegments", AllSegments);
            info.AddValue("AllSolutions", AllSolutions);
            info.AddValue("Products", Products);
            info.AddValue("Segments", Segments);
            info.AddValue("LanguageData", LanguageData);
            info.AddValue("Version", Version);
        }
    }

}