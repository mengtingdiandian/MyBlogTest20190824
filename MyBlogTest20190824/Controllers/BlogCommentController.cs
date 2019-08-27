using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlogTestDAL;
using MyBlogTestModels;

namespace MyBlogTest20190824.Controllers
{
    public class BlogCommentController : Controller
    {
        BlogCommentDAL blogCommentDAL;

        /// <summary>
        /// 注入传递
        /// </summary>
        /// <param name="bdal"></param>
        public BlogCommentController(BlogCommentDAL bcdal)
        {
            this.blogCommentDAL = bcdal;
        }

        #region 获取所有的博客列表信息
        /// <summary>
        /// 获取所有的博客列表信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string sBlogID)
        {
            List<BlogCommentEntity> blogcommentlist = new List<BlogCommentEntity>();
            BlogCommentEntity blogcomment = new BlogCommentEntity();
            Hashtable has = new Hashtable();
            blogcommentlist = blogCommentDAL.GetBlogCommentByBlogID(sBlogID);
            blogcomment.BlogID = sBlogID;
            blogcommentlist.Add(blogcomment);
            return View(blogcommentlist);
        }
        #endregion

        #region 添加,编辑博客信息
        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="sBlogID"></param>
        /// <returns></returns>
        public IActionResult Add(string sBlogID)
        {
            BlogCommentEntity m = null;
            if (!string.IsNullOrEmpty(sBlogID))
            {
                m = blogCommentDAL.GetOneBlogCommentByBlogID(sBlogID);
                if (m == null)
                {
                    m = new BlogCommentEntity();
                    m.BlogID = sBlogID;
                }
            }
            return View(m);

        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Add(BlogCommentEntity blogCommentEntity)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Content("您尚未登录用户");
            }
            if (string.IsNullOrEmpty(blogCommentEntity.BCID))
            {
                //新增
                blogCommentEntity.BCID = Guid.NewGuid().ToString();
                blogCommentEntity.CreatedTime = DateTime.Now;
                blogCommentEntity.CommentTime = DateTime.Now;
                blogCommentEntity.CommentBy = this.User.Identity.Name;
                blogCommentEntity.CreatedBy = this.User.Claims.First().Value;
                blogCommentDAL.Add(blogCommentEntity);
            }
            return Redirect("/BlogComment/Index?sBlogID="+ blogCommentEntity.BlogID);
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑博客评论
        /// </summary>
        /// <param name="sBlogID"></param>
        /// <returns></returns>
        public IActionResult Update(string sBCID)
        {
            BlogCommentEntity m = null;
            if (!string.IsNullOrEmpty(sBCID))
            {
                m = blogCommentDAL.GetBlogCommentByBCID(sBCID);
            }
            return View(m);
        }

        [HttpPost]
        public IActionResult Update(BlogCommentEntity blogCommentEntity)
        {
            BlogCommentEntity m = null;
            if (!string.IsNullOrEmpty(blogCommentEntity.BCID))
            {
                //修改
                m = blogCommentDAL.GetBlogCommentByBCID(blogCommentEntity.BCID);
                if(m!=null)
                {
                    blogCommentEntity.ModifiedTime = DateTime.Now;
                    blogCommentEntity.ModifiedBy = this.User.Identity.Name;
                    blogCommentDAL.Update(blogCommentEntity);
                }
            }
            return Redirect("/BlogComment/Index?sBlogID=" + blogCommentEntity.BlogID);
        }
        #endregion

        #region 删除博客评论信息
        [HttpPost]
        public IActionResult Del(string sBCID)
        {
            BlogCommentEntity m = null;
            if (!string.IsNullOrEmpty(sBCID))
            {
                m = blogCommentDAL.GetBlogCommentByBCID(sBCID);
                if (m != null)
                {
                    bool b = blogCommentDAL.DeleteBlogCommentByBCID(sBCID);
                    if (b)
                    {
                        return Content("删除成功！");
                    }
                    else
                    {
                        return Content("删除失败，请联系管理员！");
                    }
                }
            }
            return null;

        }
        #endregion

       
    }
}