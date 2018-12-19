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
        /// 将变量转换为int32, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int ToIntWidthEx(this object obj)
        {
            return Convert.ToInt32(obj);
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
        /// 将变量转换为int64
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long Long(this object obj)
        {
            try { return Convert.ToInt64(obj); }
            catch { return 0; }
        }
        /// <summary>
        /// 将变量转换为int64, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long ToLongWidthEx(this object obj)
        {
            return Convert.ToInt64(obj);
        }
        /// <summary>
        /// 将变量转换为int64, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long? ToLongWidthNull(this object obj)
        {
            try { return Convert.ToInt64(obj); }
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
        /// 将变量转换为double, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double ToDoubleWidthEx(this object obj)
        {
            return Convert.ToDouble(obj);
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
