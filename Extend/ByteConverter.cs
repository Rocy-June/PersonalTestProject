using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Extend
{
    public static class ByteConverter
    {
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
    }
}
