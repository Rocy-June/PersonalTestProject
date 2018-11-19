using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model
{
    class OnlineUser
    {
        public bool IsTop { get; set; }
        public string IsTopVisibilityStr { get { return IsTop ? "Visible" : "Hidden"; } }
        public string Title { get; set; }
        public DateTime LastTime { get; set; }
        public string LastTimeStr { get { return LastTime.ToString("HH:mm"); } }
        public string LastSay { get; set; }
        public List<IconFont> Status { get; set; }
        public string StatusStr { get { return string.Join("", Status.Select(e => IconFontChar.Get(e))); } }
    }
}
