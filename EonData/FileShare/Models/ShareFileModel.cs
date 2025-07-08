using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.FileShare.Models
{
    public class ShareFileModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public long? Size { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
