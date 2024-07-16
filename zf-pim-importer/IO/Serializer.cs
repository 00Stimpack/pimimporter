using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;



namespace ZFPimImporter.IO
{
    public static class Serializer
    {
        
        
        public static void WriteBin<T>(string filePath, T data)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, data);
            fs.Close();
        }

        public static T LoadBin<T>(string filePath)
        {
            T data = default;
            if (!File.Exists(filePath)) {
                return data;
            }

            using Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            if (stream.Length > 0) data = (T) bf.Deserialize(stream);

            return data;
        }
    }
}