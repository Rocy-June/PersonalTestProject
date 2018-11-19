using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model
{
    class IconFontChar
    {
        public static char Get(IconFont _if)
        {
            return new Dictionary<IconFont, char>
            {
                { IconFont.favor_fill, '' },
                { IconFont.round_close_fill, '' },
            }[_if];
        }
    }

    enum IconFont
    {
        favor_fill,
        round_close_fill,
    }
}
