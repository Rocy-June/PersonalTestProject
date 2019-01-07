using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model
{
    public class GroupChat : Menu
    {
        public DateTime ID { get; set; }

        public List<User> Members { get; set; }

        public string Name { get; set; }

        public DateTime LastRespondTime { get; set; }

        public override string LastTimeStr => throw new NotImplementedException();

        public override string StatusStr => throw new NotImplementedException();

        public override string LastSay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override DateTime LastChatTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
