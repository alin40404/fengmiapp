using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using fengmiapp.Models;

namespace fengmiapp.Controllers
{
    public class UserController : Controller
    {
        #region 用户验证、登录、注册操作
       
        /// <summary>
        /// 1.验证用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostValidUser()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string phone = Request.Params.Get("phone");
            string password = Request.Params.Get("password");

            int uId = 0;

            if (string.IsNullOrEmpty(phone))
            {
                status = "error";
                msg = "失败，帐号不能为空";
            }
            else if (string.IsNullOrEmpty(password))
            {
                status = "error";
                msg = "失败，密码不能为空";
            }
            else
            {
                User user = new User();
                user.Phone = phone;
                user.PassWord = Common.MD5(password);
                user.login();

                uId=user.Id;

                if (uId>0)
                {
                    status = "succeed";
                    msg = "验证成功";
                }
                else
                {
                    uId = 0;
                    status = "error";
                    msg = "帐号或密码错误";
                }
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：PostValidUser； ";
            title += "用户名："+phone+"，密码："+password+";  ";
            title += "用户Id：" + uId + "，验证用户登录：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, uId = uId };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult isUserExist()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string phone = Request.Params.Get("phone");
            //string password = Request.Params.Get("password");
            DataTable dt = new DataTable();
            User user = new User(phone);

            string uId = "0";

            if (user.Phone == phone)
            {
                uId = user.Id.ToString();
                status = "succeed";
                msg = "帐号存在";
            }
            else
            {
                uId = "0";
                status = "error";
                msg = "帐号不存在";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;

            title += "API：isUserExist； ";
            title += "用户名：" + phone + ";  ";
            title += "用户Id：" + uId + "，判断用户是否存在：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg, uId = uId };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 2．用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register()
        {
            string status = "error";
            string msg = "";
            string title = "";

            //帐号 手机号 密码
            string phone = Request.Params.Get("phone");
            string password = Request.Params.Get("password");

            int uId = 0;

            if (string.IsNullOrEmpty(phone))
            {
                status = "error";
                msg = "注册失败，帐号不能为空";

            }
            else if (string.IsNullOrEmpty(password))
            {
                status = "error";
                msg = "注册失败，密码不能为空";
            }
            else
            {
                DataTable dt = new DataTable();
                User user = new User(phone);
                uId = user.Id;

                if (uId < 1)
                {//用户可用
                    user.Phone = phone;
                    password = Common.MD5(password);
                    user.PassWord = password;

                    int result = user.Add();

                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "注册成功";
                    }
                    else
                    {
                        status = "error";
                        msg = "注册失败";
                    }
                }
                else
                {
                    status = "error";
                    msg = "注册失败，帐号已存在";
                }
            }

            int logType = 1;
            string ip = Request.UserHostAddress;

            title += "API：Register； ";
            title += "用户名：" + phone + "，密码：" + password + ";  ";
            title += "用户Id：" + uId + "，用户注册：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };

            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        #endregion

        #region 用户基本信息操作

        /// <summary>
        /// 3.修改用户个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUserInfo()
        {
            string title = "";
            string status = "error";
            string msg = "";

            string uId = Request.Params.Get("uId");

            string realName = Request.Params.Get("realName");
            string nickName = Request.Params.Get("nickName");
            string identityCard = Request.Params.Get("identityCard");
            string birthDay = Request.Params.Get("birthDay");
            string userFace = Request.Params.Get("userFace");
            string email = Request.Params.Get("email");
            string address = Request.Params.Get("address");
            string interests = Request.Params.Get("interests");

            
            int Id = 0;
            try
            {
                Id = int.Parse(uId);
            }
            catch
            {
                Id = 0;
            }
            User adminUser = new User();

            //adminUser.Phone = phone;
            adminUser.RealName = realName;
            adminUser.NickName = nickName;
            adminUser.IdentityCard = identityCard;
            adminUser.BirthDay = DateTime.Parse(birthDay);
            adminUser.Address = address;
            adminUser.Email = email;
            adminUser.UserFace = userFace;
            adminUser.Interests = interests;

            if (Id > 0)
            {//update
                adminUser.Id = Id;
                int result = adminUser.ModifyInfo();
                if (result > 0)
                {
                    status = "succeed";
                    msg = "修改成功";

                }
                else
                {
                    status = "error";
                    msg = "修改失败";
                }
            }
            else
            {
                status = "error";
                msg = "修改失败,用户帐号不存在";

            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUserInfo； ";
            title += "用户Id：" + uId + "，修改用户个人信息：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfo()
        {
            string title = "";
            string status = "error";
            string msg = "";

            string uId = Request.Params.Get("uId");
            int Id = 0;
            try
            {
                Id = int.Parse(uId);
            }
            catch
            {
                Id = 0;
            }
            User adminUser = new User(Id);

            string phone = adminUser.Phone;

            object userOjb = new object();

            if (!string.IsNullOrEmpty(phone))
            {//get
                string realName = adminUser.RealName;
                string nickName = adminUser.NickName;
                string identityCard = adminUser.IdentityCard;
                string birthDay = adminUser.BirthDay.ToString("yyyy-MM-dd");//HH:mm:ss
                string userFace = adminUser.UserFace;
                string email = adminUser.Email;
                string address = adminUser.Address;
                int userStatus = adminUser.Status;
                string interests = adminUser.Interests;
                int isPermitAddFriend = adminUser.IsPermitAddFriend;



                userOjb = new
                {
                    phone = phone,
                    realName = realName,
                    nickName = nickName,
                    identityCard = identityCard,
                    birthDay = birthDay,
                    userFace = userFace,
                    email = email,
                    address = address,
                    userStatus = userStatus,
                    interests = interests,
                    isPermitAddFriend = isPermitAddFriend,

                };


                status = "succeed";
                msg = "获取成功";

            }
            else
            {
                status = "error";
                msg = "  获取失败,用户帐号不存在";
                userOjb = new object();
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetUserInfo； ";
            title += "用户Id：" + Id + "，获取用户个人信息：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, user = userOjb };

            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 4.修改用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUpdatePwd()
        {

            string oldPassword = Request.Params.Get("oldPassword");
            string password = Request.Params.Get("password");
            string uId = Request.Params.Get("uId");

            int Id = 0;
            try
            {
                Id = int.Parse(uId);
            }
            catch
            {
                Id = 0;
            }

            string title = "";
            string msg = "";
            string status = "error";

            if (Id > 0)
            {//update
                User adminUser = new User(Id);
                if (!string.IsNullOrEmpty(oldPassword))
                {
                    oldPassword = Common.MD5(oldPassword);
                    if (oldPassword != adminUser.PassWord)
                    {
                        msg = "修改密码失败，原密码不正确";
                        status = "error";
                    }
                    else
                    {
                        adminUser.Id = Id;
                        password = Common.MD5(password);
                        adminUser.PassWord = password;

                        int result = adminUser.ModifyPWD();
                        if (result > 0)
                        {
                            msg = "修改成功";
                            status = "succeed";
                        }
                        else
                        {
                            msg = "修改失败";
                            status = "error";

                        }

                    }
                }
                else
                {
                    msg = "修改密码失败，原密码不能为空";
                    status = "error";
                }

            }
            else
            {
                msg = "修改失败,用户帐号不存在";
                status = "error";
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUpdatePwd； ";
            title += "用户Id：" + Id + "，修改用户密码：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserStatus()
        {
            string uId = Request.Params.Get("uId");

            int Id = 0;
            try
            {
                Id = int.Parse(uId);
            }
            catch
            {
                Id = 0;
            }
            User adminUser = new User(Id);

            string phone = adminUser.Phone;

            string title = "";
            string status = "error";
            string msg = "";
            object userOjb = new object();

            if (!string.IsNullOrEmpty(phone))
            {//get
                //1 在线 ，2 隐身
                int userStatus = adminUser.Status;

                //string realName = adminUser.RealName;
                //string nickName = adminUser.NickName;
                //string identityCard = adminUser.IdentityCard;
                //string birthDay = adminUser.BirthDay.ToString("yyyy-MM-dd HH:mm:ss");
                //string userFace = adminUser.UserFace;
                string email = adminUser.Email;
                //string address = adminUser.Address;

                userOjb = new
                {
                    uId = uId,
                    phone = phone,
                    email = email,
                    status = userStatus,
                };

                status = "succeed";
                msg = "获取成功";

            }
            else
            {
                status = "error";
                msg = "  获取失败,用户帐号不存在";
                userOjb = new object();
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：GetUserStatus； ";
            title += "用户Id：" + Id + "，获取用户状态：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg, user = userOjb };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 5．修改用户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUserStatus()
        {
            string uId = Request.Params.Get("uId");

            string userStatus = Request.Params.Get("status");

            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }
            User adminUser = new User(i_uId);

            int Id = adminUser.Id;

            string title = "";
            string status = "error";
            string msg = "";

            //1 在线 ，2 隐身
            int i_status = 1;
            try
            {
                try
                {
                    i_status = int.Parse(userStatus);
                }
                catch { }
                adminUser.Status = i_status;


                if (Id > 0)
                {//update
                   
                    int result = adminUser.ModifyStatus();
                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "修改成功";

                    }
                    else
                    {
                        status = "error";
                        msg = "修改失败";
                    }
                }
                else
                {
                    status = "error";
                    msg = "修改失败,用户帐号不存在";

                }
            }
            catch
            {
                status = "error";
                msg = "修改失败，修改状态有误";
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUserStatus； ";
            title += "用户Id：" + i_uId + "，修改用户状态：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 5．修改用户设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUserPermit()
        {
            string uId = Request.Params.Get("uId");
            string isPermitAddFriend = Request.Params.Get("isPermitAddFriend");

            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }


            User adminUser = new User(i_uId);
            int Id = adminUser.Id;
            string title = "";
            string status = "error";
            string msg = "";

            //1 允许 ，0 不允许
            int i_isPermitAddFriend = 1;
            try
            {
                i_isPermitAddFriend = int.Parse(isPermitAddFriend);
            }
            catch { }
            adminUser.IsPermitAddFriend = i_isPermitAddFriend;

            if (Id > 0)
            {//update
                int result = adminUser.ModifyPermit();
                if (result > 0)
                {
                    status = "succeed";
                    msg = "修改成功";
                }
                else
                {
                    status = "error";
                    msg = "修改失败";
                }
            }
            else
            {
                status = "error";
                msg = "修改失败,用户帐号不存在";

            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUserPermit； ";
            title += "用户Id：" + i_uId + "，修改用户设置：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 6．修改用户对某好友状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUserToFriendUserStatus()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            string fuId = Request.Params.Get("fuId");
            string modifyTime = Request.Params.Get("modifyTime");
            string userFriendStatus = Request.Params.Get("status");

            DataTable dt = new DataTable();

            //int i_uId=int.Parse(uId);
            //int i_fuId=int.Parse(fuId);
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }

            int i_fuId = 0;
            try
            {
                i_fuId = int.Parse(fuId);
            }
            catch
            {
                i_fuId = 0;
            }

            UserFriend userfriend = new UserFriend(i_uId, i_fuId);
            int UFId = userfriend.Id;

            if (UFId > 0)
            {//用户可用
                DateTime dt_modifyTime = new DateTime();
                try
                {
                    dt_modifyTime = DateTime.Parse(modifyTime);
                }
                catch
                {
                }
                userfriend.ModifyTime = dt_modifyTime;

                int i_userFriendStatus = 0;
                try
                {
                    i_userFriendStatus = int.Parse(userFriendStatus);
                }
                catch { }

                userfriend.Status = i_userFriendStatus;
                int result = userfriend.ModifyStatus();

                if (result > 0)
                {
                    UserFriendStatus userfrStatus = new UserFriendStatus();
                    userfrStatus.UId = userfriend.UId;
                    userfrStatus.FuId = userfriend.FuId;
                    userfrStatus.UploadTime = dt_modifyTime;
                    userfrStatus.Status = i_userFriendStatus;
                    userfrStatus.Add();

                    status = "succeed";
                    msg = "修改成功";
                }
                else
                {
                    status = "error";
                    msg = "修改失败";
                }
            }
            else
            {
                status = "error";
                msg = "修改失败，未加好友";
            }
            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUserToFriendUserStatus； ";
            title += "用户Id：" + i_uId + "，好友Id：" + i_fuId + "，修改用户对某好友状态：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }
       
        /// <summary>
        /// 7．修改用户对群状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DealUserToGroupStatus()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            string uGId = Request.Params.Get("uGId");
            string modifyTime = Request.Params.Get("modifyTime");
            string userStatus = Request.Params.Get("status");

            DataTable dt = new DataTable();

            //int i_uId = int.Parse(uId);
            //int i_uGId = int.Parse(uGId);

            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch
            {
                i_uGId = 0;
            }

            UserGroupUser userGroupUser = new UserGroupUser(i_uId, i_uGId);
            int Id = userGroupUser.Id;

            if (Id > 0)
            {//用户可用
                DateTime dt_modifyTime = new DateTime();
                try
                {
                    dt_modifyTime = DateTime.Parse(modifyTime);
                }
                catch
                {
                }
                userGroupUser.ModifyTime = dt_modifyTime;

                int i_userStatus = 0;
                try
                {
                    i_userStatus = int.Parse(userStatus);
                }
                catch { }

                userGroupUser.Status = i_userStatus;
                int result = userGroupUser.ModifyStatus();
                if (result > 0)
                {
                    UserGroupUserStatus userGrStatus = new UserGroupUserStatus();
                    userGrStatus.UId = userGroupUser.UId;
                    userGrStatus.UGId = userGroupUser.UGId;
                    userGrStatus.UploadTime = dt_modifyTime;
                    userGrStatus.Status = i_userStatus;
                    userGrStatus.Add();

                    status = "succeed";
                    msg = "修改成功";
                }
                else
                {
                    status = "error";
                    msg = "修改失败";
                }
            }
            else
            {
                status = "error";
                msg = "修改失败，用户未加入此群组";
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：DealUserToGroupStatus； ";
            title += "用户Id：" + i_uId + "，群Id："+i_uGId+"，修改用户对群状态：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        #endregion

        #region 用户动作，例如摇一摇
        
        /// <summary>
        /// 摇一摇
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Shake()
        {
            string title = "";
            string status = "error";
            string msg = "";

            HttpRequestBase request = Request;
            string uId = request.Params.Get("uId");
            string uploadTime = request.Params.Get("uploadTime");

            int action = 1;
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }
            DateTime dt_uploadTime = DateTime.Now;
            try
            {
                dt_uploadTime = DateTime.Parse(uploadTime);
            }
            catch { }

            int result = 0;

            UserAction userAction = new UserAction(i_uId,action);
            int Id = userAction.Id;
            userAction.UploadTime = dt_uploadTime;

            if (Id > 0)
            {//update

                userAction.ModifyTime = DateTime.Now;
                result = userAction.ModifyInfo();
            }
            else
            {
                userAction.UId = i_uId;
                userAction.Action = action;

                result = userAction.Add();
                if (result > 0)
                {
                    userAction = new UserAction(i_uId, action);
                }
            }

            List<object> userObjList = new List<object>();


            if (result > 0)
            {
                status = "succeed";
                msg = "获取成功";

                DataTable dt = new DataTable();

                //重置用户Id
                userAction.UId = 0;
                userAction.Action = action;
                userAction.Id = 0;//显示所有 包括自己的用户
                
                int number = 20;
                double hours = -1;//取一小时之前到现在的数据
                //dt = userAction.GetActionUserList(number);
                dt = userAction.GetActionUserList(number, hours);
                int count = dt.Rows.Count;

                for (int i = 0; i < count; i++)
                {

                    string t_uId = dt.Rows[i]["uId"].ToString();
                    string t_action = dt.Rows[i]["action"].ToString();
                    string t_uploadTime = dt.Rows[i]["uploadTime"].ToString();
                    string t_modifyTime = dt.Rows[i]["modifyTime"].ToString();
                    try
                    {
                        t_uploadTime = DateTime.Parse(t_uploadTime).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                    try
                    {
                        t_modifyTime = DateTime.Parse(t_modifyTime).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }


                    string phone = dt.Rows[i]["phone"].ToString();
                    string realName = dt.Rows[i]["realName"].ToString();
                    string nickName = dt.Rows[i]["nickName"].ToString();
                    string identityCard = dt.Rows[i]["identityCard"].ToString();
                    string t_birthDay = dt.Rows[i]["birthDay"].ToString();

                    string birthDay = "";
                    try
                    {
                        birthDay = DateTime.Parse(t_birthDay).ToString("yyyy-MM-dd");
                    }
                    catch { }

                    string userFace = dt.Rows[i]["userFace"].ToString();
                    //string userFace = Encoding.UTF8.GetString((byte[])dt.Rows[i]["userFace"]);

                    string email = dt.Rows[i]["email"].ToString();
                    string address = dt.Rows[i]["address"].ToString();

                    object userObj = new object();
                    userObj = new
                    {
                        uId = t_uId,
                        action = t_action,
                        uploadTime = t_uploadTime,
                        modifyTime = t_modifyTime,

                        phone = phone,
                        realName = realName,
                        nickName = nickName,
                        identityCard = identityCard,
                        birthDay = birthDay,
                        userFace = userFace,
                        email = email,
                        address = address
                    };
                    userObjList.Add(userObj);
                }
            }
            else
            {
                status = "error";
                msg = "获取失败";
            }

            int logType = 3;
            string ip = Request.UserHostAddress;
            string emergeURL = Request.Url.ToString();
            title += "API：Shake； ";
            title += "用户Id：" + i_uId + "，上传时间：" + uploadTime + "，摇一摇：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, users = userObjList };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        #endregion

        #region 好友操作

        /// <summary>
        /// 1.添加好友
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFriend()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            string fuId = Request.Params.Get("fuId");
            string addType = Request.Params.Get("addType");
            string uFGroupId = Request.Params.Get("uFGroupId");
            
            #region 字段验证
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }

            int i_fuId = 0;
            try
            {
                i_fuId = int.Parse(fuId);
            }
            catch
            {
                i_fuId = 0;
            }
            int i_addType = 2;
            try
            {
                i_addType = int.Parse(addType);
            }
            catch
            {
                i_addType = 2;
            }

            int i_uFGroupId = 0;
            try
            {
                i_uFGroupId = int.Parse(uFGroupId);
            }
            catch
            {
                i_uFGroupId = 0;
            }
            #endregion

            UserFriend userFriend = new UserFriend(i_uId, i_fuId);

            UserFriend userFriend_added = new UserFriend();

            int Id = userFriend.Id;
            int Id_added = 0;


            int userStatus =1 ;
            int result = 0;

            if (Id > 0)
            {
                userStatus = userFriend.Status; //添加好友 修改状态

                if (userStatus > 0)
                {
                    status = "error";
                    msg = "添加失败，已经是好友";
                }
                else
                {
                    userStatus = 1;
                    userFriend.Status = userStatus;

                    //通知对方


                    result = userFriend.ModifyStatus();

                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "已经恢复为好友";

                       userFriend_added = new UserFriend(i_fuId, i_uId);
                       Id_added = userFriend_added.Id;

                        if (Id_added > 0)
                        {
                            int tempStatus = userFriend_added.Status;
                            if (tempStatus < 1)
                            {
                                userFriend_added.Status = userStatus;
                                userFriend_added.ModifyStatus();
                            }
                        }
                        else
                        {
                            userFriend_added.UId =i_fuId ;
                            userFriend_added.FuId = i_uId;
                            userFriend_added.AddType = i_addType;
                            userFriend_added.UFGroupId = i_uFGroupId;
                            userFriend_added.Status = userStatus;
                            userFriend_added.Add();
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = "恢复为好友失败";
                    }

                }

            }
            else
            {
                if (i_uId <= 0 || i_fuId <= 0)
                {
                    status = "error";
                    msg = "添加失败，用户Id有误";

                }
                else
                {
                    userFriend.UId = i_uId;
                    userFriend.FuId = i_fuId;
                    userFriend.AddType = i_addType;
                    userFriend.UFGroupId = i_uFGroupId;

                    userStatus = 1;//默认在线

                    userFriend.Status = userStatus;
                    //通知对方

                    result = userFriend.Add();

                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "添加成功";

                        userFriend_added = new UserFriend(i_fuId, i_uId);
                        Id_added = userFriend_added.Id;

                        if (Id_added > 0)
                        {
                            int tempStatus=userFriend_added.Status;
                            if (tempStatus < 1)
                            {
                                userFriend_added.Status = userStatus;
                                userFriend_added.ModifyStatus();
                            }
                        }
                        else
                        {
                            userFriend_added.UId = i_fuId;
                            userFriend_added.FuId = i_uId;
                            userFriend_added.AddType = i_addType;
                            userFriend_added.UFGroupId = i_uFGroupId;
                            userFriend_added.Status = userStatus;

                            userFriend_added.Add();

                        }

                    }
                    else
                    {
                        status = "error";
                        msg = "添加失败";
                    }
                }
            }

            int logType = 1;
            string ip = Request.UserHostAddress;

            title += "API：AddFriend； ";
            title += "用户Id：" + i_uId + "，好友Id：" + i_fuId + "，添加好友：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 2.删除好友
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteFriend()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            string fuId = Request.Params.Get("fuId");//多个好友用 "，" 隔开

            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch
            {
                i_uId = 0;
            }
           
            //int i_fuId = int.Parse(fuId);
            //UserFriend userFriend = new UserFriend(i_uId, i_fuId);
            //UserFriend userFriend_added = new UserFriend(i_fuId, i_uId);

            UserFriend userFriend = new UserFriend();

            int userStatus = 0; //删除好友
            int result = 0;


            if (i_uId <= 0)
            {
                status = "error";
                msg = "删除失败，uId错误";
            }
            else
            {
                userFriend.UId = i_uId;
                userFriend.Status = userStatus; 

                result = userFriend.ModifyStatus(fuId);

                if (result > 0)
                {
                    status = "succeed";
                    msg = "删除成功";


                }
                else
                {
                    status = "error";
                    msg = "删除失败";
                }

            }

            int logType = 1;
            string ip = Request.UserHostAddress;

            title += "API：DeleteFriend； ";
            title += "用户Id：" + i_uId + "，好友Id：" + fuId + "，删除好友：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取好友
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFriend()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            //string fuId = Request.Params.Get("fuId");
            //string addType = Request.Params.Get("addType");
            //string uFGroupId = Request.Params.Get("uFGroupId");
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

            //userFriend.UId = int.Parse(uId);
            //userFriend.FuId = int.Parse(fuId);
            //userFriend.AddType = int.Parse(addType);
            //userFriend.UFGroupId = int.Parse(uFGroupId);

            //int userStatus = 1;//默认在线

            //userFriend.Status = userStatus;


            DataTable dt = new DataTable();

            userFriend.UId = i_uId;
            dt = userFriend.GetUserFriends();

            int count = dt.Rows.Count;

            List<object> userFriendObjList = new List<object>();

            if (dt != null)
            {
                status = "succeed";
                msg = "获取成功";

                for (int i = 0; i < count; i++)
                {

                    //string uId = Request.Params.Get("uId");
                    string fuId = dt.Rows[i]["fuId"].ToString();
                    string addType = dt.Rows[i]["addType"].ToString();
                    string uFGroupId = dt.Rows[i]["uFGroupId"].ToString();
                    string modifyTime = dt.Rows[i]["modifyTime"].ToString();
                    //status
                    string userStatus = dt.Rows[i]["status"].ToString();


                    string phone = dt.Rows[i]["phone"].ToString();
                    string realName = dt.Rows[i]["realName"].ToString();
                    string nickName = dt.Rows[i]["nickName"].ToString();
                    string identityCard = dt.Rows[i]["identityCard"].ToString();
                    string t_birthDay = dt.Rows[i]["birthDay"].ToString();

                    string birthDay = "";
                    try
                    {
                        birthDay = DateTime.Parse(t_birthDay).ToString("yyyy-MM-dd");
                    }
                    catch { }

                    string userFace = dt.Rows[i]["userFace"].ToString();
                    //string userFace = Encoding.UTF8.GetString((byte[])dt.Rows[i]["userFace"]);

                    string email = dt.Rows[i]["email"].ToString();
                    string address = dt.Rows[i]["address"].ToString();


                    object userFriendObj = new object();

                    userFriendObj = new
                    {
                        uId = uId,
                        fuId = fuId,
                        addType = addType,
                        uFGroupId = uFGroupId,
                        modifyTime = modifyTime,
                        status = userStatus,

                        phone = phone,
                        realName = realName,
                        nickName = nickName,
                        identityCard = identityCard,
                        birthDay = birthDay,
                        userFace = userFace,
                        email = email,
                        address = address
                    };

                    userFriendObjList.Add(userFriendObj);
                }

            }
            else
            {
                status = "error";
                msg = "获取失败";
                userFriendObjList = new List<object>();
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：GetFriend； ";
            title += "用户Id：" + i_uId + "，获取好友：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg, userFriend = userFriendObjList };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }
       
        #endregion

        #region 好友分组操作

        /// <summary>
        /// 4.创建好友分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUserFriendGroup()
        {
            string status = "error";
            string msg = "";
            string title = "";

            int uFGroupId = 0;

            string uId = Request.Params.Get("uId");
            string name = Request.Params.Get("name");

            //string userStatusStr = Request.Params.Get("status");

            string userStatusStr = "1";
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch { }

            int userStatus = 1;
            try
            {
                userStatus = int.Parse(userStatusStr);
            }
            catch { }


            if (string.IsNullOrEmpty(uId))
            {
                status = "error";
                msg = "创建失败，用户Id不能为空";
            }
            else if (string.IsNullOrEmpty(name))
            {
                status = "error";
                msg = "创建失败，组名不能为空";
            }
            else
            {
                UserFriendGroup userFriendGroup = new UserFriendGroup(i_uId, name);
                int Id = userFriendGroup.Id;

                int result = 0;
                if (Id > 0)
                {
                    int ufg_status = 1;
                    ufg_status = userFriendGroup.Status;
                    if (ufg_status < 1)
                    {
                        ufg_status = 1;
                        userFriendGroup.Status = ufg_status;
                        result = userFriendGroup.ModifyStatus();
                        if (result > 0)
                        {
                            status = "succeed";
                            msg = "创建成功";
                        }
                        else
                        {
                            status = "error";
                            msg = "创建失败";
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = "创建失败，组名已经存在";
                    }
                    uFGroupId = Id;
                }
                else
                {
                    userFriendGroup.GName = name;
                    userFriendGroup.UId = i_uId;
                    userFriendGroup.Status = userStatus;
                    result = userFriendGroup.AddBackId();

                    uFGroupId = result;

                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "创建成功";
                    }
                    else
                    {
                        status = "error";
                        msg = "创建失败";
                    }
                }
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：CreateUserFriendGroup； ";
            title += "用户Id：" + i_uId + "，分组名称：" + name + "，创建好友分组：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg,uFGroupId = uFGroupId };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 删除好友分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserFriendGroup()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uFGroupId = Request.Params.Get("uFGroupId");
            int i_uFGroupId = 0;
            try
            {
                i_uFGroupId = int.Parse(uFGroupId);
            }
            catch
            {
            }

            UserFriendGroup userFriendGroup = new UserFriendGroup(i_uFGroupId);
            int Id = userFriendGroup.Id;

            if (Id > 0)
            {
                int userStatus = 0;
                userFriendGroup.Status = userStatus;
                int result = userFriendGroup.ModifyStatus();

                if (result > 0)
                {
                    status = "succeed";
                    msg = "删除成功";
                }
                else
                {
                    status = "error";
                    msg = "删除失败";
                }
            }
            else
            {
                status = "error";
                msg = "删除失败，好友分组不存在";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：DeleteUserFriendGroup； ";
            title += "分组Id：" + i_uFGroupId + "，删除好友分组：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 修改好友分组名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyUserFriendGroupName()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uFGroupId = Request.Params.Get("uFGroupId");
            string name = Request.Params.Get("name");

            int i_uFGroupId = 0;

            try
            {
                i_uFGroupId = int.Parse(uFGroupId);
            }
            catch
            {
            }

            UserFriendGroup userFriendGroup = new UserFriendGroup(i_uFGroupId);
            int Id = userFriendGroup.Id;

            if (Id > 0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    status = "error";
                    msg = "修改组名不能为空";
                }
                else
                {
                    int i_uId = userFriendGroup.UId;
                    //判断组名是否存在
                    UserFriendGroup temp_userFriendGroup = new UserFriendGroup(i_uId, name);
                    int temp_Id = temp_userFriendGroup.Id;

                    if (temp_Id > 0)
                    {//组名已经存在
                        status = "error";
                        msg = "修改失败，好友分组已存在";
                    }
                    else
                    {
                        userFriendGroup.GName = name;
                        int result = userFriendGroup.ModifyName();
                        if (result > 0)
                        {
                            status = "succeed";
                            msg = "修改成功";
                        }
                        else
                        {
                            status = "error";
                            msg = "修改失败";
                        }
                    }
                }
            }
            else
            {
                status = "error";
                msg = "修改失败，好友分组不存在";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：ModifyUserFriendGroupName； ";
            title += "分组Id：" + i_uFGroupId + "，修改名称：" + name + "，修改好友分组名称：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 3.修改好友分组状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyUserFriendGroupStatus()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uFGroupId = Request.Params.Get("uFGroupId");
            string userStatusStr = Request.Params.Get("status");

            int i_uFGroupId = 0;
            int userStatus = 0;
            try
            {
                i_uFGroupId = int.Parse(uFGroupId);
            }
            catch
            {
            }
            try
            {
                userStatus = int.Parse(userStatusStr);
            }
            catch
            {
            }

            UserFriendGroup userFriendGroup = new UserFriendGroup(i_uFGroupId);
            int Id = userFriendGroup.Id;

            if (Id > 0)
            {

                userFriendGroup.Status = userStatus;
                int result = userFriendGroup.ModifyStatus();

                if (result > 0)
                {
                    status = "succeed";
                    msg = "修改成功";
                }
                else
                {
                    status = "error";
                    msg = "修改失败";
                }
            }
            else
            {
                status = "error";
                msg = "修改失败，好友分组不存在";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：ModifyUserFriendGroupStatus； ";
            title += "分组Id：" + i_uFGroupId + "，分组状态：" + userStatus + "，修改好友分组状态：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 添加好友到好友分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserFriToUserFriendGroup()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uId = Request.Params.Get("uId");
            string fuId = Request.Params.Get("fuId");
            string uFGroupId = Request.Params.Get("uFGroupId");
            //string userStatusStr = Request.Params.Get("status");

            int i_uFGroupId = 0;
            try
            {
                i_uFGroupId = int.Parse(uFGroupId);
            }
            catch
            {
            }
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch { }
            int i_fuId = 0;
            try
            {
                i_fuId = int.Parse(fuId);
            }
            catch
            {
                i_fuId = 0;
            }

            UserFriend userFriend = new UserFriend(i_uId, i_fuId);
            int Id = userFriend.Id;

            if (Id > 0)
            {
                userFriend.UFGroupId = i_uFGroupId;
                int result = userFriend.Modify_uFGroupId();

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
            }
            else
            {
                status = "error";
                msg = "修改失败，用户好友不存在";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：AddUserFriToUserFriendGroup； ";
            title += "用户Id：" + i_uId + "，好友Id：" + i_fuId + "，分组Id：" + i_uFGroupId + "，修改好友分组状态：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取好友分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserFriendGroup()
        {
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
            }

            List<object> userFriendGroupObjList = new List<object>();

            if (i_uId < 1)
            {
                status = "error";
                msg = "获取失败，好友分组不存在";
                userFriendGroupObjList = new List<object>();
            }
            else
            {
                DataTable dt = new DataTable();
                UserFriendGroup userFriendGroup = new UserFriendGroup();
                userFriendGroup.UId = i_uId;

                dt = userFriendGroup.GetUserFriendGroup();
                int count = dt.Rows.Count;

                if (dt != null)
                {
                    status = "succeed";
                    msg = "获取成功";

                    for (int i = 0; i < count; i++)
                    {
                        string uFGroupId = dt.Rows[i]["Id"].ToString();
                        int i_uFGroupId = 0;
                        try
                        {
                            i_uFGroupId = int.Parse(uFGroupId);
                        }
                        catch { }

                        string gName = dt.Rows[i]["gName"].ToString();
                        string modifyTime = dt.Rows[i]["modifyTime"].ToString();
                        //status
                        string userFriendGroupStatus = dt.Rows[i]["status"].ToString();
                        int i_userFriendGroupStatus = 0;
                        try
                        {
                            i_userFriendGroupStatus = int.Parse(userFriendGroupStatus);
                        }
                        catch { }



                        object userFriendGroupObj = new object();

                        userFriendGroupObj = new
                        {
                            uId = i_uId,
                            uFGroupId = i_uFGroupId,
                            gName = gName,
                            modifyTime = modifyTime,
                            status = i_userFriendGroupStatus,
                        };

                        userFriendGroupObjList.Add(userFriendGroupObj);
                    }

                }
                else
                {
                    status = "error";
                    msg = "获取失败，获取好友分组";

                    userFriendGroupObjList = new List<object>();
                }

            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：GetUserFriendGroup； ";
            title += "用户Id：" + i_uId + "，获取好友分组：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, userFriendGroup = userFriendGroupObjList };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        #endregion

        #region 群组操作
       
        /// <summary>
        /// 获取用户 加入的所有群名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserGroup()
        {
            string status = "error";
            string msg = "";
            string title="";

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

            UserGroupUser userGroupUser = new UserGroupUser();
            userGroupUser.UId = i_uId;


            DataTable dt = new DataTable();

            dt = userGroupUser.GetUserGroupWithUid();

            int count = dt.Rows.Count;

            List<object> userGroupObjList = new List<object>();

            if (dt != null)
            {
                status = "succeed";
                msg = "获取成功";

                for (int i = 0; i < count; i++)
                {

                    //string uId = Request.Params.Get("uId");
                    string uGId = dt.Rows[i]["uGId"].ToString();
                    string uRole = dt.Rows[i]["uRole"].ToString();
                    string modifyTime = dt.Rows[i]["modifyTime"].ToString();
                    //status
                    string userStatus = dt.Rows[i]["status"].ToString();


                    string gType = dt.Rows[i]["gType"].ToString();
                    string name = dt.Rows[i]["name"].ToString();
                    string createUId = dt.Rows[i]["createUId"].ToString();
      

                    object userFriendObj = new object();

                    userFriendObj = new
                    {
                        uId = uId,
                        uGId = uGId,
                        uRole = uRole,
                        modifyTime = modifyTime,
                        status = userStatus,

                        name = name,
                        createUId = createUId,
                    };

                    userGroupObjList.Add(userFriendObj);
                }

            }
            else
            {
                status = "error";
                msg = "获取失败";
                userGroupObjList = new List<object>();
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：GetUserGroup； ";
            title += "用户Id：" + i_uId + "，获取用户加入的所有群名称：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, userGroup = userGroupObjList };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 获取 群用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserGroupUser()
        {
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

            UserGroupUser userGroupUser = new UserGroupUser();
            userGroupUser.UId = i_uId;


            DataTable dt = new DataTable();

            List<object> userGroupObjList = new List<object>();

            dt = userGroupUser.GetUserGroupWithUid();
            int count = dt.Rows.Count;
            if (dt != null)
            {
                status = "succeed";
                msg = "获取成功";

                for (int i = 0; i < count; i++)
                {
                    string uGId = dt.Rows[i]["uGId"].ToString();
                    string modifyTime = dt.Rows[i]["modifyTime"].ToString();

                    string gType = dt.Rows[i]["gType"].ToString();
                    string name = dt.Rows[i]["name"].ToString();
                    string createUId = dt.Rows[i]["createUId"].ToString();

                    int i_uGId = 0;
                    try
                    {
                        i_uGId = int.Parse(uGId);
                    }
                    catch
                    {
                        i_uGId = 0;
                    }

                    DataTable ug_dt = new DataTable();

                    userGroupUser.UGId = i_uGId;
                    ug_dt = userGroupUser.GetUserGroupWithUGid();

                    int ug_count = ug_dt.Rows.Count;

                    List<object> userGroupUserObjList = new List<object>();

                    for (int j = 0; j < ug_count; j++)
                    {
                        //status
                        string ugStatus = ug_dt.Rows[j]["status"].ToString();
                        string uRole = ug_dt.Rows[j]["uRole"].ToString();

                        string ug_uId = ug_dt.Rows[j]["uId"].ToString();
                        string phone = ug_dt.Rows[j]["phone"].ToString();
                        string realName = ug_dt.Rows[j]["realName"].ToString();
                        string nickName = ug_dt.Rows[j]["nickName"].ToString();
                        string identityCard = ug_dt.Rows[j]["identityCard"].ToString();
                        string birthDay = ug_dt.Rows[j]["birthDay"].ToString();
                        try
                        {
                            birthDay = DateTime.Parse(birthDay).ToString("yyyy-MM-dd");
                        }
                        catch { birthDay = ""; }

                        //string birthDay = DateTime.Parse().ToString("yyyy-MM-dd");

                        string userFace = ug_dt.Rows[j]["userFace"].ToString();
                        string email = ug_dt.Rows[j]["email"].ToString();
                        string address = ug_dt.Rows[j]["address"].ToString();


                        object userObj = new object();

                        userObj = new
                        {
                            uRole = uRole,
                            status = ugStatus,
                            uId = ug_uId,

                            phone = phone,
                            realName = realName,
                            nickName = nickName,
                            identityCard = identityCard,
                            birthDay = birthDay,
                            userFace = userFace,
                            email = email,
                            address = address
                        };

                        userGroupUserObjList.Add(userObj);

                    }



                    object userGroupObj = new object();

                    userGroupObj = new
                    {

                        uGId = uGId,
                        modifyTime = modifyTime,

                        name = name,
                        createUId = createUId,
                        userGroupUser = userGroupUserObjList,
                    };

                    userGroupObjList.Add(userGroupObj);

                }

            }
            else
            {
                status = "error";
                msg = "获取失败";
                userGroupObjList = new List<object>();
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：GetUserGroupUser； ";
            title += "用户Id：" + i_uId + "，获取群用户：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg, userGroup = userGroupObjList };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 4.创建群
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUserGroup()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string name = Request.Params.Get("name");
            string createUId = Request.Params.Get("createUId");
            string userStatusStr = Request.Params.Get("status");
            string gType = Request.Params.Get("gType");

            int i_createUId = 0;
            try
            {
                i_createUId = int.Parse(createUId);
            }
            catch { }

            int i_gType = 0;
            try
            {
                i_gType = int.Parse(gType);
            }
            catch { }

            int userStatus = 1;
            try
            {
                userStatus = int.Parse(userStatusStr);
            }
            catch { }

            UserGroup userGroup = new UserGroup();

            userGroup.Name = name;
            userGroup.CreateUId = i_createUId;
            userGroup.GType = i_gType;
            userGroup.Status = userStatus;

            //int result = userGroup.Add();
            int result = userGroup.AddBackId();

            int uGId = 0;
            
            if (result > 0)
            {
                status = "succeed";
                msg = "创建成功";

                //uGId = userGroup.GetUserGroupId();
                uGId = result;

                UserGroupUser userGroupUser = new UserGroupUser();

                int i_uGId = uGId;
                int i_uId = i_createUId;
                int i_uRole = 1;
                userGroupUser.UGId = i_uGId;
                userGroupUser.UId = i_uId;
                userGroupUser.URole = i_uRole;
                userGroupUser.Status = userStatus;
                result = userGroupUser.Add();

            }
            else
            {
                status = "error";
                msg = "创建失败";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：CreateUserGroup； ";
            title += "群名称：" + name + "，创建用户：" + i_createUId + "，创建群：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg, uGId = uGId };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyUserGroupName()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");
            string name = Request.Params.Get("name");

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch { }

            UserGroup userGroup = new UserGroup();
            userGroup.Name = name;
            userGroup.Id = i_uGId;


            int result = userGroup.ModifyName();

            if (result > 0)
            {
                status = "succeed";
                msg = "修改成功";

            }
            else
            {
                status = "error";
                msg = "修改失败";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：ModifyUserGroupName； ";
            title += "群Id：" + i_uGId + "，群名称：" + name + "，修改群名称：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserGroup()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch { }

            UserGroup userGroup = new UserGroup();
            userGroup.Id = i_uGId;
            int ug_status = 0;
            userGroup.Status = ug_status;

            int result = userGroup.ModifyStatus();

            if (result > 0)
            {
                status = "succeed";
                msg = "删除成功";

            }
            else
            {
                status = "error";
                msg = "删除失败";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：DeleteUserGroup； ";
            title += "群Id：" + i_uGId + "，删除群：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }


        /// <summary>
        /// 添加群用户，支持多个
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserGroupUser()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");
            string uId = Request.Params.Get("uId");// ","号隔开
            string uRole = Request.Params.Get("uRole");//角色统一

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch { }

            string[] uIdList = uId.Split(',');

            int i_uId = 0;

            int i_uRole = 0;
            try
            {
                i_uRole = int.Parse(uRole);
            }
            catch { }

            int count=uIdList.Length;

            int addResult = 0;
            string  uIdStr = "";

            for (int i = 0; i < count; i++)
            {
                uIdStr = uIdList[i];
                try
                {
                    i_uId = int.Parse(uIdStr);
                }
                catch { i_uId = 0; }

                int userStatus = 1;

                UserGroupUser userGroupUser = new UserGroupUser(i_uId, i_uGId);

                int Id = userGroupUser.Id;

                userGroupUser.UGId = i_uGId;
                userGroupUser.UId = i_uId;
                userGroupUser.URole = i_uRole;

                int result = 0;
                if (Id > 0)
                {
                    userStatus = userGroupUser.Status;

                    if (userStatus < 1)
                    {
                        userGroupUser.ModifyStatus();
                        addResult++;//添加成功
                    }
                    else
                    {
                        //status = "error";
                        //msg = "添加失败，用户已加入该群";
                    }
                }
                else
                {
                    userStatus = 1;
                    userGroupUser.Status = userStatus;
                    result = userGroupUser.Add();

                    if (result > 0)
                    {
                        //status = "succeed";
                       // msg = "添加成功";
                        addResult++;//添加成功
                    }
                    else
                    {
                        //status = "error";
                       // msg = "添加失败";
                    }
                }
            }

            if (addResult > 0)
            {
                status = "succeed";
                msg = "添加成功："+addResult+"条数据添加成功；"+(count-addResult)+"条数据添加失败";

            }
            else
            {
                status = "error";
                msg = "添加失败";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：AddUserGroupUser； ";
            title += "群Id：" + i_uGId + "， 用户Id：" + uId + "，角色：" + uRole + "，添加群用户，支持多个：";
            Common.addLog(logType, title + msg);

            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 6.删除群用户，支持多个
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserGroupUser()
        {
            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");
            string uId = Request.Params.Get("uId");

            //int i_uGId = int.Parse(uGId);
            //int i_uId = int.Parse(uId);

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch { }

            string[] uIdList = uId.Split(',');

            int count=uIdList.Length;

            int delResult = 0;
            string uIdStr = "";

            for (int i = 0; i < count; i++)
            {
                uIdStr = uIdList[i];

                int i_uId = 0;
                try
                {
                    i_uId = int.Parse(uIdStr);
                }
                catch { }

                int userStatus = 0;
                UserGroupUser userGroupUser = new UserGroupUser(i_uId, i_uGId);
                int Id = userGroupUser.Id;
                userGroupUser.Status = userStatus;

                int result = 0;
                if (Id > 0)
                {
                    result = userGroupUser.ModifyStatus();

                    if (result > 0)
                    {
                        delResult++;//添加成功

                        //status = "succeed";
                        //msg = "删除成功";
                    }
                    else
                    {
                        //status = "error";
                        //msg = "删除失败";
                    }

                }
                else
                {
                    //status = "error";
                    //msg = "信息不存在，无法删除";

                }
            }

            if (delResult > 0)
            {
                status = "succeed";
                msg = "删除成功：" + delResult + "条数据删除成功；" + (count - delResult) + "条数据删除失败";

            }
            else
            {
                status = "error";
                msg = "删除失败";
            }


            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：DeleteUserGroupUser； ";
            title += "群Id：" + i_uGId + "， 用户Id：" + uId + "，删除群用户：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);
        }

        /// <summary>
        /// 修改群用户角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyUserGroupUserRole()
        {

            string status = "error";
            string msg = "";
            string title = "";

            string uGId = Request.Params.Get("uGId");
            string uId = Request.Params.Get("uId");
            string uRole = Request.Params.Get("uRole");

            int i_uGId = 0;
            try
            {
                i_uGId = int.Parse(uGId);
            }
            catch { }
            int i_uId = 0;
            try
            {
                i_uId = int.Parse(uId);
            }
            catch { }

            int i_uRole = 0;
            try
            {
                i_uRole = int.Parse(uRole);
            }
            catch { }

            int userStatus = 1;

            UserGroupUser userGroupUser = new UserGroupUser(i_uId, i_uGId);

            int Id = userGroupUser.Id;

            userGroupUser.UGId = i_uGId;
            userGroupUser.UId = i_uId;
            userGroupUser.URole = i_uRole;


            int result = 0;
            if (Id > 0)
            {
                userStatus = userGroupUser.Status;
                if (userStatus < 1)
                {
                    status = "error";
                    msg = "修改失败，用户已退群";
                }
                else
                {
                    result = userGroupUser.Modify_uRole();
                    if (result > 0)
                    {
                        status = "succeed";
                        msg = "修改成功";
                    }
                    else
                    {
                        status = "error";
                        msg = "修改失败";
                    }
                }
            }
            else
            {
                    status = "error";
                    msg = "修改失败，用户未加入该群";
            }

            int logType = 1;
            string ip = Request.UserHostAddress;
            title += "API：ModifyUserGroupUserRole； ";
            title += "群Id：" + i_uGId + "， 用户Id：" + uId + "，角色：" + uRole + "，修改群用户角色：";
            Common.addLog(logType, title + msg);


            object obj = new { status = status, msg = msg };
            string contentType = "text/json; charset=utf-8";

            return Json(obj, contentType);

        }


        #endregion

    }
}
