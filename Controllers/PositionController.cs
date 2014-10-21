using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fengmiapp.Models;

namespace fengmiapp.Controllers
{
    public class PositionController : Controller
    {
        //
        // GET: /Position/
        /// <summary>
        /// 设置用户定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPosition()
        {
            HttpRequestBase request = Request;

            string uId = request.Params.Get("uId");
            string longitude = request.Params.Get("longitude");
            string latitude = request.Params.Get("latitude");

            string placeName = request.Params.Get("placeName");
            string uploadTime = request.Params.Get("uploadTime");
            string modifyTime = "";

            UserPosition position = new UserPosition();

            string title = "";
            string status = "error";
            string msg = "";

            position.UId =int.Parse( uId);
            position.Longitude = double.Parse(longitude);
            position.Latitude =double.Parse( latitude);
            position.PlaceName = placeName;
            position.UploadTime =DateTime.Parse(uploadTime);
            position.ModifyTime = DateTime.Now;

            int result = position.Add();
            if (result > 0)
            {
                status = "succeed";
                msg = "添加成功";

            }
            else
            {
                status = "error";
                msg = "添加失败";
            }


            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            //int adminId = int.Parse(Session["adminId"].ToString());

            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };

            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 2.获取用户定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPosition()
        {
            //HttpRequestBase request = Request;

            string uId = Request.Params.Get("uId");
            string perPage = Request.Params.Get("perPage");
            string page = Request.Params.Get("page");

            int i_perPage=10;
            try
            {
                i_perPage = int.Parse(perPage);
            }
            catch { i_perPage = 10; }

            int i_page =1;
            try
            {
                i_page = int.Parse(page);
            }
            catch { i_page = 1; }


            int i_uId = int.Parse(uId);

            UserPosition position = new UserPosition();
            position.UId = i_uId;

            DataTable dt = new DataTable();
            string status = "error";
            string msg = "";
            string title = "";

            if (i_page == 1)
            {
                dt = position.GetPositionList(i_perPage);
            }
            else
            {
                dt = position.GetPositionList(i_page, i_perPage);
            }

            int count = dt.Rows.Count;
            int total = position.GetPositionListCount();

            List<object> listObj = new List<object>();
            if (count < 1)
            {
                status = "error";
                msg = "无数据";
            }
            else
            {
                status = "succeed";
                msg = "已获取";

                for (int i = 0; i < count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string Id = dr["Id"].ToString();

                    string longitude = dr["longitude"].ToString();
                    string latitude = dr["latitude"].ToString();
                    string placeName = dr["placeName"].ToString();
                    string uploadTime = dr["uploadTime"].ToString();
                    string modifyTime = dr["modifyTime"].ToString();

                    listObj.Add(new
                   {
                       longitude = longitude,
                       latitude = latitude,
                       placeName = placeName,
                       uploadTime = uploadTime,
                       modifyTime = modifyTime,

                   });
                }
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            //int adminId = int.Parse(Session["adminId"].ToString());

            Common.addLog(logType, title + msg);
            double d_pageTotal = 1.0 * total / i_perPage;
            int pageTotal = (int)Math.Ceiling(d_pageTotal);

            object obj = new
            {
                status = status,
                msg = msg,
                total=total,
                pageTotal=pageTotal,
                data = listObj,
            };

            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取所有好友的最新定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNewestPosition()
        {
            //HttpRequestBase request = Request;

            //string uId = request.Params.Get("uId");
            string uId = Request.Params.Get("uId");
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }

            UserFriend userFriend = new UserFriend();

            DataTable dt_uf = new DataTable();

            userFriend.UId = i_uId;
            dt_uf = userFriend.GetUserFriends();


            string status = "error";
            string msg = "";
            string title = "";

            List<object> listObj = new List<object>();


            int countUF = dt_uf.Rows.Count;
            if (countUF < 1)
            {
                status = "error";
                msg = "无数据";

            }
            else
            {
                status = "succeed";
                msg = "已获取";
                string userStatus = string.Empty;
                string fuId = string.Empty;
                for (int i_UF = 0; i_UF < countUF; i_UF++)
                {
                    fuId = dt_uf.Rows[i_UF]["fuId"].ToString();
                    userStatus = dt_uf.Rows[i_UF]["userStatus"].ToString();

                    int i_fuId = 0;
                    try
                    {
                        i_fuId = int.Parse(fuId);
                    }
                    catch { }

                    UserPosition position = new UserPosition();
                    position.UId = i_fuId;

                    int number = 1;
                    DataTable dt = new DataTable();
                    dt = position.GetPositionList(number);
                    int count = dt.Rows.Count;
                    object obj_t = new object();
                    if (count < 1)
                    {
                        obj_t = new
                        {
                            uId = uId,
                            fuId = fuId,
                            status=userStatus,
                            position = new { },
                        };

                    }
                    else
                    {
                        int i = 0;
                        DataRow dr = dt.Rows[i];
                        string Id = dr["Id"].ToString();

                        string longitude = dr["longitude"].ToString();
                        string latitude = dr["latitude"].ToString();
                        string placeName = dr["placeName"].ToString();
                        string uploadTime = dr["uploadTime"].ToString();
                        string modifyTime = dr["modifyTime"].ToString();

                        obj_t = new
                        {
                            uId = uId,
                            fuId = fuId,
                            status = userStatus,
                            position = new
                            {
                                longitude = longitude,
                                latitude = latitude,
                                placeName = placeName,
                                uploadTime = uploadTime,
                                modifyTime = modifyTime,
                            },

                        };
                    }
                    listObj.Add(obj_t);
                }
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            //int adminId = int.Parse(Session["adminId"].ToString());

            Common.addLog(logType, title + msg);


            object obj = new
            {
                status = status,
                msg = msg,
                data = listObj,
            };

            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

    }
}
