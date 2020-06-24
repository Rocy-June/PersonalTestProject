using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.Data
{
    public class NetPackage
    {
        public Guid ID { get; set; }
        public long PackageID { get; set; }
        public byte[] PackageData { get; set; }
        public bool IsEnd { get; set; }
    }
}
