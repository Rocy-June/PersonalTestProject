using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Function
{
    class SQLite<T> where T : new()
    {
        /// <summary>
        /// 创建SQLite.db文件
        /// </summary>
        /// <param name="fileName">db文件名</param>
        public static void CreateBDFile()
        {
            var path = Environment.CurrentDirectory + @"\Data\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var dbFileName = path + typeof(T).Name + ".db";
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }
        }

        /// <summary>
        /// 获取给定SQLite.db的连接字符串
        /// </summary>
        /// <param name="fileName">db文件名</param>
        /// <returns>db的连接字符串</returns>
        public static string CreateConnectionString()
        {
            return new SQLiteConnectionStringBuilder() { DataSource = $@"Data\{typeof(T).Name}.db" }.ToString();
        }

        /// <summary>
        /// 执行sql并返回受影响行数
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>受影响行数</returns>
        public static int ExcuteNonQuery(string sqlStr, params SQLiteParameter[] args)
        {
            using (var con = new SQLiteConnection(CreateConnectionString()))
            {
                con.Open();
                using (var cmd = new SQLiteCommand() { CommandText = sqlStr })
                {
                    cmd.Parameters.AddRange(args);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询并返回查询结果的第一行第一列
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>查询结果的第一行第一列</returns>
        public static object ExecuteScalar(string sqlStr, params SQLiteParameter[] args)
        {
            using (var con = new SQLiteConnection(CreateConnectionString()))
            {
                con.Open();
                using (var cmd = new SQLiteCommand() { CommandText = sqlStr })
                {
                    cmd.Parameters.AddRange(args);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行查询并返回多条数据
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>查询的数据表</returns>
        public static DataTable GetDataTable(string sqlStr, params SQLiteParameter[] args)
        {
            using (var con = new SQLiteConnection(CreateConnectionString()))
            {
                con.Open();
                using (var cmd = new SQLiteCommand() { CommandText = sqlStr })
                {
                    cmd.Parameters.AddRange(args);

                    var ds = new DataSet();
                    var adapter = new SQLiteDataAdapter(cmd);

                    adapter.Fill(ds);

                    return ds.Tables[0];
                }
            }
        }



    }
}
