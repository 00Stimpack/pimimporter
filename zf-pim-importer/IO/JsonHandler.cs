using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;




namespace ZFPimImporter.IO
{
    public static class JsonHandler
    {
        
        public static void WriteJson<T>(string jsonPath, T data)
        {
            var encoderSettings = new TextEncoderSettings();
            //encoderSettings.AllowCharacters('\u0436', '\u0430');
            encoderSettings.AllowRange(UnicodeRanges.All);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true
            };
            
            //Console.WriteLine("writitttng:"+jsonPath);

            string jsonString =  JsonSerializer.Serialize(data,options);
            File.WriteAllText(jsonPath, jsonString);
        }
        
        
        
        internal static T LoadJsonaa<T>(string filePath)
        {
            T data = default;
            Console.WriteLine("filePath:"+filePath);

            if (!File.Exists(filePath))
            {
               ;
                Console.WriteLine("file doesn't exist:"+filePath);

                
                return data;
            }
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowRange(UnicodeRanges.All);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true
            };

            using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                if (stream.Length > 3)
                {
                    // data =  Newtonsoft.Json.JsonConverter<T>(stream, options);
                    data = JsonSerializer.Deserialize<T>(stream, options);
                }
                else
                {
                    //File.Delete(filePath);
                    //LoadJsonaa<T>(filePath);
                    Console.WriteLine($"stream is null:{filePath}");
                    return data;


                };
            }
          

            return data;
        }
    }
    
}