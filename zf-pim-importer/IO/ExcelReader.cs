using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using ExcelDataReader;
using ZFPimImporter.DataTypes;
using ZFPimImporter.Views;

namespace ZFPimImporter.IO
{
  
    public static class ExcelReader
    {


        

        public static int RemoveZeros(string number)
        {
            bool hitNumber = false;

            string idString = "";
            for (int j = 0; j < number.Length; j++)
            {
                if (number[j] == '0' && !hitNumber)
                    continue;
                hitNumber = true;
                idString += number[j];
            }

            if (!int.TryParse(idString, out var id))
                return -1;

            return id;
        }
            
        

        public static (PimData,LanguageType) GuessLanguage(PimRawData raw)
        {
            //PimRawToPimData(PimRawData raw)
            PimData pim = null;
            if(string.IsNullOrEmpty( raw.Language))
                return (pim,LanguageType.EN);
            int index = 0;
            LanguageType type = LanguageType.EN;
            LanguageDataTools.LanguageData langdata = null;
            bool hit = false;
            for (int i = 0; i < DataSave.CurrentProject.LanguageData.Count; i++)
            {
                for (int j = 0; j < DataSave.CurrentProject.LanguageData[i].Exakt.Count; j++)
                {
                    if (raw.Language == DataSave.CurrentProject.LanguageData[i].Exakt[j])
                    {
                        index = i;
                        langdata = DataSave.CurrentProject.LanguageData[i];
                        type = langdata.Code;
                        hit = true;
                        break;
                    
                    }
                }
           
                for (int j = 0; j < DataSave.CurrentProject.LanguageData[i].Regex.Count; j++)
                {
                    if (raw.Language.Contains(DataSave.CurrentProject.LanguageData[i].Exakt[j]))
                    {
                        index = i;
                        langdata = DataSave.CurrentProject.LanguageData[i];
                        hit = true;
                        type = langdata.Code;
                        break;
                     
                    }
                }
            };
         

            if (hit)
            {
                pim = PimRawToPimData(raw);
                int id = RemoveZeros(raw.ID);
                if(id < 0)
                    return (null,LanguageType.EN);
                    
                if (!DataSave.CurrentProject.LanguageData[index].Data.TryGetValue(id, out var oldpim))
                {
                    DataSave.CurrentProject.LanguageData[index].Data.Add(id,pim);
                }
                else
                {
                    DataSave.CurrentProject.LanguageData[index].Data[id] = pim;
                }
                
            }
            return (pim,type);

        }

     

        public static List<PimJson> ConvertPimRaw(List<PimRawData> rawData, bool parseConfidential = false)
        {
            List<PimJson> pimJsons = new List<PimJson>();
            List<string> addIDs = new List<string>();
            
            for (int i = 0; i < rawData.Count; i++)
            {

                var currentItem = rawData[i];


                //var dataLang =  PimRawToPimData(currentItem);

                if (string.IsNullOrEmpty(currentItem.ID))
                    continue;
                if (addIDs.Contains(currentItem.ID)) continue;

                PimData pimData = null;

                
                var all = rawData.FindAll(x => x.ID == currentItem.ID);

                bool isConfidential = false;
                for (int j = 0; j < all.Count; j++)
                {
                    if (all[j].IsConfidential)
                    {
                        isConfidential = true;
                    }
                    if(all[j].ProductName != null)
                    {
                        var langpim = GuessLanguage(all[j]);
                        if (langpim.Item1 == null) continue;
                        if (langpim.Item2 == LanguageType.EN)
                        {
                          
                            pimData = langpim.Item1;
                        }
             
                    }

                    var index = rawData.IndexOf(all[j]);
                    if (index > i)
                    {
                        //Console.WriteLine("NEED TO DELETE:"+index.ToString());
                       // i--;
                    }
                }
                
                PimJson json = new PimJson();
                json.id = -1;
          
                bool hitNumber = false;

                string idString = "";
                for (int j = 0; j < currentItem.ID.Length; j++)
                {
                    if (currentItem.ID[j] == '0' && !hitNumber)
                        continue;
                    hitNumber = true;
                    idString += currentItem.ID[j];
                }

                if (isConfidential)
                {
                    json.is_confidential = true;
                }
                if (!int.TryParse(idString, out var id))
                {
                    continue;
                }
                json.id = id;
                // if (data.)
                if (pimData != null && parseConfidential)
                {
                    json.is_confidential = isConfidential;
                }
                
                if (pimData != null && json.en == null)
                {
                    json.en = pimData;
                    pimJsons.Add(json);
                    addIDs.Add(currentItem.ID);
                }
           

            }

            return pimJsons;
        }


