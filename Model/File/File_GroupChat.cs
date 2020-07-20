using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.File
{
    [Serializable]
    public class File_GroupChat : File_Menu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>群所有者IP</remarks>
        public byte[] OwnerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DateTimeTicks</remarks>
        public long ID { get; set; }

        public List<File_User> Members { get; set; }

        public string Name { get; set; }
    }
}
