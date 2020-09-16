using System;
using System.Collections.Generic;
using System.Text;

namespace SqlHandler.Model
{
    class GroupChat
    {
        public string SenderID { get; set; }

        public string GroupID { get; set; }

        public string SenderIP { get; set; }

        public bool IsText { get; set; }

        public string Text { get; set; }

        public bool IsImage { get; set; }

        public bool IsFile { get; set; }

        public string FileUrl { get; set; }

        public long SendTime { get; set; }
    }
}
