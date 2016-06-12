using Fashion.Code.DAL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Code.BLL
{
    public class Post_bll
    {

        /// <summary>
        /// 获取4张普通咨询的帖子
        /// </summary>
        /// <returns></returns>
        public List<Post_model> GetFourPuTongPost()
        {
            Post_dal post_dal = new Post_dal();
            List<Post_model> post_model = post_dal.GetFourPuTongPost();
            return post_model;
        }









        /// <summary>
        /// 执行收藏，取消收藏或不执行操作的逻辑判断
        /// 执行操作返回1，无操作返回0
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="postId"></param>
        /// <param name="postType"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public int check_ShouCangTieZi(string userName,string postId,string postType,string Num){
                    User_bll user_bll = new User_bll();
                    string userId = user_bll.GetUserId(userName).ToString();
                    Post_dal post1 = new Post_dal();
                    object type=post1.select_ShouCang(userId,postId);
                    if (type == null && Num == "1")//无主帖记录，只可插入不可删除
                        return post1.insert_ShouCang(userId, postId, postType);
                 
                    else if (Convert.ToInt32(type) == 1 && Num == "0")//有主帖记录，只可删除不可插入
                        return post1.delete_ShouCang(postId,postType);
                    else return 0; 
         }



        /// <summary>
        /// 判断是否存在该标题
        /// 结果返回1代表存在
        /// 结果返回0代表不存在
        /// 结果返回2代表数据库出错，因为数据库存在超过2条数据的该标题
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public int havingCaption(string caption)
        {
            Post_dal Caption = new Post_dal();
            int accountCount = (int)Caption.getCaptionCount(caption);
            if (accountCount <= 0)
            {
                return 0;
            }
            if (accountCount > 1)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 通过标题查询数据库获得postId
        /// 结果返回1代表数据库出错
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public int GetPostId(string caption)
        {
            Post_dal post_dal = new Post_dal();
            object postId = post_dal.GetPostId(caption);
            if (postId == null || postId == System.DBNull.Value)
            {
                return 0;
            }
            return (int)postId;
        }
        /// <summary>
        /// 通过静态post页面url查询数据库获得postId
        /// 结果返回1代表数据库出错
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public int GetPostIdBy_PostHtmlUrl(string postHtmlUrl)
        {
            Post_dal post_dal = new Post_dal();
            object postId = post_dal.GetPostIdBy_PostHtmlUrl(postHtmlUrl);
            if (postId == null || postId == System.DBNull.Value)
            {
                return 0;
            }
            return (int)postId;
        }

        /// <summary>
        /// 通过原贴的帖子id获取该贴的点赞数
        /// 返回-1代表数据库不存在该条数据,即查询到0行
        /// 返回-2代表数据库查询到了数据，但是此字段为空
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int GetSupportCount(int postId)
        {
            Post_dal post_dal = new Post_dal();
            object postSupportCount = post_dal.GetPostSupportCount(postId);
            if(postSupportCount==null)
            {
                return -1;//数据库不存在该条数据,即查询到0行
            }
            if (postSupportCount == System.DBNull.Value)
            {
                return -2;//数据库查询到了数据，但是此字段为空
            }
            return Convert.ToInt32(postSupportCount);   
        }
          /// <summary>
        /// 通过postId更新帖子，让点赞数+1
        /// /// 返回受影响的行数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int UpdateSupportCountAdd1(int postId)
        {
            Post_dal post_dal = new Post_dal();
            return post_dal.UpdateSupportCountAdd1(postId);
            
        }
        /// <summary>
        /// 通过postId更新帖子，让点赞数-1
        /// /// 返回受影响的行数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int UpdateSupportCountReduce1(int postId)
        {
            Post_dal post_dal = new Post_dal();
            return post_dal.UpdateSupportCountReduce1(postId);

        }

        /// <summary>
        /// 获取帖子的指定条数据
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="count">每页数据的条数</param>       
        public List<Post_model> GetPost(int page, int count)
        {
            int min = (page - 1) * count + 1;//开始
            int max = page * count;//结尾
            Post_dal post_dal = new Post_dal();
            return post_dal.GetPost(min, max);
        }

        /// <summary>
        /// 获取标题包含关键字的帖子数据
        /// 返回post_modelList;
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="count">数量</param>
        /// <param name="searchKeywork">要搜索的关键字</param>
        /// <returns></returns>
        public List<Post_model> GetSearchPost(int page, int count, string searchKeywork)
        {
            int min = (page - 1) * count + 1;//开始
            int max = page * count;//结尾
            Post_dal post_dal = new Post_dal();
            return post_dal.GetSearchPost(min,max,searchKeywork);
    
        }



        /// <summary>
        /// 通过用户名获取某个用户userId的主贴帖子
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 特定咨询帖子id  标题 内容的前200字符  帖子的第一张图片 日期
        /// 返回post_modelList;
        /// </summary>
        /// <param name="userId">用户id</param>       
        /// <returns></returns>
        public List<Post_model> GetShortPostData(int userId)
        {
            Post_dal post_dal=new Post_dal();
            List<Post_model> post_modelList = post_dal.GetShortPostData(userId);
            return post_modelList;
        }

        /// <summary>
        /// 获取帖子编号为postId的数据,一条数据
        /// </summary>
        /// <param name="postId">帖子编号</param>
        /// <returns></returns>
        public Post_model GetOnePost(int postId)
        {
            Post_dal post_dal = new Post_dal();
            return post_dal.GetOnePost(postId);
        }

        /// <summary>
        /// 插入问题，问题说明，提问人
        /// 成功返回1
        /// 失败返回0
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        /// <param name="postsender"></param>
        /// <returns></returns>
        public int finshInsert(string caption, string content, int postsenderId, int themeId, string staticHtmlPath, DateTime datetime)
        {
            Post_dal insert = new Post_dal();
            if (insert.insertCaption(caption, content, postsenderId, themeId,staticHtmlPath, datetime) == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }


        

    }
}