using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortPinger
{
    public static class Extend
    {
        public static int ToInt(this object obj)
        {
            return ToIntWithNull(obj) ?? 0;
        }

        public static int? ToIntWithNull(this object obj)
        {
            try { return Convert.ToInt32(obj); }
            catch { return null; }
        }
    }
}
