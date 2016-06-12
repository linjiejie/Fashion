using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Fashion.Models;
using Fashion.Code.BLL;
using Fashion.Code.DAL;
namespace Fashion.Controllers
{
   
  
    public class TopicController : Controller
    {
        //
        // GET: /Topic/  

        public ActionResult HomeTWO()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }

        




        public ActionResult PostDetails()
        {
         
            LoginStatusConfig();          //配置登录状态            
            int postId = Convert.ToInt32(Request["postId"]);
            //获取postId的原帖数据
            Post_bll post_bll = new Post_bll();
            Post_model post_mode = post_bll.GetOnePost(postId);
            ViewData["post_mode"] = post_mode;
            //获取对postId原帖的回帖数据
            ReplyPost_bll replyPost_bll =new ReplyPost_bll();
            List<ReplyPost_model> replyPost_modelList = replyPost_bll.GetReplyPost(postId);
            return View(replyPost_modelList);
        }
        public ActionResult ExpertReply()
        {

            return View();
        }
        public ActionResult Index()
        {
            
            return View();
        }
        /// <summary>
        /// 返回特定咨询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Consult()
        {
            if (Session["username"] == null)
            {
                return View("loginremind");
            }
            string userName = Session["username"].ToString();
            LoginStatusConfig();//配置登录状态


            User_bll user_bll = new User_bll();
            int userId = Convert.ToInt32(user_bll.GetUserId(userName));//通过用户名获取userId
            ////获取用户等级名
            Rank_bll rank_bll = new Rank_bll();
            string rankNameDB = rank_bll.GetRankName(userName);//该用户数据库里的等级名
            rankNameDB = rankNameDB.Trim();//去除空格
            CountUser_model countUser_model = user_bll.GetCountUser(userId, rankNameDB);//获取用户的CountUser_model 数据：点赞数 关注数 粉丝数 收藏数 提问数 回帖数 特定咨询数 等          
            ViewData["countUser_model"] = countUser_model;     
            List<ExpertUserConsult_model> expertUserConsult_modelList = user_bll.GetExpertConsult();  //获取专家的ExpertUserConsult_model数据,用户填写特定咨询时，需要选择专家
            User_model user_model = new User_model();
            try { 
                user_model = user_bll.GetUserDataConsult(userName);//用户的个人数据
            }
            catch(Exception e)
            {
                return Content(e.ToString());
            }
            
            ViewData["user_model"] = user_model;
            return View(expertUserConsult_modelList);
        }

