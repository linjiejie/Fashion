using Fashion.Code.BLL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Fashion.Controllers
{
    public class PostsController : Controller
    {
        //
        // GET: /Posts/


        /// <summary>
        /// 通过Ajax获得4个普通咨询的首张图片和前200个字符
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxHomeGetPuTongPicture()
        {

            Post_bll post_bll = new Post_bll();
            JavaScriptSerializer serializer1 = new JavaScriptSerializer();

            string json = serializer1.Serialize(post_bll.GetFourPuTongPost());
            //return Content("1");
            return Content(json);//返回一个json数据
        }






        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 收藏帖子
        /// 从前端接收4个参数提交到Post_dal
        /// 返回1更新成功
        /// 返回0更新失败
        /// 返回2提示登录
        /// </summary>
        /// <returns></returns>
        public ActionResult ShouCangTieZi() 
        {
            if (Session["userName"] == null)
            {
                return Content("2");
            }
            string userName = Request["userName"];
            string postId = Request["postId"];
            string postType = Request["postType"];
            string Num = Request["Num"];
            Post_bll post_bll = new Post_bll();
            return Content(post_bll.check_ShouCangTieZi(userName, postId, postType, Num).ToString());
        
        
        }



        /// <summary>
        /// 在搜索框里输入关键字搜索帖子
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchPosts()
        {
            string searchKeyWord = Request["searchKeyWord"].ToString();


            if (Session["userName"] == null)
            {
                return View("../Topic/LoginRemind");
            }
            Post_bll post_bll = new Post_bll();
            List<Post_model> post_modelList = post_bll.GetSearchPost(1, 20, searchKeyWord);
            LoginStatusConfig();          //配置登录状态            
            //return View(post_modelList);
            return View("../Topic/Home",post_modelList);
        }

        /// <summary>
        /// 对主贴的回帖
        /// </summary>
        /// <returns></returns>
        public ActionResult MakePostReply()
        {
            ////--------先把前端传回来的content内容保存为静态页面
            byte[] byteData = new byte[Request.InputStream.Length]; //定义一个字节数组保存前端传回来的Post数据全部数据
            Request.InputStream.Read(byteData, 0, byteData.Length);//将流读取到byteData，InputStream读取到的是http头里的主体数据
            //string postData = System.Text.Encoding.Default.GetString(byteData);//系统的默认编码为gb2312,不适用这种
            string postData = System.Text.Encoding.UTF8.GetString(byteData);
            postData = Server.UrlDecode(postData);//对数据进行url解码
            string[] datas = postData.Split('&');//对前端传回来的数据进行分割，提取出文本框里的html数据
            string contentData = datas[0].ToString(); //data[0]为变量名为content的内容
            contentData = contentData.Substring(contentData.IndexOf('=') + 1);//去除变量名，如content=aaa，只取出aaa
            DateTime datetime = DateTime.Now;
            string fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";//定义文件名fileName
            string fileNamePath = Server.MapPath("~/StaticHtml/HuiTieHtml/") + fileName;//物理路径
            while (System.IO.File.Exists(fileNamePath))//先判断文件是否存在，若存在：更换文件名
            {
                datetime = DateTime.Now;
                fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";
                fileNamePath = Server.MapPath("~/StaticHtml/HuiTieHtml/") + fileName;
            }
            System.IO.FileStream fs = new System.IO.FileStream(fileNamePath, System.IO.FileMode.Create);
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(contentData);
            //byte[] contentBytes = System.Text.Encoding.Default.GetBytes(contentData);
            fs.Write(contentBytes, 0, contentBytes.Length);
            fs.Close();//---------保存静态html成功
            ///----------/将数据保存到数据库里tb_ReplyPost （主贴编号 回帖者Id 
            ///回帖内容的前200字符 点赞数 日期 回帖的静态html）
            int postId = Convert.ToInt32(Request["hidPostId"]); //主贴id
            int supportCount = 0;//点赞数
            string replyUserName = Request["hidHuiTieUserName"].ToString();
            User_bll user_bll=new User_bll();
            int replyUserId = user_bll.GetUserId(replyUserName);//回帖者id
            string editorContent = datas[3].ToString();//回帖内容的纯文本包含图片，data[3]的为前端传回来的发帖内容的纯文本 （备注：不能从request["editorContent"]读取，会报错提示说存在html标签）
            editorContent = editorContent.Substring(editorContent.IndexOf('=') + 1);
            System.Text.RegularExpressions.Regex regexImg = new System.Text.RegularExpressions.Regex(@"<img[^>]+>");
            editorContent = regexImg.Replace(editorContent, "");//过滤掉editorContent里图片
            int len = editorContent.Length;
            if (len > 200)//如果editorContent的长度超过200,取editorContent里的前两百个字符，将用于保存到数据库
            {
                len = 200;
            }
            string content200 = editorContent.Substring(0, len);  //回帖的200字符
            string staticHuiTieHtml = "/StaticHtml/HuiTieHtml/" + fileName;//相对路径
            ReplyPost_bll replyPost_bll = new ReplyPost_bll();
            replyPost_bll.InsertReplyPost(postId, replyUserId, content200, supportCount, datetime, staticHuiTieHtml);
            //--------回帖数据保存成功

            //////获取回帖里的所有图片的路径,并且将图片路径保存到数据库里tb_PostPhoto （PostPhoto_PostType=）
            int replyPostId = replyPost_bll.GetReplyPostId(staticHuiTieHtml);//根据回帖的静态html路径查询数据库，得到该贴子的replyPost_Id
            System.Text.RegularExpressions.Regex regImg2 = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);// 定义正则表达式用来匹配 img 标签
            System.Text.RegularExpressions.MatchCollection matches = regImg2.Matches(contentData);
            int i = 0;
            string[] strUrlList = new string[matches.Count];
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                strUrlList[i++] = match.Groups["imgUrl"].Value;// 取得匹配项列表
            }
            if (strUrlList.Length >= 1)
            {
                PostPhoto_bll postPhoto_bll = new PostPhoto_bll();
                if (postPhoto_bll.InsertPhotoUrl(replyPostId, strUrlList, 2) < 0)
                {
                    return Content("保存图片路径时数据库出错");
                }
            }
            return RedirectToAction("../Topic/PostDetails", new { postId = postId });
        }

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
            People_bll peopleBll = new People_bll();
            ViewData["TouXiangUrl"] = peopleBll.GetImgUrlTouXiang(Session["userName"].ToString());//从数据库里获取头像url
            return true;
        }

    }
}
