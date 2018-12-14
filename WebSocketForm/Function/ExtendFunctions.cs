using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Function
{
    static class ExtendFunctions
    {
        /// <summary>
        /// 将变量转换为int32
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int Int(this object obj)
        {
            try { return Convert.ToInt32(obj); }
            catch { return 0; }
        }
        /// <summary>
        /// 将变量转换为int32, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int? ToIntWidthNull(this object obj)
        {
            try { return Convert.ToInt32(obj); }
            catch { return null; }
        }

        /// <summary>
        /// 将变量转换为double
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double Double(this object obj)
        {
            try { return Convert.ToDouble(obj); }
            catch { return 0; }
        }
        /// <summary>
        /// 将变量转换为double, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double? ToDoubleWidthNull(this object obj)
        {
            try { return Convert.ToDouble(obj); }
            catch { return null; }
        }
    }
}
