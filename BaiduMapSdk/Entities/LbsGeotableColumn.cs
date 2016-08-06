using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduMapSdk.Entities
{
    using LBSYunNetSDK;
    public class LbsGeotableColumn
    {
        public string Name{ get; set; }
        public string Key{ get; set; }
        public UInt32 Type{ get; set; }
        public UInt32 MaxLength{ get; set; }
        public string DefaultValue{ get; set; }
        public UInt32 IsSortfilterField{ get; set; }
        public UInt32 IsSearchField{ get; set; }
        public uint IsIndexField { get; set; }
        public UInt32 IsUniqueField{ get; set; }
    }
}
