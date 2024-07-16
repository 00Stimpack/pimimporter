using System;
using System.Collections.Generic;

namespace ZFPimImporter.DataTypes
{
    
    [Serializable]

    public class ProjectOverViewData
    {
             
        public string  ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        
        public string ProjectFile { get; set; }


    }
    
    
}