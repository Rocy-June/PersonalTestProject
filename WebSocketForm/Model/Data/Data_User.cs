using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Model.Enum;

namespace WebSocketForm.Model.Data
{
    [Serializable]
    public class Data_User
    {
        public string IP { get; set; }

        public byte[] HeadImage { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }
    }
}
