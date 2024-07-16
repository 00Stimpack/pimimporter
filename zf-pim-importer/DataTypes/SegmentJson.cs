using System;

namespace ZFPimImporter.DataTypes
{
    using System.Runtime.Serialization;

    [Serializable]
    public class SegmentJson 
    {
        public int id { get; set; } = -1;
        public Translate translate { get; set; }
        public string[] subheadline { get; set; }
        public string[] subheadlinelanguage { get; set; }
        public string name { get; set; }
        public string namelanguage { get; set; }
        public string path { get; set; }
        public string icon { get; set; }
        public bool enabled { get; set; }
        public bool enabledlanguage { get; set; }

        // Default constructor
        public SegmentJson() { }

        // Special constructor for deserialization
        protected SegmentJson(SerializationInfo info, StreamingContext context)
        {
            id = info.GetInt32("id");
            translate = (Translate)info.GetValue("translate", typeof(Translate));
            subheadline = (string[])info.GetValue("subheadline", typeof(string[]));
            subheadlinelanguage = (string[])info.GetValue("subheadlinelanguage", typeof(string[]));
            name = info.GetString("name");
            namelanguage = info.GetString("namelanguage");
            path = info.GetString("path");
            icon = info.GetString("icon");
            enabled = info.GetBoolean("enabled");
            enabledlanguage = info.GetBoolean("enabledlanguage");
        }

        // Method to serialize data
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", id);
            info.AddValue("translate", translate);
            info.AddValue("subheadline", subheadline);
            info.AddValue("subheadlinelanguage", subheadlinelanguage);
            info.AddValue("name", name);
            info.AddValue("namelanguage", namelanguage);
            info.AddValue("path", path);
            info.AddValue("icon", icon);
            info.AddValue("enabled", enabled);
            info.AddValue("enabledlanguage", enabledlanguage);
        }
    }

}