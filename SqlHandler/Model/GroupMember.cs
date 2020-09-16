using System;
using System.Collections.Generic;
using System.Text;

namespace SqlHandler.Model
{
    class GroupMember
    {
        public string GroupID { get; set; }

        public string UserID { get; set; }

        public long JoinTime { get; set; }
    }
}
