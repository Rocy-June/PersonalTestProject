using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.File
{
    [Serializable]
    public class File_User : File_Menu
    {
        public byte[] IP { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public int OnlineStatus { get; set; }

        public long LastResponsedTime { get; set; }
    }
}
