using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enum
{
    public enum MessageStatus
    {
        Sending = 0,
        Success = 200,
        SendFail = 503
    }
}
