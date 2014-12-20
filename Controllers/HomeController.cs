using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fengmiapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string Test()
        {
            string str="12";
            string str1 = "12,41,5,455,56,6";
            string[] strArr = str.Split(',');
            string[] str1Arr = str1.Split(',');

            string html = "";
            for (int i = 0; i < strArr.Length; i++)
            {
                html+=(strArr[i]);
            }
            html += ("<hr />");

            for (int i = 0; i < str1Arr.Length; i++)
            {
                html += (str1Arr[i]);

            }
            return html;

        }

        public ActionResult Form()
        {
            return View();
        }
        
        public ActionResult LoginForm()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Send()
        {
            return View();
        }
    
    }
}
