using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fashion.Code.DAL
{   


    
    public class Post_dal
    {
        /////此函数在User_dal 里已经实现，GetUserId
        ///// <summary>
        ///// 返回查询的userId
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public object select_userId(string userName)
        //{
        //    string sqlStr = "select User_Id from tb_User where User_Name=@userName";
        //    SqlParameter[] parameters = new SqlParameter[] { 
        //        new SqlParameter("@userName",userName)
        //    };
        //    return SqlHelper.ExecuteScalar(sqlStr, parameters);
        //}




        /// <summary>
        /// 从数据库获取四张普通咨询帖子的首图、html链接、内容前两百个字符
        /// </summary>
        /// <returns></returns>
        public List<Post_model> GetFourPuTongPost()
        {
            string sqlStr = "select top 4 * from PostView";
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr);
            List<Post_model> post_modelList = new List<Post_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                post_modelList.Add(ToPictureModel(row));//调用ToPictureModel()函数
            }

            return post_modelList;

        }

        //将一条数据转化为Post-model数据
        //帖子的第一张图片，内容的前200个字符
        public Post_model ToPictureModel(DataRow row)
        {
            Post_model post_model = new Post_model();
            post_model.postContent = row["content"].ToString();//内容
            post_model.firstPostPhotoUrl = row["PostPhoto_PhotoUrl"].ToString();//图
            post_model.postId = (int)row["id"];
            return post_model;
        }




        /// <summary>
        ///查询表tb_Collect中的Collect_PostType，返回1，收藏已经存在,返回Null，不存在,以后可能有返回0的情况（跟帖）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public object select_ShouCang(string userId, string postId)
        {
            string sqlStr = "select Collect_PostType from tb_Collect where Collect_CollectorId=@userId and Collect_PostId=@postId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userId",userId),
                new SqlParameter("@postId",postId),
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);


        }

        /// <summary>
        /// 执行插入收藏，插入tb_Collect
        /// 成功返回1，失败返回0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="postType"></param>
        /// <returns></returns>
        public int insert_ShouCang(string userId,string postId,string postType){
               string sqlStr="insert  into tb_Collect(Collect_CollectorId,Collect_PostId,Collect_PostType)values(@userId,@postId,@postType)";
                  SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@postId",postId),
                    new SqlParameter("@postType",postType),
            };        
         return SqlHelper.ExecuteNonquery(sqlStr, parameters);
       }

      
    
       /// <summary>
        ///执行取消收藏操作, 成功返回1，失败返回0
       /// </summary>
       /// <param name="postId"></param>
       /// <param name="postType"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
        public int delete_ShouCang(string postId,string postType){
              string sqlStr="delete  from tb_Collect where Collect_PostId=@postId and Collect_PostType=@postType";
                  SqlParameter[] parameters = new SqlParameter[] {                 
                      new SqlParameter("@postId",postId),
                      new SqlParameter("@postType",postType),
                     

            };        
         return SqlHelper.ExecuteNonquery(sqlStr, parameters);

        }



         /// <summary>
        /// 通过标题查询数据库里该标题的条数
        /// </summary>
        /// <param name="Caption"></param>
        /// <returns></returns>
        public object getCaptionCount(string caption)
        {
            string sqlStr = "select count(*) from [Post] where caption =@caption";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@caption",caption)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }
        /// <summary>
        /// 通过标题查询数据库获得postId
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public object GetPostId(string caption)
        {
            string sqlStr = "select Post_Id from tb_Post where Post_Caption=@caption";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@caption",caption)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);  
        }

        /// <summary>
        /// 通过静态post页面url查询数据库获得postId
        /// 结果返回1代表数据库出错
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public object GetPostIdBy_PostHtmlUrl(string postHtmlUrl)
        {
            string sqlStr = "select Post_Id from tb_Post where Post_HtmlUrl=@postHtmlUrl";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@postHtmlUrl",postHtmlUrl)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);

        }

      

        /// <summary>
        /// 获取数据库里帖子表的从min到max的数据
        /// 返回post_modelList;
        /// </summary>
        /// <param name="min">开始</param>
        /// /// <param name="max">结尾</param>
        /// <returns></returns>
        public List<Post_model> GetPost(int min,int max)
        {
            string sqlStr = @"select * from(
                                                          select ROW_NUMBER() over(order by id) orderNumber,* from PostView
                                                               ) as postTable 
                                                      where postTable.orderNumber between @min and @max";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@min",min),
                new SqlParameter("@max",max)
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            List<Post_model> post_modelList = new List<Post_model>();
            foreach(DataRow row in dataTable.Rows)
            {
                post_modelList.Add(ToModel(row));
            }
            return post_modelList;
        }

        /// <summary>
        /// 获取标题包含关键字的帖子数据，从min到max
        /// 返回post_modelList;
        /// </summary>
        /// <param name="min">开始</param>
        /// /// <param name="max">结尾</param>
        /// <param name="searchKeywork">要搜索的关键字</param>
        /// <returns></returns>
        public List<Post_model> GetSearchPost(int min,int max, string searchKeywork)
        {
            string sqlStr = @"select * from(
                                                          select ROW_NUMBER() over(order by id) orderNumber,* from PostView where caption like '%"+searchKeywork+@"%'
                                                               ) as postTable 
                                                      where postTable.orderNumber between @min and @max";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@min",min),
                new SqlParameter("@max",max)
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            List<Post_model> post_modelList = new List<Post_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                post_modelList.Add(ToModel(row));
            }
            return post_modelList;
        }




        /// <summary>
        /// 通过用户名获取某个用户userId的主贴帖子
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 原贴帖子id  标题 内容的前200字符  帖子的第一张图片 日期
        /// 返回post_modelList;
        /// </summary>
        /// <param name="userId">用户id</param>       
        /// <returns></returns>
        public List<Post_model> GetShortPostData(int userId)
        {
            string sqlStr = @"select Post_Id as id,Post_Caption as caption,Post_Content content,Post_Date as [datetime],
                                                   PostPhotoOne.photoUrl  from tb_Post left join 
                                                      (select * from 
                                                            (select PostPhoto_PostId,PostPhoto_PhotoUrl as photoUrl,
                                                             ROW_NUMBER()over(partition by PostPhoto_PostId order by PostPhoto_PostId) as new_index
                                                            from tb_PostPhoto where PostPhoto_PostType=1) as PostPhotoAll  where new_index=1)
                                                     AS PostPhotoOne on tb_Post.Post_Id=PostPhotoOne.PostPhoto_PostId  
                                            WHERE   Post_SenderId=@userId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userId",userId)
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            List<Post_model> post_modelList = new List<Post_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                post_modelList.Add(ToShortModel(row));
            }
            return post_modelList;
        }





        /// <summary>
        /// 获取全部帖子数据
        /// </summary>
        /// <returns></returns>
        public List<Post_model> GetAllPost()
        {
            string sqlStr = "select * from PostView";
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr);
            List<Post_model> post_modelList = new List<Post_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                post_modelList.Add(ToModel(row));
            }
            return post_modelList;

        }

        /// <summary>
        /// 获取帖子编号为postId的数据,一条数据
        /// </summary>
        /// <param name="postId">帖子编号</param>
        /// <returns></returns>
        public Post_model GetOnePost(int postId)
        {
            string sqlStr = "select * from PostView where id=@postId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@postId",postId)
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            Post_model post_model= new Post_model();
            
            post_model = ToModel(dataTable.Rows[0]);
            return post_model;
        }

        
        /// <summary>
        /// 将一条数据转化为Post_model数据
        /// 原贴帖子id  标题 内容的前200字符  帖子的第一张图片 日期
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Post_model ToShortModel(DataRow row)
        {
            Post_model post_model = new Post_model();
            post_model.postId = (int)row["id"];
            post_model.postCaption = row["caption"].ToString();
            post_model.postContent = row["content"].ToString();
            post_model.firstPostPhotoUrl = row["photoUrl"].ToString();
            post_model.postDate = (DateTime)row["datetime"];
            return post_model;
        }


        /// <summary>
        /// 将一条数据转化为Post_model数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Post_model ToModel(DataRow row)
        {
            Post_model post_model = new Post_model();
            post_model.User.userId = (int)row["userId"];
            post_model.User.userName = row["userName"].ToString();
            post_model.User.signature = (row["signature"] != DBNull.Value ? row["signature"].ToString() : null);
            post_model.User.touXiangUrl = row["touXiangUrl"].ToString();
            post_model.postId = (int)row["id"];
            post_model.postCaption = row["caption"].ToString();
            post_model.postContent = row["content"].ToString();
            post_model.firstPostPhotoUrl = row["PostPhoto_PhotoUrl"].ToString();
            post_model.postDate = (DateTime)row["datetime"];
            post_model.postHtmlUrl = row["htmlUrl"].ToString();
            post_model.postSupportCount = (int)row["supportCount"];
            post_model.Theme.themeName = row["themeName"].ToString();
            post_model.Theme.themeId = (int)row["themeId"];
            post_model.commentCount = (row["commentCount"] != DBNull.Value ? (int)row["commentCount"] : 0);
            post_model.tuiTieCount = (row["tuiTieCount"] != DBNull.Value ? (int)row["tuiTieCount"] : 0);
            return post_model;
        }
        
       /// <summary>
       /// 通过原贴的帖子id获取该贴的点赞数
       /// </summary>
       /// <param name="postId"></param>
        public object GetPostSupportCount(int postId)
        {
            string sqlStr="select Post_SupportCount from tb_Post where Post_Id=@postId";
            SqlParameter[]parameters=new SqlParameter[]{
                new SqlParameter("@postId",postId)
            };
            return SqlHelper.ExecuteScalar(sqlStr,parameters);
        }


        /// <summary>
        /// 通过postId更新帖子，让点赞数+1
        /// 返回受影响的行数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int UpdateSupportCountAdd1(int postId)
        {
            string sqlStr = "update tb_Post set Post_SupportCount=Post_SupportCount+1 where Post_Id=@postId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@postId",postId)
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }
        /// <summary>
        /// 通过postId更新帖子，让点赞数-1
        /// 返回受影响的行数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int UpdateSupportCountReduce1(int postId)
        {
            string sqlStr = "update tb_Post set Post_SupportCount=Post_SupportCount-1 where Post_Id=@postId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@postId",postId)
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }


        /// <summary>
        /// 插入数据 标题   内容  提问题的人的编号    主题Id  
        /// 编号从tb_User里面拿，有空在写
        /// 插入成功返回1
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="content">发帖的内容的前200字符</param>
        /// <param name="postsender">发帖者</param>
        /// <param name="themeId">板块：如发型，裤子等</param>
        /// <returns></returns>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        public int insertCaption(string caption, string content, int postsenderId, int themeId, string staticHtmlPath, DateTime datetime)
        {

            string sqlStr = "insert into [tb_Post] ( Post_Caption,Post_Content,Post_SenderId,Post_ThemeId,Post_HtmlUrl,Post_Date) values (@caption,@content,@postsenderId,@themeId,@staticHtmlPath,@datetime)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@caption",caption),
                new SqlParameter("@content",content),
                new SqlParameter("@postsenderId",postsenderId),
                new SqlParameter("@themeId",themeId),
                new SqlParameter("@staticHtmlPath",staticHtmlPath),
                new SqlParameter("@datetime",datetime)
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }


    }
    
}