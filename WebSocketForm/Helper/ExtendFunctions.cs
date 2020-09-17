using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Helper
{
    static class ExtendFunctions
    {
        #region 类型转换
        /// <summary>
        /// 将变量转换为int32
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int ToInt(this object obj)
        {
            return (int)obj.ToDouble();
        }
        /// <summary>
        /// 将变量转换为int32, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int ToIntWithEx(this object obj)
        {
            return (int)obj.ToDoubleWithEx();
        }
        /// <summary>
        /// 将变量转换为int32, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static int? ToIntWithNull(this object obj)
        {
            return (int?)obj.ToDoubleWithNull();
        }

        /// <summary>
        /// 将变量转换为int64
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long ToLong(this object obj)
        {
            return (long)obj.ToDouble();
        }
        /// <summary>
        /// 将变量转换为int64, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long ToLongWithEx(this object obj)
        {
            return (long)obj.ToDoubleWithEx();
        }
        /// <summary>
        /// 将变量转换为int64, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static long? ToLongWithNull(this object obj)
        {
            return (long?)obj.ToDoubleWithNull();
        }

        /// <summary>
        /// 将变量转换为double
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double ToDouble(this object obj)
        {
            try { return Convert.ToDouble(obj); }
            catch { return 0; }
        }

        /// <summary>
        /// 将变量转换为double, 但无法转换会报错
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double ToDoubleWithEx(this object obj)
        {
            return Convert.ToDouble(obj);
        }

        /// <summary>
        /// 将变量转换为double, 但无法转换会返回null
        /// </summary>
        /// <param name="obj">任意变量</param>
        /// <returns>值</returns>
        public static double? ToDoubleWithNull(this object obj)
        {
            try { return Convert.ToDouble(obj); }
            catch { return null; }
        }
        #endregion

        #region 时间转换

        public static string ToMinimizeDateString(this DateTime dt)
        {
            var now = DateTime.Now;
            if (dt.Year != now.Year)
            {
                return dt.ToString("yyyy-MM-dd");
            }
            else if (dt.DayOfYear == now.DayOfYear)
            {
                return dt.ToString("HH:mm:ss");
            }
            else if (dt.DayOfYear + 1 == now.DayOfYear)
            {
                return "昨天";
            }
            else if (dt.DayOfYear + 2 == now.DayOfYear)
            {
                return "前天";
            }
            else
            {
                return dt.ToString("MM-dd HH:mm");
            }
        }

        #endregion

        #region 序列化
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] ToBytes(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buffer">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T ToObject<T>(this byte[] buffer) where T : new()
        {
            using (var ms = new MemoryStream(buffer))
            {
                var bf = new BinaryFormatter();
                return (T)bf.Deserialize(ms);
            }
        }
        #endregion
    }
}
