using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fengmiapp.Controllers
{
    public class CommonController : ApiController
    {
        /*
        [HttpGet]
        public IEnumerable test()
        {

            object obj = new object();
            List<object> objList = new List<object>();
            for (int i = 0; i < 10; i++)
            {
                string temp = "name" + i;
                obj = new { id = i, name = temp, };
                objList.Add(obj);
            }
           
            return objList;
        }
        */

        [HttpGet]
        public string token()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime now=DateTime.Now;
            long t = (long)(now - startTime).TotalMilliseconds;
            string secretKey = "DEVFORUSER-ANDRIOD-IOS-CRM-001-KEY";

            string param=string.Empty;
            param += "t=" + t;
            param += secretKey;
            string token = string.Empty;
            token = Common.MD5(param);

            string msg = string.Empty;
            msg = "t=" + t + " token=" + token;
            return msg;
        }
    }
}
