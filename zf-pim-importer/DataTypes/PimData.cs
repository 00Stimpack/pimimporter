using System;

namespace ZFPimImporter.DataTypes
{
 using System;
using System.Runtime.Serialization;

[Serializable]
public class PimData 
{
    public string ProductName { get; set; }
    public string Subheadline { get; set; }
    public string Details { get; set; }
    public string KeyFact1 { get; set; }
    public string KeyFact1Description { get; set; }
    public string KeyFact2 { get; set; }
    public string KeyFact2Description { get; set; }
    public string KeyFact3 { get; set; }
    public string KeyFact3Description { get; set; }
    public string InformationHeader1 { get; set; }
    public string InformationText1 { get; set; }
    public string InformationHeader2 { get; set; }
    public string InformationText2 { get; set; }
    
    /*
    public bool IsConfidential  { get; set; } = false;
    */


    // Default constructor
    public PimData() { }

    // Special constructor for deserialization
    protected PimData(SerializationInfo info, StreamingContext context)
    {
        ProductName = info.GetString("ProductName");
        Subheadline = info.GetString("Subheadline");
        Details = info.GetString("Details");
        KeyFact1 = info.GetString("KeyFact1");
        KeyFact1Description = info.GetString("KeyFact1Description");
        KeyFact2 = info.GetString("KeyFact2");
        KeyFact2Description = info.GetString("KeyFact2Description");
        KeyFact3 = info.GetString("KeyFact3");
        KeyFact3Description = info.GetString("KeyFact3Description");
        InformationHeader1 = info.GetString("InformationHeader1");
        InformationText1 = info.GetString("InformationText1");
        InformationHeader2 = info.GetString("InformationHeader2");
        InformationText2 = info.GetString("InformationText2");
        

        
        /*try
        {
            IsConfidential = info.GetBoolean("IsConfidential");
            
        }
        catch (SerializationException)
        {
            IsConfidential = false;

        }*/
        
    }

    // Method to serialize data
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("ProductName", ProductName);
        info.AddValue("Subheadline", Subheadline);
        info.AddValue("Details", Details);
        info.AddValue("KeyFact1", KeyFact1);
        info.AddValue("KeyFact1Description", KeyFact1Description);
        info.AddValue("KeyFact2", KeyFact2);
        info.AddValue("KeyFact2Description", KeyFact2Description);
        info.AddValue("KeyFact3", KeyFact3);
        info.AddValue("KeyFact3Description", KeyFact3Description);
        info.AddValue("InformationHeader1", InformationHeader1);
        info.AddValue("InformationText1", InformationText1);
        info.AddValue("InformationHeader2", InformationHeader2);
        info.AddValue("InformationText2", InformationText2);
        /*
        info.AddValue("IsConfidential", IsConfidential);
        */

    }
}

}