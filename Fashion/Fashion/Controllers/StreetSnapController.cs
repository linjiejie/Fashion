using Fashion.Code.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class StreetSnapController : Controller
    {
        //
        // GET: /StreetSnap/

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult streetHome()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
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
            ViewData["rankName"] = Session["rank"].ToString();
            People_bll peopleBll = new People_bll();
            ViewData["TouXiangUrl"] = peopleBll.GetImgUrlTouXiang(Session["userName"].ToString());//从数据库里获取头像url
            return true;
        }
    }
}