        /// <summary>
        /// 保存特定咨询的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult MakeConsult()
        {
            if (Session["username"] == null)
            {
                return View("loginremind");
            }
            string userName = Session["username"].ToString();
            User_bll user_bll = new User_bll();
            int userId = user_bll.GetUserId(userName);
            //string expertName=Request[].ToString();
            //int expertId = user_bll.GetUserId(expertName);
            string occasion = Request["occasion"].ToString();//场合
            string details = Request["details"].ToString();//特定咨询详情
            DateTime datetime = DateTime.Now;
            //保存个人照片到文件夹：GeRenZhao
            byte[] imgGeRenZhao64Byte = Convert.FromBase64String(Request["geRenZhao"]);//将图片数据转化为base64的格式
            System.IO.MemoryStream ms = new System.IO.MemoryStream(imgGeRenZhao64Byte);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(ms);
            string geRenZhaoFileName = Guid.NewGuid().ToString() + ".png";//唯一的文件名
            bitmap.Save(Server.MapPath("~/Images/ConsultImages/GeRenZhao/" + geRenZhaoFileName), System.Drawing.Imaging.ImageFormat.Png);
            //保存喜欢风格的照片到文件夹：LikeStyleImage
            byte[] likeStyleImageBase64 = Convert.FromBase64String(Request["likeStyleImage"]);//将图片数据转化为base64的格式
            System.IO.MemoryStream ms2 = new System.IO.MemoryStream(likeStyleImageBase64);
            System.Drawing.Bitmap bitmap2 = new System.Drawing.Bitmap(ms2);
            string likeStyleImageFileName = Guid.NewGuid().ToString() + ".png";//唯一的文件名
            bitmap2.Save(Server.MapPath("~/Images/ConsultImages/LikeStyleImage/" + likeStyleImageFileName), System.Drawing.Imaging.ImageFormat.Png);
            //保存不喜欢风格的照片到文件夹：DislikeStyleImage
            byte[] dislikeStyleImageBase64 = Convert.FromBase64String(Request["dislikeStyleImage"]);//将图片数据转化为base64的格式
            System.IO.MemoryStream ms3 = new System.IO.MemoryStream(dislikeStyleImageBase64);
            System.Drawing.Bitmap bitmap3 = new System.Drawing.Bitmap(ms3);
            string dislikeStyleImageFileName = Guid.NewGuid().ToString() + ".png";//唯一的文件名
            bitmap3.Save(Server.MapPath("~/Images/ConsultImages/DislikeStyleImage/" + dislikeStyleImageFileName), System.Drawing.Imaging.ImageFormat.Png);

            SpecialConsult_bll specialConsult_bll = new SpecialConsult_bll();//保存特定咨询数据
            specialConsult_bll.InsertConsultData(userId, occasion, details, geRenZhaoFileName, likeStyleImageFileName, dislikeStyleImageFileName,datetime);
            //通过geRenZhaoFileName查询该咨询的id
            int specialConsult_Id = specialConsult_bll.GetSpecialConsultId(geRenZhaoFileName);
            //将用户特定咨询时选择的专家，添加到数据库
            SpecialConsultSelectExperts_bll specialConsultSelectExperts_bll = new SpecialConsultSelectExperts_bll();
            List<string> expertIdList = new List<string>();
            string expertIdStr = Request["expertIdList"].ToString();//获取用户选择的专家id
            foreach (string expertId in expertIdStr.Split(','))
            {
                expertIdList.Add(expertId);
            }
            specialConsultSelectExperts_bll.InsertSpecialConsultSelectExperts(specialConsult_Id, expertIdList);
            return Content(specialConsult_Id.ToString());//返回specialConsult_Id
        }
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult Test2( )
        {
            //上传头像
            var path = System.IO.Path.Combine(Server.MapPath("~/test"), Request.Files[0].FileName);
            Request.Files[0].SaveAs(path);
            return Content("成功");
        }
        
        
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
         