        private static PimData PimRawToPimData(PimRawData raw)
        {
            PimData data = new PimData();
            data.ProductName = raw.ProductName;
            data.Subheadline = raw.Subheadline;
            /*
            data.IsConfidential = raw.IsConfidential;
            */
            data.Details = raw.Details;
            data.KeyFact1 = raw.KeyFact1;
            data.KeyFact2 = raw.KeyFact2;
            data.KeyFact3 = raw.KeyFact3;
            data.KeyFact1Description = raw.KeyFact1Description;
            data.KeyFact2Description = raw.KeyFact2Description;
            data.KeyFact3Description = raw.KeyFact3Description;

            Console.Write( raw.InformationHeader1);
            data.InformationHeader1 = raw.InformationHeader1;
            data.InformationHeader2 = raw.InformationHeader2;
            data.InformationText1 = raw.InformationText1;
            data.InformationText2 = raw.InformationText2;

            List<string> bulletTemps = new List<string>();
            
    
            
            return data;
            
        }


        private static string standradTable = "NoSuchTableSelected";

        internal static bool HasConfidential(DataTableCollection resultTables, string tableName)
        {
            foreach (DataTable table in resultTables)
            {
                if (table.Columns.Contains(tableName))
                {
                    return true;
                }
            }
            return false;
        }

