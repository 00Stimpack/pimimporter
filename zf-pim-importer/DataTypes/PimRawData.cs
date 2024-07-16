namespace ZFPimImporter.DataTypes
{
    public class PimRawData
    {
        public string ID { get; set; }
        public string Language { get; set; }
        public string ProductName { get; set; }
        public string Subheadline { get; set; }
        public string Details { get; set; }
        public string KeyFact1 { get; set; }
        public string KeyFact1Description { get; set; }
        public string KeyFact2 { get; set; }
        public string KeyFact2Description { get; set; }
        public string KeyFact3 { get; set; }
        public string KeyFact3Description { get; set; }


        public string InformationHeader1 { get; set; } = "";
        public string InformationText1 { get; set; } = "";
     

        public string InformationHeader2 { get; set; } = "";
        public string InformationText2 { get; set; } = "";



        public bool IsConfidential  { get; set; } = false;

    }

}