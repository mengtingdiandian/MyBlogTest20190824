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
    public partial class BlogDAL
    {
        /// <summary>
        /// 数据库连接字符串，从web层传入
        /// </summary>
        public string ConnStr { set; get; }

        #region 获取所有的博客列表信息
        /// <summary>
        /// 获取所有的博客列表信息
        /// </summary>
        /// <param name="sQuery">文章标题</param>
        /// <param name="dtStartTime">创建起始时间</param>
        /// <param name="dtEndTime">创建截止时间</param>
        /// <param name="iPageIndex">分页参数</param>
        /// <param name="iPageSize">分页参数</param>
        /// <param name="has">扩展参数</param>
        /// <returns>博客</returns>
        public List<MyBlogTestModels.BlogEntity> GetAllBlogList(string sQuery, DateTime? dtStartTime, DateTime? dtEndTime, int iPageIndex, int iPageSize,Hashtable has)
        {
            List<MyBlogTestModels.BlogEntity> list = new List<MyBlogTestModels.BlogEntity>();
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                //获取目前已发布的所有博客
                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT TOP (@parm_pageSize) * FROM (SELECT 
                                         ROW_NUMBER() OVER(ORDER BY CreatedTime) as rowid 
                                              ,BlogID
                                              ,BlogTitle
                                              ,BlogContent
                                              ,Memo
                                              ,CreatedBy
                                              ,CreatedTime
                                              ,ModifiedBy
                                              ,ModifiedTime
                                          FROM Blog
                ");
                if (!string.IsNullOrEmpty(sQuery))
                {
                    sql.Append(" where BlogTitle like '%" + sQuery + "%' ");
                }
                else
                {
                    sql.Append(" where 1=1");
                }
                if (dtStartTime!=null)
                {
                    sql.Append(" and CreatedTime>= '" + dtStartTime+"'");

                }
                if (dtEndTime != null)
                {
                    if (dtStartTime.Equals(dtEndTime))
                    {
                        sql.Append(" and CreatedTime<= DATEADD(DAY, 1,'" + dtEndTime + "')");
                    }
                    else
                    {
                        sql.Append(" and CreatedTime<= '" + dtEndTime + "'");
                    }
                }
                sql.Append(" ) a WHERE rowid > ((@parm_pageIndex - 1) * @parm_pageSize);");
                //SqlParameter[] parms = {
                //                       new SqlParameter("@parm_pageIndex",iPageIndex),
                //                       new SqlParameter("@parm_pageSize",iPageSize)
                //                   };
                var param = new DynamicParameters();
                if (iPageIndex == 0) iPageIndex = 1;
                if (iPageSize == 0) iPageSize = 10;
                param.Add("@parm_pageIndex", iPageIndex, dbType: DbType.Int32);
                param.Add("@parm_pageSize", iPageSize, dbType: DbType.Int32);

                list = connection.Query<MyBlogTestModels.BlogEntity>(sql.ToString(), param).ToList();

            }
            return list;
        }
        #endregion

        #region 获取总博客记录数
        /// <summary>
        /// 获取总博客记录数
        /// </summary>
        /// <param name="sQuery">文章标题</param>
        /// <param name="dtStartTime">创建起始时间</param>
        /// <param name="dtEndTime">创建截止时间</param>
        /// <returns></returns>
        public int CalcCount(string sQuery, DateTime? dtStartTime, DateTime? dtEndTime, Hashtable has)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select count(1) from Blog");
            if (!string.IsNullOrEmpty(sQuery))
            {
                sql.Append(" where BlogTitle like '%" + sQuery + "%' ");
            }
            else
            {
                sql.Append(" where 1=1");
            }
            if (dtStartTime != null)
            {
                sql.Append(" and CreatedTime>= '" + dtStartTime+"'");
            }

            if (dtEndTime != null)
            {
                if (dtEndTime.Equals(dtStartTime))
                {
                    sql.Append(" and CreatedTime<= DATEADD(DAY, 1,'" + dtEndTime + "')");
                }
                else
                {
                    sql.Append(" and CreatedTime<= '" + dtEndTime + "'");
                }
            }
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int i = connection.QuerySingle<int>(sql.ToString());
                return i;

            }
        }
        #endregion

        #region  添加博客信息 
        /// <summary>
        /// 添加博客信息
        /// </summary>
        public int Add(BlogEntity blogEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into blog(");
            strSql.Append("BlogID, BlogTitle, BlogContent, Memo, CreatedBy , CreatedTime, ModifiedBy, ModifiedTime)");
            strSql.Append(" values (");
            strSql.Append("@BlogID, @BlogTitle, @BlogContent, @Memo, @CreatedBy, @CreatedTime, @ModifiedBy, @ModifiedTime)");

            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", blogEntity.BlogID, dbType: DbType.String);
                param.Add("@BlogTitle", blogEntity.BlogTitle, dbType: DbType.String);
                param.Add("@BlogContent", blogEntity.BlogContent, dbType: DbType.String);
                param.Add("@Memo", blogEntity.Memo, dbType: DbType.String);
                param.Add("@CreatedBy", blogEntity.CreatedBy, dbType: DbType.String);
                param.Add("@CreatedTime", blogEntity.CreatedTime, dbType: DbType.DateTime);
                param.Add("@ModifiedBy", blogEntity.ModifiedBy, dbType: DbType.String);
                param.Add("@ModifiedTime", blogEntity.ModifiedTime, dbType: DbType.DateTime);
                int i = connection.Execute(strSql.ToString(), param);
                return i;
            }
        }
        #endregion

        #region  更新博客信息 
        /// <summary>
        /// 更新博客信息
        /// </summary>
        public bool Update(BlogEntity blogEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Blog set ");
            strSql.Append(" BlogContent=@BlogContent,ModifiedBy=@ModifiedBy, ModifiedTime=@ModifiedTime ");
            strSql.Append(" where BlogID=@BlogID ");
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int i = connection.Execute(strSql.ToString(), blogEntity);
                return i > 0;
            }
        }

        #endregion

        #region 根据ID获取博客信息
        /// <summary>
        /// 根据ID获取博客信息
        /// </summary>
        public BlogEntity GetBlogByID(string sBlogID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Blog ");
            strSql.Append(" where BlogID=@BlogID ");
            BlogEntity blog = null;
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", sBlogID, dbType: DbType.String);
                blog = connection.QuerySingleOrDefault<BlogEntity>(strSql.ToString(),param);
            }
            return blog;
        }
        #endregion

        #region 根据ID删除博客信息
        /// <summary>
        /// 根据ID删除博客信息
        /// </summary>
        public bool DeleteBlogByID(string sBlogID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Blog");
            strSql.Append(" where BlogID=@BlogID ");
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", sBlogID, dbType: DbType.String);
                int i = connection.Execute(strSql.ToString(),param);
                return i > 0;
            }
        }
        #endregion
    }

}
