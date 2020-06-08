using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.Data
{
    class Data_Package
    {
        public long ID { get; set; }
        public long Index { get; set; }
        public byte[] Data { get; set; }
        public int DataLength { get; set; }
    }
}
