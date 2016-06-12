using Fashion.Code.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class ShangChengController : Controller
    {
        //
        // GET: /ShangCheng/

        public ActionResult Index()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult ClothDetails()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index1()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index2()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index3()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index4()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index5()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index6()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index7()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index8()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index9()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index10()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index11()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index12()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index13()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index14()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index15()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index16()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index17()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index18()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index19()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index20()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index21()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index22()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index23()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index24()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index25()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index26()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index27()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index28()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index29()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index30()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index31()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index32()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index33()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index34()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index35()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
        }
        public ActionResult Index36()
        {
            LoginStatusConfig();          //配置登录状态     
            return View();
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
