using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.Enum
{
    public enum ChatStatus
    {
        Sending = 0,
        Success = 200,
        SendFail = 503
    }
}
