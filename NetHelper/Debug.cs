using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkHandler
{
    public static class Debug
    {
        public static void TCP_Log(string text)
        {
            Log($@"(TCP) {text}");
        }

        public static void UDP_Log(string text)
        {
            Log($@"(UDP) {text}");
        }

        public static void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
