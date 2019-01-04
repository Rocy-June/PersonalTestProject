using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model
{
    [Serializable]
    public class NickName
    {
        public IPAddress IP { get; set; }

        public string Name { get; set; }
    }
}
