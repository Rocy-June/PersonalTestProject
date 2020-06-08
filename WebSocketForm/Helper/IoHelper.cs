using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Helper
{
    static class IoHelper
    {
        public static void PathCheckAndCreate(string uri)
        {
            if (!File.Exists(uri))
            {
                var pathes = uri.Split('\\').ToList();
                pathes.RemoveAt(pathes.Count - 1);
                var path = string.Join("\\", pathes);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}
