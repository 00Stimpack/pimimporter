using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZFPimImporter.IO;
using ZFPimImporter;
using System.Runtime.Serialization;

namespace ZFPimImporter.DataTypes
{
    

public class LanguageDataTools
{


    
   [Serializable]
   public class LanguageData 
   {
       public string Abbreviation { get; set; } = "EN";
       public LanguageType Code { get; set; } = LanguageType.EN;
       public string Name { get; set; } = "";
       public List<string> Exakt { get; set; } = new List<string>();
       public List<string> Regex { get; set; } = new List<string>();
       public Dictionary<int, PimData> Data { get; set; } = new Dictionary<int, PimData>();

       // Default constructor
       public LanguageData() { }

       // Special constructor for deserialization
       protected LanguageData(SerializationInfo info, StreamingContext context)
       {
           Abbreviation = info.GetString("Abbreviation");
           Code = (LanguageType)info.GetValue("Code", typeof(LanguageType));
           Name = info.GetString("Name");
           Exakt = (List<string>)info.GetValue("Exakt", typeof(List<string>));
           Regex = (List<string>)info.GetValue("Regex", typeof(List<string>));
           Data = (Dictionary<int, PimData>)info.GetValue("Data", typeof(Dictionary<int, PimData>));
       }

       // Method to serialize data
       public void GetObjectData(SerializationInfo info, StreamingContext context)
       {
           info.AddValue("Abbreviation", Abbreviation);
           info.AddValue("Code", Code);
           info.AddValue("Name", Name);
           info.AddValue("Exakt", Exakt);
           info.AddValue("Regex", Regex);
           info.AddValue("Data", Data);
       }
   }
    
    public readonly static List<(LanguageType, string, List<string>, List<string>)> OriginData = new List<(LanguageType, string, List<string>, List<string>)>()
    {
        (LanguageType.EN,"English", new List<string>(){
            "English",
            "English (Global)",
            "EN",
        },
            new List<string>()
            {
                "English"
            }),
        (LanguageType.DE,"German", new List<string>(){
                "German",
                "German (Global)",
                "DE",
            },
            new List<string>()
            {
                "German",
                "Deutsch"
            }),
        (LanguageType.KO,"Korean", new List<string>(){
                "Korean",
                "Koreanisch",
                "Korean (Global)",
                "KO"
            },
            new List<string>()
            {
                "Korean",
                "Koreanisch"
            }),
        (LanguageType.ES,"Spanish", new List<string>(){
                "Spanish",
                "Spanisch",
                "Español",
                "Spanisch (Global)",
                "ES"
            },
            new List<string>()
            {
                "Spanish",
                "Spanisch",
                "Español"
            }),
        (LanguageType.IT,"Italian", new List<string>(){
                "Italienisch",
                "Italian",
                "Italian (Global)",
                "IT"
            },
            new List<string>()
            {
                "Italian",
                "Italienisch"
            }),
        (LanguageType.FR,"French", new List<string>(){
                "French",
                "France",
                "Französisch",
                "France (Global)",
                "FR"
            },
            new List<string>()
            {
                "French",
                "France"
            }),
        (LanguageType.JP,"Japanese", new List<string>(){
                "Japanese",
                "Japan",
                "Japanese (Global)",
                "JP"
            },
            new List<string>()
            {
                "Japanese",
                "Japan"
            }),
        (LanguageType.ZH,"Chinese", new List<string>(){
                "Chinese",
                "China",
                "Chinese (Global)",
                "ZH"
            },
            new List<string>()
            {
                "Chinese",
                "China"
            }),
        (LanguageType.PL,"Polish", new List<string>(){
                "Polish",
                "Poland",
                "Polish (Global)",
                "PL"
            },
            new List<string>()
            {
                "Polish",
                "Poland"
            }),
        (LanguageType.TR,"Turkish", new List<string>(){
                "Turkish",
                "Turkiye",
                "Turkish (Global)",
                "TR"
            },
            new List<string>()
            {
                "Turkish",
                "Turkiye"
            }),
    };
    

    
    public static List<LanguageDataTools.LanguageData> CopyLanguageData(List<LanguageDataTools.LanguageData> needToCopy)
    {
        List<LanguageDataTools.LanguageData> copiedData = new List<LanguageDataTools.LanguageData>();
    
        foreach (var original in needToCopy)
        {
            LanguageDataTools.LanguageData copy = new LanguageDataTools.LanguageData
            {
                Abbreviation = original.Abbreviation,
                Code = original.Code,
                Name = original.Name,
                Exakt = new List<string>(original.Exakt),
                Regex = new List<string>(original.Regex),
                Data = new Dictionary<int, PimData>(original.Data)
            };

            copiedData.Add(copy);
        }

        return copiedData;
    }
    
    private static void MenuItem_LanguageClick(object sender, RoutedEventArgs e)
    {
        MenuItem item = (MenuItem)sender;
        Console.WriteLine("FOASODS");
        
        //MenuLanguageButton.Header = " " + item.Header +" ";
    }

    public static void LanguageRefresh(MenuItem menuLanguageButton,
        Action<object, RoutedEventArgs> menuItemLanguageClick)
    {
    }

    public static void LanguageMenuSwitch(LanguageType type)
    {
        var langdata = DataSave.CurrentProject.LanguageData[(int)type];
        for (int i = 0; i < DataSave.Products.Count; i++)
        {
            int id = DataSave.Products[i].id;
            if( langdata.Data.TryGetValue(id, out var pim))
            {
                DataSave.Products[i].de = pim;
                //Console.WriteLine("switched:"+id.ToString());
            }
            else
            {
                DataSave.Products[i].de =   DataSave.Products[i].en;
            }
           
        }

    }

    public static void LanguageMenuSwitch(string type,bool save=true)
    {
       // Console.WriteLine("TRY LANG TYPE:"+type);
        if (Enum.TryParse<LanguageType>(type, out LanguageType result)) {
            LanguageMenuSwitch(result);
            
            PathGenerator.SwitchSegmentLanguage(result);
            
            if (save)
            {
               // JsonHandler.WriteJson(PathGenerator.GetProductJsonPath(),DataSave.Products.ToList());
            }

            DataSave.CurrentProject.Option.SecondLanguage = result.ToString();
            PathGenerator.SaveOptions();
        }



    }
 
    
    public static void LanguageDataInit()
    {
        
        bool needCreation = DataSave.CurrentProject.LanguageData == null || DataSave.CurrentProject.LanguageData.Count == 0 || DataSave.CurrentProject.LanguageData.Count != OriginData.Count;
        if (needCreation)
        {
            DataSave.CurrentProject.LanguageData = new List<LanguageData>();
            Console.WriteLine("needCreation count:"+DataSave.CurrentProject.LanguageData.Count  + " OriginCount:"+OriginData.Count);
            // Console.WriteLine("temp:"+temp.Count);
            for (int i = 0; i < OriginData.Count; i++)
            {
               
                LanguageData newdata = new LanguageData()
                {
                    Code = OriginData[i].Item1,
                    Abbreviation = OriginData[i].Item1.ToString(),
                    Name = OriginData[i].Item2,
                    Exakt = new List<string>( OriginData[i].Item3),
                    Regex = new List<string>(OriginData[i].Item4) ,
                    Data = new Dictionary<int, PimData>(),
                };
               
                DataSave.CurrentProject.LanguageData?.Add(newdata);
            
            }

        }



    }
    
}



}

