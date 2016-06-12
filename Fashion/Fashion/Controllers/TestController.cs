using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
          //  ViewData["aa"] = Request.Files[0].FileName;
            //foreach(string file in Request.Files)
            //{
            //    HttpPostedFileBase postFile = Request.Files[file];
            //    ViewData["aa"] = postFile.FileName;
                
            //}
            
            //string filename = Request["aa"].ToString();
            //ViewData["aa"] = filename;
            return View();
        }
        public ActionResult Index3()
        {
            return View();
        }
        public ActionResult yuanyangtest()
        {
            return View();
        }


    }
}
