using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using MyBlogTestModels;

namespace MyBlogTestDAL
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public partial class UserInfoDAL
    {
        /// <summary>
        /// 数据库连接字符串，从web层传入
        /// </summary>
        public string ConnStr { set; get; }

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfoEntity Login(string sUsername, string sPassword)
        {
            string sql = "select * from UserInfo where username=@username and passwords=@password";
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@username", sUsername, dbType: DbType.String);
                param.Add("@password", sPassword, dbType: DbType.String);
                var m = connection.Query<UserInfoEntity>(sql, param).FirstOrDefault();
                return m;
            }
        }
        #endregion

    }

}
