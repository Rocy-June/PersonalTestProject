using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.File
{
    [Serializable]
    public class File_Menu
    {
        public byte[] HeadImage { get; set; }

        public bool IsTop { get; set; }

        public bool IsMuted { get; set; }
    }
}
