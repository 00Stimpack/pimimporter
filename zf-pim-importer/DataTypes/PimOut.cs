using System;
using System.Collections.Generic;

namespace ZFPimImporter.DataTypes
{
    [Serializable]
    public class PimOut
    {

        public Option option { get; set; } = new Option();

        public List<SegmentJson> segments { get; set; } = new List<SegmentJson>();
        
        public List<PimJson> solutions { get; set; } = new List<PimJson>();

    }
}

