using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model
{
    public interface IConnection
    {
        void Send(string str);
    }
}
