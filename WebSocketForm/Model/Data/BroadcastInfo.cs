using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;

namespace WebSocketForm.Model.Data
{
    [Serializable]
    public class BroadcastInfo
    {
        public PostActionType Action { get; set; }
        public IPAddress IP { get; set; }
        public bool NeedHandShake { get; set; }
        public bool IsRequest { get; set; }
    }
}
