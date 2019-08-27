
//The code generated time: 2019-08-26 18:40:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlogTestModels
{
	/// <summary>
	/// Blog
	/// </summary>
	public partial class BlogEntity
	{
		public string BlogID{ get; set; }
		public string BlogTitle{ get; set; }
		public string BlogContent{ get; set; }
		public string Memo{ get; set; }
		public string CreatedBy{ get; set; }
		public System.DateTime? CreatedTime{ get; set; }
		public string ModifiedBy{ get; set; }
		public System.DateTime? ModifiedTime{ get; set; }
	}
	
	/// <summary>
	/// BlogComment
	/// </summary>
	public partial class BlogCommentEntity 
	{
		public string BCID{ get; set; }
		public string BlogID{ get; set; }
		public string BCContent{ get; set; }
		public string CommentBy{ get; set; }
		public System.DateTime? CommentTime{ get; set; }
		public string Memo{ get; set; }
		public string CreatedBy{ get; set; }
		public System.DateTime? CreatedTime{ get; set; }
		public string ModifiedBy{ get; set; }
		public System.DateTime? ModifiedTime{ get; set; }
	}
	
	/// <summary>
	/// UserInfo
	/// </summary>
	public partial class UserInfoEntity 
	{
		public string UserID{ get; set; }
		public string UserCode{ get; set; }
		public string UserName{ get; set; }
		public string Passwords{ get; set; }
		public string Memo{ get; set; }
		public string CreatedBy{ get; set; }
		public System.DateTime? CreatedTime{ get; set; }
		public string ModifiedBy{ get; set; }
		public System.DateTime? ModifiedTime{ get; set; }
	}
	
}