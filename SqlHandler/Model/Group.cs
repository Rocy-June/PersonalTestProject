using System;
using System.Collections.Generic;
using System.Text;

namespace SqlHandler.Model
{
    class Group
    {
        public string ID { get; set; }

        public string OwnerID { get; set; }

        public string Name { get; set; }

        public long CreateTime { get; set; }
    }
}
