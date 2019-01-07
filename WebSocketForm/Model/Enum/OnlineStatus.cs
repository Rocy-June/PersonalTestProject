using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.Enum
{
    public enum OnlineStatus
    {
        Unknow = -1,
        Offline = 0,
        Online = 1,
        Hiding = 5,
        Leaving = 10,
        Busy = 20
    }
}
