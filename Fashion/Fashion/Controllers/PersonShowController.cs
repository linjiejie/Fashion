using Fashion.Code.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class PersonShowController : Controller
    {
        //
        // GET: /PersonShow/

        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult PersonShowIndex()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }

        public ActionResult VideoShowIndex()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
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
        public ActionResult VideoPlay()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay2()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay3()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay4()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay5()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay6()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay7()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay8()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay9()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult VideoPlay10()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult PictureShowIndex()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
        public ActionResult PicturePlay()
        {
            LoginStatusConfig();//配置登录状态
            return View();
        }
    }
}
