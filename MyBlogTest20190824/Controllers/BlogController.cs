using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlogTestDAL;
using MyBlogTestModels;

namespace MyBlogTest20190821.Controllers
{

    public class viewModel
    {
        public List<BlogEntity> blog;
        public List<BlogCommentEntity> blogcomment;

    }

    /// <summary>
    /// 博客信息
    /// </summary>
    public class BlogController : Controller
    {
        BlogDAL blogDAL;
        BlogCommentDAL blogCommentDAL;

        /// <summary>
        /// 注入传递
        /// </summary>
        /// <param name="bdal"></param>
        public BlogController(BlogDAL bdal,BlogCommentDAL bcdal)
        {
            this.blogDAL = bdal;
            this.blogCommentDAL = bcdal;
        }


        #region 获取所有的博客列表信息
        /// <summary>
        /// 获取所有的博客列表信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string sUserID,string sQuery,DateTime? dtStartTime,DateTime? dtEndTime, int iPageIndex, int iPageSize)
        {
            List<MyBlogTestModels.BlogEntity> bloglist = null;
            Hashtable has = new Hashtable();
            bloglist = blogDAL.GetAllBlogList(sQuery, dtStartTime, dtEndTime,iPageIndex, iPageSize, has);
            return View(bloglist);
        }
        #endregion

        #region 根据条件获取博客列表信息
        /// <summary>
        /// 根据条件获取博客列表信息
        /// </summary>
        /// <returns></returns>
        public IActionResult List(string sUserID, string sQuery, DateTime? dtStartTime, DateTime? dtEndTime, int iPageIndex, int iPageSize)
        {
            List<MyBlogTestModels.BlogEntity> bloglist = null;
            Hashtable has = new Hashtable();
            bloglist = blogDAL.GetAllBlogList(sQuery, dtStartTime, dtEndTime, iPageIndex, iPageSize, has);
            ArrayList arr = new ArrayList();
            foreach (var item in bloglist)
            {
                arr.Add(new
                {
                    blogid = item.BlogID,
                    blogtitle = item.BlogTitle,
                    createdby = item.CreatedBy,
                    modifitime = item.ModifiedTime,
                    createdtime = item.CreatedTime
                });
            }
            return Json(arr);
            //return View(bloglist);
        }
        #endregion

        #region 取博客总记录数
        /// <summary>
        /// 取博客总记录数
        /// </summary>
        /// <returns></returns>
        public IActionResult GetTotalCount(string sQuery, DateTime? dtStartTime, DateTime? dtEndTime)
        {
            Hashtable has = new Hashtable();
            int totalcount = blogDAL.CalcCount(sQuery, dtStartTime, dtEndTime,has);
            return Content(totalcount.ToString());
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
            BlogEntity m = null;
            if (!string.IsNullOrEmpty(sBlogID))
            {
                m = blogDAL.GetBlogByID(sBlogID);
            }
            return View(m);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Add(BlogEntity blogEntity)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Content("您尚未登录用户");
            }
            if (string.IsNullOrEmpty(blogEntity.BlogID))
            {
                //新增
                blogEntity.BlogID = Guid.NewGuid().ToString();
                blogEntity.CreatedTime = DateTime.Now;
                blogEntity.CreatedBy = this.User.Claims.First().Value;
                blogDAL.Add(blogEntity);
            }
            return Redirect("/Blog/Index");
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑博客
        /// </summary>
        /// <param name="sBlogID"></param>
        /// <returns></returns>
        public IActionResult Update(string sBlogID)
        {
            BlogEntity m = null;
            if (!string.IsNullOrEmpty(sBlogID))
            {
                m = blogDAL.GetBlogByID(sBlogID);
            }
            return View(m);
        }

        [HttpPost]
        public IActionResult Update(BlogEntity blogEntity)
        {
            BlogEntity m = null;
            if (!string.IsNullOrEmpty(blogEntity.BlogID))
            {
                //修改
                m = blogDAL.GetBlogByID(blogEntity.BlogID);
                blogEntity.BlogTitle = m.BlogTitle;
                blogEntity.BlogContent = blogEntity.BlogContent;
                blogEntity.ModifiedTime = DateTime.Now;
                blogEntity.ModifiedBy = this.User.Identity.Name;
                blogDAL.Update(blogEntity);
            }
            return Redirect("/Blog/Index");
        }
        #endregion

        #region 删除博客信息
        [HttpPost]
        public IActionResult Del(string sBlogID)
        {
            BlogEntity m = null;
            if (!string.IsNullOrEmpty(sBlogID))
            {
                m = blogDAL.GetBlogByID(sBlogID);
                if (m != null)
                {
                    bool b = blogDAL.DeleteBlogByID(sBlogID);
                    if (b)
                    {
                        bool ba = blogCommentDAL.DeleteBlogCommentByBlogID(sBlogID);
                        if(ba)
                        { 
                            return Content("删除成功！");
                        }
                        else
                        {
                            return Content("删除失败，请联系管理员！");
                        }
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

        #region 根据ID获取博客信息
        /// <summary>
        /// 根据ID获取博客信息
        /// </summary>
        /// <param name="sBlogID"></param>
        /// <returns></returns>
        public IActionResult Detail(string sBlogID)
        {
            BlogEntity m = null;
            if (!string.IsNullOrEmpty(sBlogID))
            {
                m = blogDAL.GetBlogByID(sBlogID);
            }
            return View(m);
        }
        #endregion

    }
}