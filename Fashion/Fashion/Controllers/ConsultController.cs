using Fashion.Code.BLL;
using Fashion.Code.DAL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Fashion.Controllers
{
    public class ConsultController : Controller
    {
        //
        // GET: /Consult/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 返回专家特定咨询解答页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExpertAnswer()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginRemind", "Topic");
            }
            LoginStatusConfig();//配置登录状态
            string expertUserName = Request["expertUserName"].ToString();
            int specialConsultId = Convert.ToInt32(Request["specialConsultId"]);
            //检查用户是否为专家
            Rank_bll rank_bll = new Rank_bll();
            string rankName=rank_bll.GetRankName(expertUserName);//获取用户的等级名称
            rankName = rankName.Trim();//去除字符串里的空格
            if(rankName!="专家")
            {
                return Content("0");
            }            
            //获取specialConsultId的特定咨询的数据
            SpecialConsult_bll specialConsult_bll=new SpecialConsult_bll();
            SpecialConsult_model specialConsult_model = specialConsult_bll.GetOneSpecialConsult(specialConsultId);
            return View(specialConsult_model);
        }
        /// <summary>
        /// 专家对特定咨询的回帖，当点击提交时调用此函数
        /// </summary>
        /// <returns></returns>
        public ActionResult MakeExpertAnswer()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginRemind", "Topic");
            }
            LoginStatusConfig();//配置登录状态
            string expertUserName = Session["userName"].ToString();
            //检查用户是否为专家
            Rank_bll rank_bll = new Rank_bll();
            string rankName = rank_bll.GetRankName(expertUserName);//获取用户的等级名称
            rankName = rankName.Trim();//去除字符串里的空格
            if (rankName != "专家")
            {
                return Content("您不是专家");
            }
            User_bll user_bll = new User_bll();
            int expertId = user_bll.GetUserId(expertUserName);
            ////先把前端传回来的content内容保存为静态页面
            byte[] byteData = new byte[Request.InputStream.Length]; //定义一个字节数组保存前端传回来的Post数据全部数据
            Request.InputStream.Read(byteData, 0, byteData.Length);//将流读取到byteData，InputStream读取到的是http头里的主体数据
            //string postData = System.Text.Encoding.Default.GetString(byteData);//系统的默认编码为gb2312,不适用这种
            string postData = System.Text.Encoding.UTF8.GetString(byteData);
            string[] datas = postData.Split('&');//对前端传回来的数据进行分割，提取出文本框里的html数据
            string contentData = datas[2].ToString(); //data[0]为变量名为content的内容
            contentData = contentData.Substring(contentData.IndexOf('=') + 1);//去除变量名，如content=aaa，只取出aaa
            contentData = Server.UrlDecode(contentData);//对数据进行url解码,这个解码的操作要放在Split('&')之后，因为可能content里会含有&符号
            DateTime datetime = DateTime.Now;
            string fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";//定义文件名fileName
            string fileNamePath = Server.MapPath("~/StaticHtml/ConsultAnswerHtml/") + fileName;//物理路径
            while (System.IO.File.Exists(fileNamePath))//先判断文件是否存在，若存在：更换文件名
            {
                datetime = DateTime.Now;
                fileName = datetime.ToString("yyyyMMddHHmmss_ffff") + ".html";
                fileNamePath = Server.MapPath("~/StaticHtml/ConsultAnswerHtml/") + fileName;
            }
            System.IO.FileStream fs = new System.IO.FileStream(fileNamePath, System.IO.FileMode.Create);
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(contentData);
            //byte[] contentBytes = System.Text.Encoding.Default.GetBytes(contentData);
            fs.Write(contentBytes, 0, contentBytes.Length);
            fs.Close();//保存静态html成功
            
            ////将数据保存到数据库里tb_SpecialConsultAnswer  SpecialConsultAnswer_SpecialConsultId   SpecialConsultAnswer_HtmlUrl
            int specialConsultId = Convert.ToInt32(Request["hidSpecialConsultId"]);//用户特定咨询的id
            string staticConsultAnswerHtml = "/StaticHtml/ConsultAnswerHtml/" + fileName;//相对路径
            SpecialConsult_bll specialConsult_bll = new SpecialConsult_bll();

            specialConsult_bll.InsertAnswerData(specialConsultId, expertId,staticConsultAnswerHtml, datetime);          
            
            ////保存购买链接
            int specialConsultAnswer_Id = specialConsult_bll.GetConsultAnswerId(staticConsultAnswerHtml);            
            int selectCount = Convert.ToInt32(Request["selectCount"]);//代表购买链接的条数
            Dictionary<string, string> selectClothUrlDic = new Dictionary<string, string>();  //服饰类型名：购买链接
            for (int i = 1; i <= selectCount; i++)
            {
                try
                {
                    selectClothUrlDic.Add(Request["selectCloth" + i].ToString(), Request["clothShoppingUrl" + i].ToString());
                }
                catch (Exception e)
                {
                    return Content("<script type='text/javascript'>alert('请不要出入两种同样的服饰,请重新操作');window.location.href = '../Consult/ExpertAnswer';</script>");
                }
            }
            SpecialConsultAnswerClothes_bll specialConsultAnswerClothes_bll = new SpecialConsultAnswerClothes_bll();
            specialConsultAnswerClothes_bll.InsertConsultAnswerClothes(specialConsultAnswer_Id,selectClothUrlDic);
            return RedirectToAction("ConsultDetails", new { specialConsultId = specialConsultId, expertUserName=expertId });
        }

        /// <summary>
        /// 用户特定咨询从专家中挑选一个专家，这里获取专家的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExpertConsult()
        {            
            User_bll user_bll = new User_bll();
            List<ExpertUserConsult_model> expertUserConsult_modelList = user_bll.GetExpertConsult();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(expertUserConsult_modelList);
            return Content(jsonData);
        }

        /// <summary>
        /// 显示特定咨询选择的专家列表解答情况
        /// </summary>
        /// <returns></returns>
        public ActionResult MySpecialConsultExpertList()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginRemind", "Topic");
            }
            LoginStatusConfig();//配置登录状态
            int specialConsultId = Convert.ToInt32(Request["specialConsultId"]);//特定咨询帖子的id
            ViewData["specialConsultId"] = specialConsultId;
            SpecialConsult_bll specialConsult_bll = new SpecialConsult_bll();
            SpecialConsult_model specialConsult_model=specialConsult_bll.GetOneShortConsultData(specialConsultId);//获取的咨询的标题，场合，内容以及时间
            ViewData["specialConsult_model"] = specialConsult_model;
            SpecialConsultAnswer_bll specialConsultAnswer_bll = new SpecialConsultAnswer_bll();
            List<SpecialConsultAnswer_model> specialConsultAnswer_modelList = specialConsultAnswer_bll.GetAllSelectExpertShortAnswer(specialConsultId);//获取选择的多个专家的解答数据
            return View(specialConsultAnswer_modelList);
        }

        //特定咨询详情页面（用户和专家共用）
        public ActionResult ConsultDetails()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginRemind", "Topic");
            }
            LoginStatusConfig();//配置登录状态
            string userName = Session["userName"].ToString();
            ////获取用户等级名
            Rank_bll rank_bll = new Rank_bll();
            string rankNameDB = rank_bll.GetRankName(userName);//该用户数据库里的等级名
            rankNameDB = rankNameDB.Trim();//去除空格
            //string expertUserName = Request["expertUserName"].ToString();
            int specialConsultId = Convert.ToInt32(Request["specialConsultId"]);  
            SpecialConsult_bll specialConsult_bll = new SpecialConsult_bll();
            SpecialConsult_model specialConsult_model = specialConsult_bll.GetOneSpecialConsult(specialConsultId);//通过specialConsultId获取用户特定咨询时填写的特定咨询数据
            ViewData["specialConsult_model"] = specialConsult_model;
            ViewData["rankName"] = rankNameDB;
            SpecialConsultAnswer_bll specialConsultAnswer_bll = new SpecialConsultAnswer_bll();
            string expertUserName = Request["expertUserName"].ToString();
            User_bll user_bll = new User_bll();
            int expertId = user_bll.GetUserId(expertUserName);
            SpecialConsultAnswer_model specialConsultAnswer_model = specialConsultAnswer_bll.GetOneSpecialAnswerData(specialConsultId, expertId);//通过specialConsultId获取特定咨询的专家解答数据
            return View(specialConsultAnswer_model);            
        }

        /// <summary>
        /// 通过解答的数据id specialAnswerId获取该数据的搭配的衣服
        /// 返回json格式的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSpecialAnswerClothes()
        {
            int specialAnswerId=Convert.ToInt32(Request["specialAnswerId"]);            
            SpecialConsultAnswerClothes_bll specialConsultAnswerClothes_bll = new SpecialConsultAnswerClothes_bll();
            List<SpecialConsultAnswerClothes_model>  specialConsultAnswerClothes_modelList=specialConsultAnswerClothes_bll.GetSpecialAnswerClothes(specialAnswerId);
            //接下来专家为json数据
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(specialConsultAnswerClothes_modelList);
            return Content(jsonData);
        }
        /// <summary>
        /// 配置用户登录状态
        /// 如果已登录，返回true，并且设置登录状态：设置ViewData["LoginYes"] = 1；并且从数据库里取出用户头像的链接：ViewData["TouXiangUrl"] =...；
        /// 未登录返回false,并且设置ViewData["LoginYes"] = 0
        /// </summary>
        /// 
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
