using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.FileShare.Models
{
    public class ShareFolderModel
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public IEnumerable<ShareFileModel> Files { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
