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
    public partial class BlogCommentDAL
    {
        /// <summary>
        /// 数据库连接字符串，从web层传入
        /// </summary>
        public string ConnStr { set; get; }

        #region  添加博客信息 
        /// <summary>
        /// 添加博客信息
        /// </summary>
        public int Add(BlogCommentEntity blogCommentEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BlogComment(");
            strSql.Append("BCID, BlogID, BCContent , CommentBy, CommentTime, Memo, CreatedBy, CreatedTime, ModifiedBy, ModifiedTime)");
            strSql.Append(" values (");
            strSql.Append("@BCID, @BlogID, @BCContent, @CommentBy, @CommentTime, @Memo, @CreatedBy, @CreatedTime,@ModifiedBy, @ModifiedTime)");

            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BCID", blogCommentEntity.BCID, dbType: DbType.String);
                param.Add("@BlogID", blogCommentEntity.BlogID, dbType: DbType.String);
                param.Add("@BCContent", blogCommentEntity.BCContent, dbType: DbType.String);
                param.Add("@CommentBy", blogCommentEntity.CommentBy, dbType: DbType.String);
                param.Add("@CommentTime", blogCommentEntity.CommentTime, dbType: DbType.DateTime);
                param.Add("@Memo", blogCommentEntity.Memo, dbType: DbType.String);
                param.Add("@CreatedBy", blogCommentEntity.CreatedBy, dbType: DbType.String);
                param.Add("@CreatedTime", blogCommentEntity.CreatedTime, dbType: DbType.DateTime);
                param.Add("@ModifiedBy", blogCommentEntity.ModifiedBy, dbType: DbType.String);
                param.Add("@ModifiedTime", blogCommentEntity.ModifiedTime, dbType: DbType.DateTime);
                int i = connection.Execute(strSql.ToString(), param);
                return i;
            }
        }
        #endregion

        #region  更新博客信息 
        /// <summary>
        /// 更新博客信息
        /// </summary>
        public bool Update(BlogCommentEntity blogCommentEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BlogComment set ");
            strSql.Append(" BCContent=@BCContent, ModifiedTime=getdate() ");
            strSql.Append(" where BCID=@BCID ");
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                int i = connection.Execute(strSql.ToString(), blogCommentEntity);
                return i > 0;
            }
        }

        #endregion

        #region 根据ID获取博客评论信息
        /// <summary>
        /// 根据ID获取博客评论信息
        /// </summary>
        public List<BlogCommentEntity> GetBlogCommentByBlogID(string sBlogID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BlogComment ");
            strSql.Append(" where BlogID=@BlogID ");
            List<BlogCommentEntity> blogcomment = null;
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", sBlogID, dbType: DbType.String);
                blogcomment = connection.Query<BlogCommentEntity>(strSql.ToString(),param).ToList();
            }
            return blogcomment;
        }
        #endregion

        #region 根据ID获取博客评论信息
        /// <summary>
        /// 根据ID获取博客评论信息
        /// </summary>
        public BlogCommentEntity GetOneBlogCommentByBlogID(string sBlogID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from BlogComment ");
            strSql.Append(" where BlogID=@BlogID ");
            BlogCommentEntity blogcomment = null;
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", sBlogID, dbType: DbType.String);
                blogcomment = connection.QuerySingleOrDefault<BlogCommentEntity>(strSql.ToString(), param);
            }
            return blogcomment;
        }
        #endregion

        #region 根据主键ID获取一条博客评论信息
        /// <summary>
        /// 根据ID获取博客评论信息
        /// </summary>
        public BlogCommentEntity GetBlogCommentByBCID(string sBCID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BlogComment ");
            strSql.Append(" where BCID=@BCID ");
           BlogCommentEntity blogcomment = null;
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BCID", sBCID, dbType: DbType.String);
                blogcomment = connection.QuerySingle<BlogCommentEntity>(strSql.ToString(), param);
            }
            return blogcomment;
        }
        #endregion

        #region 根据ID删除博客评论信息
        /// <summary>
        /// 根据ID删除博客评论信息
        /// </summary>
        public bool DeleteBlogCommentByBCID(string sBCID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BlogComment");
            strSql.Append(" where BCID=@BCID ");
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BCID", sBCID, dbType: DbType.String);
                int i = connection.Execute(strSql.ToString(), param);
                return i > 0;
            }
        }
        #endregion

        #region 根据BlogID删除博客评论信息
        /// <summary>
        /// 根据BlogID删除博客评论信息
        /// </summary>
        public bool DeleteBlogCommentByBlogID(string sBlogID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BlogComment");
            strSql.Append(" where BlogID=@BlogID ");
            using (var connection = ConnectionFactory.GetOpenConnection(ConnStr))
            {
                var param = new DynamicParameters();
                param.Add("@BlogID", sBlogID, dbType: DbType.String);
                int i = connection.Execute(strSql.ToString(), param);
                return i > 0;
            }
        }
        #endregion
    }

}
