using System;

namespace ZFPimImporter.DataTypes
{
    [Serializable]
    public class ServerSettings
    {
        public string Ip { get; set; } = "localhost";
    
        public int Port { get; set; } = 3000;
    
        public bool UseHttps { get; set; } = false;
    
        public string CertPath { get; set; } = "";
    
        public string KeyPath { get; set; } = "";
    
        public int Timeout { get; set; } = 120000;
    
        public int MaxConnections { get; set; } = 100;
    
        public bool EnableCompression { get; set; } = true;
    
        public bool EnableCors { get; set; } = false;
    
        public string CorsOrigin { get; set; } = "*";
    
        public bool EnableAuth { get; set; } = false;
    
        public string AuthSecret { get; set; } = "";
    
        public string AuthIssuer { get; set; } = "";
    
        public string AuthAudience { get; set; } = "";
    }
}