        internal static List<PimRawData> LoadPimDataFromExcel(string pathToExcel)
        {
            List<PimRawData> pimData = new List<PimRawData>();
            using (var stream = File.Open(pathToExcel, FileMode.Open, FileAccess.Read))
            {
                ExcelReaderConfiguration readerConf = new ExcelReaderConfiguration();
              
                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream,readerConf))
                {

                    

                    for (var q = 0; q < reader.ResultsCount; q++)
                    {
                        
                      
                                         
                        Console.WriteLine("RESULT:"+reader.ResultsCount);
                        var fromRow = 0;
                        var fromCol = 0;
                        var index = 0;
                        var conf = new ExcelDataSetConfiguration
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                
                                FilterRow = rowReader => fromRow <= ++index - 1,
                                FilterColumn = (rowReader, colIndex) => fromCol <= colIndex,
                                UseHeaderRow = true
                            }
                        };

                        
                        var result = reader.AsDataSet(conf);
                        //bool hasConfidential = HasConfidential(result.Tables, DataSave.Option.TableName);
                        //bool hasConfidential = HasConfidential(result.Tables, "Confidential");
                        for (int r = 0; r < result.Tables.Count; r++)
                        {
                           var dataTable = result.Tables[r];
                     
                            for (var i = 0; i < dataTable.Rows.Count; i++)
                            {
                                var product_name = "";
                                PimRawData pimdata = new PimRawData();
                                for (var j = 0; j < dataTable.Columns.Count; j++)
                                {


                                    string currentRow  = dataTable.Rows[i][j].ToString();
                                    string test  = currentRow.Replace("\n","").Trim();
                                    if(string.IsNullOrEmpty(test))continue;
                                    string currentColum = dataTable.Columns[j].ToString().Trim();
                                    switch (currentColum)
                                    {
                                        case "Product-ID (PIM Product Basic) (PIM Product Basic)":
                                            if(string.IsNullOrEmpty(pimdata.ID)) 
                                                pimdata.ID = currentRow;
                                            break;
                                        case "Product-ID":
                                            if(string.IsNullOrEmpty(pimdata.ID))
                                                pimdata.ID = currentRow;
                                            break;
                                        case "Product Name (PIM Product Basic) (PIM Product Basic)":
                                            if(string.IsNullOrEmpty(pimdata.ProductName))
                                                pimdata.ProductName = currentRow;
                                            break;
                                        case "Product Name":
                                            if(string.IsNullOrEmpty(pimdata.ProductName))
                                                pimdata.ProductName = currentRow;
                                            break;
                                        case "ES: Product Name":
                                            if(string.IsNullOrEmpty(pimdata.ProductName))
                                                pimdata.ProductName = currentRow;
                                            break;
                                        case "ProductName_EN":
                                            product_name = currentRow;
                                            break;
                                        case "Language":
                                            if(string.IsNullOrEmpty(pimdata.ProductName))
                                                pimdata.Language = currentRow;
                                            break;
                                        case "Subheadline":
                                            if(string.IsNullOrEmpty(pimdata.Subheadline))
                                                pimdata.Subheadline = currentRow;
                                            break;
                                        case "ES: Subheadline":
                                            if(string.IsNullOrEmpty(pimdata.Subheadline))
                                                pimdata.Subheadline = currentRow;
                                            break;
                                        case "Lead Text":
                                            if(string.IsNullOrEmpty(pimdata.Details)) 
                                                pimdata.Details = currentRow;
                                            break;
                                        case "ES: Key Fact 1":
                                        case "Top Key Fact 1 - Key Value":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact1)) 
                                                pimdata.KeyFact1 = currentRow;
                                            break;
                                        case "ES: Key Fact 1 - Short Description":
                                        case "Top Key Fact 1 - Short Description":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact1Description)) 
                                                pimdata.KeyFact1Description = currentRow;
                                            break;
                                        case "ES: Key Fact 2":
                                        case "Top Key Fact 2 - Key Value":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact2)) 
                                                pimdata.KeyFact2 = currentRow;
                                            break;
                                        case "ES: Key Fact 2 - Short Description":
                                        case "Top Key Fact 2 - Short Description":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact2Description))
                                                pimdata.KeyFact2Description = currentRow;
                                            break;
                                        case "ES: Key Fact 3":
                                        case "Top Key Fact 3 - Key Value":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact3))
                                                pimdata.KeyFact3 = currentRow;
                                            break;
                                        case "ES: Key Fact 3 - Short Description":
                                        case "Top Key Fact 3 - Short Description":
                                            if(string.IsNullOrEmpty(pimdata.KeyFact3Description))
                                                pimdata.KeyFact3Description = currentRow;
                                            break;
                                        case "Further Information 1 - Header":
                                            if(string.IsNullOrEmpty(pimdata.InformationHeader1))
                                                pimdata.InformationHeader1 = currentRow;
                                            break;
                                        case "Further Information 1 - Text":
                                            if(string.IsNullOrEmpty(pimdata.InformationText1))
                                                pimdata.InformationText1 = currentRow;
                                            break;
                                        case "Further Information 2 - Header":
                                            if(string.IsNullOrEmpty(pimdata.InformationHeader2))
                                                pimdata.InformationHeader2 = currentRow;
                                            break;
                                        case "Further Information 2 - Text":
                                            if(string.IsNullOrEmpty(pimdata.InformationText2))
                                                pimdata.InformationText2 = currentRow;
                                            break;
                                     
                                        /*case "Confidential":
                                            if(string.IsNullOrEmpty(pimdata.Confidential) && tableName == standradTable)
                                                pimdata.Confidential = currentRow;
                                            break;*/
                                        
                                        /*
                                         case "Confidential":
                                            if(string.IsNullOrEmpty(pimdata.Confidential) && tableName == standradTable)
                                                pimdata.Confidential = currentRow;
                                            break;
                                        */
                                    }

                                    if (currentColum == $"{DataSave.Option.TableName}")
                                    {
                                        foreach (var confidentialGroup in DataSave.Option.ConfidentialGroups)
                                        {
                                            string testGroup = confidentialGroup.Trim();
                                            if(string.IsNullOrWhiteSpace(testGroup))continue;
                                            if (string.Equals(test, testGroup, StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                pimdata.IsConfidential = true;
                                            }
                                        }
                                    }
                                    
                                    /*if (dataTable.Columns[j].ToString().Trim() == tableName)
                                    {
                                        if(string.IsNullOrEmpty(pimdata.Confidential) )
                                            pimdata.Confidential = currentRow;
                                    }*/
                                    
                                }
                                
                                

                                if (string.IsNullOrEmpty(pimdata.ProductName))
                                    pimdata.ProductName = product_name;

                                
                                if(!string.IsNullOrEmpty(pimdata.ID))
                                {
                                    //Console.Write("FOUND:"+pimdata.ProductName + " Language:"+pimdata.Language+ " ID:"+pimdata.ID);
                                    pimData.Add(pimdata);
                                }
                                else
                                {
                                    Console.Write("COULNDT DETERMINE DATA:"+pimdata.ProductName + " Language:"+pimdata.Language+ " ID:"+pimdata.ID);
                                }

                            }
                        }
                        reader.NextResult();
                    }
                }
            }

            return pimData;
        }
    }
}