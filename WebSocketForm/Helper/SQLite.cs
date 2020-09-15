using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Helper
{
    class SQLite
    {
        /// <summary>
        /// 创建SQLite.db文件
        /// </summary>
        /// <param name="fileName">db文件名</param>
        public static void CreateBDFile()
        {
            if (!Directory.Exists(AppData.DATABASE_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(AppData.DATABASE_DIRECTORY_PATH);
            }

            if (!File.Exists(AppData.DATABASE_FILE_PATH))
            {
                SQLiteConnection.CreateFile(AppData.DATABASE_FILE_PATH);
            }
        }

        /// <summary>
        /// SQLite 连接
        /// </summary>
        private SQLiteConnection conn;

        /// <summary>
        /// 实例化一个 SQLite 连接
        /// </summary>
        public SQLite()
        {
            conn = new SQLiteConnection(new SQLiteConnectionStringBuilder() { DataSource = AppData.DATABASE_FILE_PATH }.ToString());
            conn.Open();
        }

        private SQLiteCommand SQLiteCommand(string sqlStr)
        {
            return new SQLiteCommand(sqlStr, conn);
        }

        /// <summary>
        /// 执行sql并返回受影响行数
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>受影响行数</returns>
        public int ExcuteNonQuery(string sqlStr, params SQLiteParameter[] args)
        {
            using (var cmd = SQLiteCommand(sqlStr))
            {
                cmd.Parameters.AddRange(args);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行查询并返回查询结果的第一行第一列
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>查询结果的第一行第一列</returns>
        public object ExecuteScalar(string sqlStr, params SQLiteParameter[] args)
        {
            using (var cmd = SQLiteCommand(sqlStr))
            {
                cmd.Parameters.AddRange(args);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool CheckTableExists(string name) 
        {
            using (var cmd = SQLiteCommand($"SELECT 1 FROM {name}")) 
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行查询并返回多条数据
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>查询的数据表</returns>
        public DataTable GetDataTable(string sqlStr, params SQLiteParameter[] args)
        {
            using (var cmd = SQLiteCommand(sqlStr))
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
