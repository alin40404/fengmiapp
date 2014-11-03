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
        [HttpGet]
        public IEnumerable test()
        {
            /*
            return new string[] { "Item1", "Item2" }.Select(s => new
            {
                Name = s,
                Code = s,
                Items = new ArrayList
                            {
                                new { Name = "Item1" },
                                new { Name = "Item2" },
                                new { Name = "Item3" },
                                new { Name = "Item4" },
                            }
            });
             * */

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
    }
}
