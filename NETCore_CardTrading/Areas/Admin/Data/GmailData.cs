using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore_CardTrading.Areas.Admin.Data
{
    public class GmailData
    {
        public string mailTemplate { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string title { get; set; }
        public string mainContent { get; set; }
        public string path { get; set; }
    }
}
