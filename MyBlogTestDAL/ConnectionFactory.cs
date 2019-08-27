using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks; 


namespace MyBlogTestDAL
{
    /// <summary>
    /// 数据库库连接工厂
    /// </summary>
    public class ConnectionFactory
    {
        #region 取数据库连接
        /// <summary>
        /// 取数据库连接
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns></returns>
        public static DbConnection GetOpenConnection(string ConnStr)
        {
            var connection = new System.Data.SqlClient.SqlConnection(ConnStr);
            connection.Open();
            return connection;
        }
        #endregion

    }
}
