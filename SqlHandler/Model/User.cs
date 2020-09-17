using System;
using System.Collections.Generic;
using System.Text;

namespace SqlHandler.Model
{
    class User
    {
        public string ID { get; set; }

        public string IP { get; set; }

        public string Name { get; set; }

        public string HeadImageUrl { get; set; }

        public string NickName { get; set; }

        public long FirstConnectTime { get; set; }

        public long LastResponsedTime { get; set; }
    }
}
