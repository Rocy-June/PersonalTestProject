using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlHandler
{
    public static class SQLite
    {

        /// <summary>
        /// SQLite 连接
        /// </summary>
        private static SQLiteConnection conn;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="fileName">db文件名</param>
        public static void Init(string file_path)
        {
            var dir = file_path.Substring(0, file_path.LastIndexOf('\\') + 1);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(file_path))
            {
                SQLiteConnection.CreateFile(file_path);
            }

            conn = new SQLiteConnection(new SQLiteConnectionStringBuilder() { DataSource = file_path }.ToString());
            conn.Open();
        }

        public static bool CheckTables()
        {
            var result = true;

            var checkTableSql = "SELECT 1 FROM ";
            var createTableSql = "CREATE TABLE ";

            foreach (var model in Assembly.Load("SqlHandler").GetTypes().Where(e => e.Namespace == "SqlHandler.Model").ToList())
            {
                try
                {
                    ExcuteNonQuery($"{checkTableSql}[{model.Name}]");
                }
                catch
                {
                    result = false;

                    var resetTableSql = $@"{createTableSql}[{model.Name}]({
                        string.Join(",",
                            model.GetProperties()
                                .Select(e =>
                                {
                                    var type = e.PropertyType.Name;
                                    switch (type)
                                    {
                                        case "Int32":
                                        case "Boolean":
                                            type = "INT";
                                            break;
                                        case "Int64":
                                            type = "LONG";
                                            break;
                                        case "String":
                                            type = "TEXT";
                                            break;
                                    }
                                    return $"{e.Name} {type}";
                                }
                            )
                        )
                    })";

                    ExcuteNonQuery(resetTableSql);
                }
            }

            return result;
        }

        private static SQLiteCommand SQLiteCommand(string sqlStr)
        {
            return new SQLiteCommand(sqlStr, conn);
        }

        /// <summary>
        /// 执行sql并返回受影响行数
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>受影响行数</returns>
        public static int ExcuteNonQuery(string sqlStr, params SQLiteParameter[] args)
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
        public static object ExecuteScalar(string sqlStr, params SQLiteParameter[] args)
        {
            using (var cmd = SQLiteCommand(sqlStr))
            {
                cmd.Parameters.AddRange(args);
                return cmd.ExecuteScalar();
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
