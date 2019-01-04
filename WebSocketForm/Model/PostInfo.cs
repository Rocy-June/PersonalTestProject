using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketForm.Enum;

namespace WebSocketForm.Model
{
    [Serializable]
    public class PostInfo
    {
        public PostActionType Action { get; set; }
        public IPAddress IP { get; set; }
        public object Data { get; set; }
    }
}
