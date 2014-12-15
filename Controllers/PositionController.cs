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
    public class PositionController : AbstraCommonController
    {
        //
        // GET: /Position/

        public PositionController()
        {
        }

        /// <summary>
        /// 上传用户定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPosition()
        {
            this.init();

            string title = "";
            string status = "error";
            string msg = "";

            HttpRequestBase request = Request;
            string uId = request.Params.Get("uId");
            string longitude = request.Params.Get("longitude");
            string latitude = request.Params.Get("latitude");

            string placeName = request.Params.Get("placeName");
            string uploadTime = request.Params.Get("uploadTime");


            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }
            if (this.IsEffetive)
            {
                User user = new User(i_uId);

                if (user.isUserExist())
                {
                    DateTime dt_uploadTime = DateTime.Now;
                    try
                    {
                        dt_uploadTime = DateTime.Parse(uploadTime);
                    }
                    catch { }


                    UserPosition position = new UserPosition();
                    position.UId = i_uId;
                    position.GetNewestPositionOne();
                    int Id = position.Id;
                    int result = 0;
                    string temp_placeName = position.PlaceName;
                    int userStatus = user.Status;
                    //获取对好友隐身的好友Id
                    string offlineUserIds=string.Empty;
                    UserFriend userFriend = new UserFriend();
                    bool isHide = false;
                    int isHiding = 0;
                    if (userStatus == 2)
                    {//用户隐身
                        isHide = true;
                        isHiding = 1;
                    }
                    else
                    {
                        isHide = false;
                        isHiding = 0;
                    }
                    offlineUserIds = userFriend.GetUserFriendsIdList(isHide);

                    position.IsHiding = isHiding;
                    position.OfflineUserIds = offlineUserIds;

                    if (Id > 0 && temp_placeName == placeName)
                    {
                        position.UploadTime = dt_uploadTime;
                        position.ModifyTime = DateTime.Now;

                        result = position.Modify_Time_IsHiding();
                        if (result > 0)
                        {
                            status = "succeed";
                            msg = "上传修改成功";

                        }
                        else
                        {
                            status = "error";
                            msg = "上传修改失败";
                        }
                    }
                    else
                    {
                        double d_longitude = 0;
                        try
                        {
                            d_longitude = double.Parse(longitude);
                        }
                        catch { }
                        double d_latitude = 0;
                        try
                        {
                            d_latitude = double.Parse(latitude);
                        }
                        catch { }


                        position.UId = i_uId;
                        position.Longitude = d_longitude;
                        position.Latitude = d_latitude;
                        position.PlaceName = placeName;
                        position.UploadTime = dt_uploadTime;
                        position.ModifyTime = DateTime.Now;

                        result = position.Add();
                        if (result > 0)
                        {
                            status = "succeed";
                            msg = "上传成功";

                        }
                        else
                        {
                            status = "error";
                            msg = "上传失败";
                        }
                    }
                }
                else
                {
                    status = "error";
                    msg = "上传失败，用户不存在";
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：SetPosition； ";
            title += "用户Id：" + i_uId + "，上传用户定位信息：";
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
            this.init();
            string status = "error";
            string msg = "";
            string title = "";
            
            int total = 0;

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

            List<object> listObj = new List<object>();

            if (this.IsEffetive)
            {

                UserPosition position = new UserPosition();
                position.UId = i_uId;
                DataTable dt = new DataTable();

                if (i_page == 1)
                {
                    dt = position.GetPositionList(i_perPage);
                }
                else
                {
                    dt = position.GetPositionList(i_page, i_perPage);
                }

                int count = dt.Rows.Count;
                 total = position.GetPositionListCount();

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
                        string offlineUserIds = dr["offlineUserIds"].ToString();
                        string isHiding = dr["isHiding"].ToString();
                        int i_isHiding = 0;
                        try
                        {
                            i_isHiding = int.Parse(isHiding);
                        }
                        catch { }

                        listObj.Add(new
                       {
                           longitude = longitude,
                           latitude = latitude,
                           placeName = placeName,
                           uploadTime = uploadTime,
                           modifyTime = modifyTime,
                           offlineUserIds = offlineUserIds,
                           isHiding = i_isHiding,

                       });
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetPosition； ";
            title += "用户Id：" + i_uId + "，获取用户定位信息：";
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
        ///  获取用户最新定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserNewestPosition()
        {
            this.init();

            string status = "error";
            string msg = "";
            string title = "";

            string uIdStr = Request.Params.Get("uId");
            string[] uIdList = uIdStr.Split(',');


            List<object> listObj = new List<object>();

            if (this.IsEffetive)
            {
                int uIdCount = uIdList.Length;
                if (uIdCount < 1)
                {
                    status = "error";
                    msg = "参数有误";
                }
                else
                {
                    string userStatus = string.Empty;
                    string uId = string.Empty;

                    for (int i_U = 0; i_U < uIdCount; i_U++)
                    {
                        uId = uIdList[i_U];
                        int i_uId = 0;
                        try
                        {
                            i_uId = int.Parse(uId);
                        }
                        catch
                        {
                            i_uId = 0;
                        }

                        UserPosition position = new UserPosition();
                        position.UId = i_uId;

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
                            string offlineUserIds = dr["offlineUserIds"].ToString();
                            string isHiding = dr["isHiding"].ToString();
                            int i_isHiding = 0;
                            try
                            {
                                i_isHiding = int.Parse(isHiding);
                            }
                            catch { }

                            obj_t = new
                            {
                                uId = uId,
                                position = new
                                {
                                    longitude = longitude,
                                    latitude = latitude,
                                    placeName = placeName,
                                    uploadTime = uploadTime,
                                    modifyTime = modifyTime,
                                    offlineUserIds = offlineUserIds,
                                    isHiding = i_isHiding,
                                },

                            };
                        }

                        listObj.Add(obj_t);
                    }
                    if (listObj != null)
                    {
                        status = "succeed";
                        msg = "获取成功";

                    }
                    else
                    {
                        status = "error";
                        msg = "获取失败";
                    }

                }

            }
            else
            {
                msg = this.ValidMsg;
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetUserNewestPosition； ";
            title += "用户Id：" + uIdStr + "，获取用户最新定位信息：";
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

        /// <summary>
        ///  获取群用户最新定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserNewestPositionByUGid()
        {
            this.init();

            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");
            int i_uGId = 0;

            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch
            {
                i_uGId = 0;
            }

            List<object> listObj = new List<object>();
            if (this.IsEffetive)
            {

                DataTable ug_dt = new DataTable();
                UserGroupUser userGroupUser = new UserGroupUser();
                userGroupUser.UGId = i_uGId;
                ug_dt = userGroupUser.GetUserGroupWithUGid();

                int ug_count = ug_dt.Rows.Count;

                if (ug_count < 1)
                {
                    status = "error";
                    msg = "参数有误";

                }
                else
                {
                    for (int j = 0; j < ug_count; j++)
                    {
                        string ug_uId = ug_dt.Rows[j]["uId"].ToString();
                        string uId = ug_uId;

                        int i_uId = 0;
                        try
                        {
                            i_uId = int.Parse(uId);
                        }
                        catch
                        {
                            i_uId = 0;
                        }

                        UserPosition position = new UserPosition();
                        position.UId = i_uId;

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
                            string offlineUserIds = dr["offlineUserIds"].ToString();
                            string isHiding = dr["isHiding"].ToString();
                            int i_isHiding = 0;
                            try
                            {
                                i_isHiding = int.Parse(isHiding);
                            }
                            catch { }

                            obj_t = new
                            {
                                uId = uId,
                                position = new
                                {
                                    longitude = longitude,
                                    latitude = latitude,
                                    placeName = placeName,
                                    uploadTime = uploadTime,
                                    modifyTime = modifyTime,
                                    offlineUserIds = offlineUserIds,
                                    isHiding = i_isHiding,
                                },

                            };
                        }
                        listObj.Add(obj_t);
                    }

                    if (listObj != null)
                    {
                        status = "succeed";
                        msg = "获取成功";
                    }
                    else
                    {
                        status = "error";
                        msg = "获取失败";
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetUserNewestPositionByUGid； ";
            title += "群Id：" + i_uGId + "，获取群用户最新定位信息：";
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

        /// <summary>
        /// 获取所有好友的最新定位信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNewestPosition()
        {
            this.init();

            string status = "error";
            string msg = "";
            string title = "";

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
            List<object> listObj = new List<object>();

            if (this.IsEffetive)
            {
                UserFriend userFriend = new UserFriend();
                DataTable dt_uf = new DataTable();
                userFriend.UId = i_uId;
                dt_uf = userFriend.GetUserFriends();

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
                                status = userStatus,
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
                            string offlineUserIds = dr["offlineUserIds"].ToString();
                            string isHiding = dr["isHiding"].ToString();
                            int i_isHiding = 0;
                            try
                            {
                                i_isHiding = int.Parse(isHiding);
                            }
                            catch { }


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
                                    offlineUserIds = offlineUserIds,
                                    isHiding = i_isHiding,

                                },

                            };
                        }
                        listObj.Add(obj_t);
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetNewestPosition； ";
            title += "用户Id：" + i_uId + "，获取所有好友的最新定位信息：";
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
