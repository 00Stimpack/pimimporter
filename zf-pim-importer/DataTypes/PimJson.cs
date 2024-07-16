using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

//find . -type d -name Library \( -exec test -d '{}/Packages' \; -or -exec test -d '{}/Assets' \; \) -print -prune

//find . -type d \( -exec test -d '{}/Assets' \; -or -exec test -d '{}/Library' \; \) -print -prune | grep "*"
//find . -type d \( -exec test -d '{}/Assets' \; -or -exec test -d '{}/Library' \; \) -print -prune  | xargs echo
namespace ZFPimImporter.DataTypes
{
       
    [Serializable]
    public class PimJson 
    {
        public int id { get; set; }
        public string path { get; set; }
        public PimData en { get; set; }
        public PimData de { get; set; }
        public string thumbnail { get; set; } = "";
        public string hero { get; set; } = "";
        public string hero_video { get; set; } = "";
        public string hero_model { get; set; } = "";
        public bool enabled { get; set; } = true;
        public bool is_confidential { get; set; } = false;

        // Default constructor
        public PimJson() { }

        // Special constructor for deserialization
        protected PimJson(SerializationInfo info, StreamingContext context)
        {
            // Deserialize all other properties as usual
            id = info.GetInt32("id");
            path = info.GetString("path");
            en = (PimData)info.GetValue("en", typeof(PimData));
            de = (PimData)info.GetValue("de", typeof(PimData));
            thumbnail = info.GetString("thumbnail");
            hero = info.GetString("hero");
            hero_video = info.GetString("hero_video");
            hero_model = info.GetString("hero_model");
            enabled = info.GetBoolean("enabled");

            // Use try-catch to handle cases where 'is_confidential' does not exist in the serialized data
            try
            {
                is_confidential = info.GetBoolean("is_confidential");
            }
            catch (SerializationException)
            {
                // Default to false if 'is_confidential' does not exist
                is_confidential = false;
            }
        }

        // Method to serialize data
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", id);
            info.AddValue("path", path);
            info.AddValue("en", en);
            info.AddValue("de", de);
            info.AddValue("thumbnail", thumbnail);
            info.AddValue("hero", hero);
            info.AddValue("hero_video", hero_video);
            info.AddValue("hero_model", hero_model);
            info.AddValue("enabled", enabled);
            info.AddValue("is_confidential", is_confidential);
        }
    }
    
}