            Post_bll post_bll = new Post_bll();
            List<Post_model>post_modelList=post_bll.GetPost(1,50);
            LoginStatusConfig();          //配置登录状态            
            return View(post_modelList);
        }
        /// <summary>
        /// 获取Home页面的数据，帖子遍历的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxHomeGetData()
        {
            //string theme = "{ 'name': 'dong' }";
            List<Dictionary<string,object>>list=new List<Dictionary<string,object>>();
            Dictionary<string,object>dic=new Dictionary<string,object>();
            dic.Add("name1","dong");
            //list.Add(dic);
            dic.Add("name2","xu");
            list.Add(dic);
            Dictionary<string, object> dic2 = new Dictionary<string, object>();
            dic2.Add("name3", "dong2");
            //list.Add(dic);
            dic2.Add("name4", "xu2");
            list.Add(dic2);
            JavaScriptSerializer serializer1 = new JavaScriptSerializer();
            string json = serializer1.Serialize(list);
            return Content(json);
            //return Content(theme);
        }


        /// <summary>
        /// 实现原贴的点赞
        /// 成功返回1
        /// 失败返回0
        /// 返回2，提示登录
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxPostSupportCountAdd1()
        {
            //if (Session["userName"] == null)
            //    return Content("2");

            string postIdStr=Request["postId"].ToString();
            int postId=Convert.ToInt32(postIdStr);
            Post_bll post_bll = new Post_bll();
            if(post_bll.UpdateSupportCountAdd1(postId)<1)
            {
                return Content("0");
            }
            return Content("1");
        }
        /// <summary>
        /// 实现原贴的取消点赞
        /// 成功返回1
        /// 失败返回0
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxPostSupportCountReduce1()
        {
           
            string postIdStr = Request["postId"].ToString();
            int postId = Convert.ToInt32(postIdStr);
            Post_bll post_bll = new Post_bll();
            if (post_bll.UpdateSupportCountReduce1(postId) < 1)
            {
                return Content("0");
            }
            return Content("1");
        }
        /// <summary>
        /// 获取评论的数据
        /// 页面：Home
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxGetCommentData()
        {           
            int postId = Convert.ToInt32(Request["postId"]);
            int postType = Convert.ToInt32(Request["postType"]);
            PostComment_bll postComment_bll = new PostComment_bll();
            List<PostComment_model> postComment_modelList = postComment_bll.GetPostComment(postId,postType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(postComment_modelList);
            return Content(jsonData);
        }


        /// <summary>
        /// 实现帖子的评论功能,将数据保存到数据库
        /// 帖子类型为1代表主贴的评论
        /// 帖子类型为2代表回帖的评论
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxTieZiComment()
        {
            int postId = Convert.ToInt32(Request["postId"]);
            string commentUserName = Request["commenterUserName"].ToString();
            User_bll user_bll=new User_bll();
            int commenterId = user_bll.GetUserId(commentUserName);
            string beCommenterUserName = Request["beCommenterUserName"];
            int beCommenterId = user_bll.GetUserId(beCommenterUserName);
            string content = Request["content"].ToString();
            int postType = Convert.ToInt32(Request["postType"]);
            DateTime datetime = DateTime.Now;
            PostComment_bll postComment_bll = new PostComment_bll();
            int InsertCount = postComment_bll.InsertComment(postId, commenterId, beCommenterId, content, datetime, postType);//插入评论数据的成功条数
            return Content(InsertCount.ToString());
            
        }

        /// <summary>
        /// 未登录提醒页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginRemind()
        {
            return View();
        }
        
        public ActionResult Answer()
        {
            //ViewData["a"]=Request["type"].ToString();
            //ViewData["a"]="a";
            return View();
        }
        public ActionResult AjaxAnswer()
        {
            return Content("HelloWorld This is Answer");
        }
        public ActionResult Post()
        {
            LoginStatusConfig();//验证登录
            if (Session["userName"] == null)
            {
                return View("LoginRemind");
            }
            return View();
        }
        public ActionResult fabu()
        {
            return View();
        }

        /// <summary>
        /// 提交数据
        /// 分为3个步骤：
        /// 1.将前端传回来的content保存为静态html
        /// 2.将帖子标题，内容的前200个字符保存到数据库
        /// 3.将帖子的图片的路径保存到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult postData()
        {
            
            ////先把前端传回来的content内容保存为静态页面
            byte[] byteData = new byte[Request.InputStream.Length]; //定义一个字节数组保存前端传回来的Post数据全部数据
            Request.InputStream.Read(byteData, 0, byteData.Length);//将流读取到byteData，InputStream读取到的是http头里的主体数据
            //string postData = System.Text.Encoding.Default.GetString(byteData);//系统的默认编码为gb2312,不适用这种
            string postData = System.Text.Encoding.UTF8.GetString(byteData);
            string[] datas = postData.Split('&');//对postData数据进行分割，提取出发帖内容里的html数据
            string contentData = datas[1].ToString(); //data[1]为变量名为content的内容
            contentData = contentData.Substring(contentData.IndexOf('=') + 1);//去除变量名，如content=aaa，只取出aaa
            contentData = Server.UrlDecode(contentData);//对数据进行url解码,这个解码的操作要放在Split('&')之后，因为可能content里会含有&符号
            DateTime datetime = DateTime.Now;
            string fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";//定义文件名fileName
            string fileNamePath = Server.MapPath("~/StaticHtml/TieZiHtml/") + fileName;//物理路径
            while (System.IO.File.Exists(fileNamePath))//先判断文件是否存在，若存在：更换文件名
            {
                datetime = DateTime.Now;
                fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";
                fileNamePath = Server.MapPath("~/StaticHtml/TieZiHtml/") + fileName;
            }
            System.IO.FileStream fs = new System.IO.FileStream(fileNamePath,System.IO.FileMode.Create);
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(contentData);
            //byte[] contentBytes = System.Text.Encoding.Default.GetBytes(contentData);
            fs.Write(contentBytes,0,contentBytes.Length);
            fs.Close();//保存静态html成功
            ///////将帖子数据保存到数据库
            Post_bll Post = new Post_bll();
            Theme_bll themeName = new Theme_bll();
            People_bll User = new People_bll();
            string caption = Request["question"].ToString();
            string userName = Session["userName"].ToString();
            int userId = User.GainUserId(userName);
            string theme = Request["theme"].ToString();
            int themeId = themeName.CollocateThemeId(theme);
            string staticHtmlPath = "/StaticHtml/TieZiHtml/" + fileName;//相对路径
            string editorContent = datas[3].ToString();//data[3]的为前端传回来的发帖内容的纯文本
            contentData = Server.UrlDecode(editorContent);//对数据进行url解码,这个解码的操作要放在Split('&')之后，因为可能content里会含有&符号
            editorContent = contentData.Substring(contentData.IndexOf('=') + 1);
            System.Text.RegularExpressions.Regex regexImg = new System.Text.RegularExpressions.Regex(@"<img[^>]+>");
            editorContent = regexImg.Replace(editorContent, "");//过滤掉editorContent里图片
            int len = editorContent.Length;
            if (len > 200)//如果editorContent的长度超过200,取editorContent里的前两百个字符，将用于保存到数据库
            {
                len = 200;
            }

            string content200 = editorContent.Substring(0, len);  
            if (Post.finshInsert(caption, content200, userId, themeId, staticHtmlPath,datetime) != 1)
            {
                return Content("保存帖子信息时数据库出错");
            }//将帖子数据保存到数据库---------成功
            //////获取所有图片里的图片路径,并且将图片路径保存到数据库里


            int postId = Post.GetPostIdBy_PostHtmlUrl(staticHtmlPath); //根据帖子的标题查询数据库，得到该贴子的postId
            System.Text.RegularExpressions.Regex regImg2 = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);// 定义正则表达式用来匹配 img 标签
            System.Text.RegularExpressions.MatchCollection matches = regImg2.Matches(contentData);            
            int i = 0;
            string[] strUrlList = new string[matches.Count];
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                strUrlList[i++] = match.Groups["imgUrl"].Value;// 取得匹配项列表
            }
            if(strUrlList.Length>=1)
            {
                PostPhoto_bll postPhoto_bll = new PostPhoto_bll();
                if (postPhoto_bll.InsertPhotoUrl(postId, strUrlList,1) < 0)
                {
                    return Content("保存图片路径时数据库出错");
                }
            }
            return RedirectToAction("PostDetails", new { postId=postId});
        }
        ///// <summary>
        ///// 验证登录是否成功；
        ///// 若登录失败，设置ViewData["LoginYes"] = 0；
        ///// 若登录成功，设置ViewData["LoginYes"] = 1；并且从数据库里取出用户头像的链接：ViewData["TouXiangUrl"] =...；
        ///// </summary>
        //public void CheckLogin()
        //{
        //    if (Session["userName"] == null)
        //    {//未登录
        //        ViewData["LoginYes"] = 0;

        //    }
        //    else
        //    {//已登录
        //        ViewData["LoginYes"] = 1;
        //        ViewData["userName"] = Session["userName"].ToString();
        //        People_bll peopleBll = new People_bll();
        //        ViewData["TouXiangUrl"] = peopleBll.GetImgUrlTouXiang(Session["userName"].ToString());//从数据库里获取头像url
        //    }
        //}






        /// <summary>
        /// 配置用户登录状态
        /// 如果已登录，返回true，并且设置登录状态：设置ViewData["LoginYes"] = 1；并且从数据库里取出用户头像的链接：ViewData["TouXiangUrl"] =...；
        /// 未登录返回false,并且设置ViewData["LoginYes"] = 0
        /// </summary>
        public bool LoginStatusConfig()
        {
            if (Session["userName"] == null)
            {//未登录
                ViewData["LoginYes"] = 0;
                return false;
            }
            //已登录
            ViewData["LoginYes"] = 1;
            ViewData["userName"] = Session["userName"].ToString();
            ViewData["signature"] = Session["signature"].ToString();
            ViewData["rankName"] = Session["rank"].ToString();
            People_bll peopleBll = new People_bll();
            ViewData["TouXiangUrl"] = peopleBll.GetImgUrlTouXiang(Session["userName"].ToString());//从数据库里获取头像url
            return true;
        }

    }
}
