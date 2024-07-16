using System;

namespace ZFPimImporter.DataTypes
{
    [Serializable]
    public class ProductData
    {


        public string Thumbnail { get; set; } = "";
        public string Hero { get; set; }= "";
        public string[] FeatureImages { get; set; } = new string[]{};
        public string[] BenefitsImages { get; set; } = new string[]{};

        public string path { get; set; }
        public PimData en { get; set; }
        public PimData de { get; set; }

        public string thumbnail { get; set; } = "";
        
        public string hero { get; set; } = "";
        
        public string hero_video { get; set; } = "";

        public string hero_model { get; set; } = "";

        public bool enabled { get; set; } = true;
    }
}