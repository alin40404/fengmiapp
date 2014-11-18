using fengmiapp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace fengmiapp.Controllers
{
    public class CommonController : AbstraCommonController
    {
        #region 属性
        private bool _isReusable = false;
        protected bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get
            {
                return this._isReusable;
            }
            set { _isReusable = value; }
        }

        private string _saveImageUrl = string.Empty;
        protected string SaveImageUrl
        {
            get
            {
                return this._saveImageUrl;
            }
            set { _saveImageUrl = value; }
        }

        #endregion

        public string token()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime now=DateTime.Now;
            long t = (long)(now - startTime).TotalMilliseconds;
            string secretKey = "DEVFORUSER-ANDRIOD-IOS-CRM-001-KEY";
            t = 1415853862667;
            string param=string.Empty;
            param += "t=" + t;
            param += secretKey;
            string token = string.Empty;
            token = Common.MD5(param);

            string msg = string.Empty;
            msg = "t=" + t + " token=" + token;
            return msg;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            this.init();

            string status = "error";
            string msg = "";
            string title = "";
            int logType = 1;
            string ip = string.Empty;

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

            if (this.IsEffetive)
            {
                if (i_uId <= 0)
                {
                    status = "error";
                    msg = "帐号不存在";
                }
                else
                {
                    User adminUser = new User(i_uId);
                    int Id = adminUser.Id;
                    if (Id > 0)
                    {//update
                        //在此写入您的处理程序实现。
                        //源图片路径
                        try
                        {
                            /*
                            string _fileNamePath = Request.Files["uploadfile"].FileName;
                            int fileLength = Request.Files["uploadfile"].ContentLength;
                            Stream fileStream = Request.Files["uploadfile"].InputStream;

                            byte[] fileData = new byte[fileLength];
                            fileStream.Read(fileData, 0, fileLength);

                            string _savedFileResult = this.uploadFile(_fileNamePath, fileData); //开始上传
                           
                            msg = _savedFileResult;
                            */

                            this._isReusable = Fileupload.UploadFile(Request.Files["uploadfile"], out this._saveImageUrl,out msg);
                           
                            int result = 0;
                            if (this._isReusable)
                            {
                                adminUser.UserFace = this._saveImageUrl;
                                result = adminUser.ModifyUserFace();
                                if (result > 0)
                                {
                                    status = "succeed";
                                    msg = "图片保存成功";
                                }
                                else
                                {
                                    status = "error";
                                    msg = "图片保存失败";

                                }
                            }
                            else
                            {
                                status = "error";
                                //msg = "图片上传失败";
                            }
                        }
                        catch(Exception ex)
                        {
                            status = "error";
                            msg = "未知错误";
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = "修改图片失败,用户帐号不存在";
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            //ip = Request.UserHostAddress;
            ip = Request.Url.Host;
            title += "API：UploadFile； ";
            title += "用户Id：" + i_uId + "，上传图片：";
            Common.addLog(logType, title + msg);

            string port = Request.Url.Port.ToString();
            string host = "http://" + ip + ":" + port + "/";
            string userFace = string.Empty;
            if (this._saveImageUrl != string.Empty)
            {
                userFace = host + this._saveImageUrl;
            }

            object obj = new { status = status, msg = msg, userFace = userFace };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFileTest()
        {
            //this.init();

            string status = "error";
            string msg = "";
            string title = "";
            int logType = 1;
            string ip = string.Empty;

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

            //if (this.IsEffetive)
            if (true)
            {
                if (i_uId <= 0)
                {
                    status = "error";
                    msg = "帐号不存在";
                }
                else
                {
                    User adminUser = new User(i_uId);
                    int Id = adminUser.Id;
                    if (Id > 0)
                    {//update
                        //在此写入您的处理程序实现。
                        //源图片路径
                        try
                        {
                            /*
                            string _fileNamePath = Request.Files["uploadfile"].FileName;
                            int fileLength = Request.Files["uploadfile"].ContentLength;
                            Stream fileStream = Request.Files["uploadfile"].InputStream;

                            byte[] fileData = new byte[fileLength];
                            fileStream.Read(fileData, 0, fileLength);

                            string _savedFileResult = this.uploadFile(_fileNamePath, fileData); //开始上传
                           
                            msg = _savedFileResult;
                            */

                            this._isReusable = Fileupload.UploadFile(Request.Files["uploadfile"], out this._saveImageUrl, out msg);

                            int result = 0;
                            if (this._isReusable)
                            {
                                adminUser.UserFace = this._saveImageUrl;
                                result = adminUser.ModifyUserFace();
                                if (result > 0)
                                {
                                    status = "succeed";
                                    msg = "图片保存成功";
                                }
                                else
                                {
                                    status = "error";
                                    msg = "图片保存失败";

                                }
                            }
                            else
                            {
                                status = "error";
                                //msg = "图片上传失败";
                            }
                        }
                        catch (Exception ex)
                        {
                            status = "error";
                            msg = "未知错误";
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = "修改图片失败,用户帐号不存在";
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            //ip = Request.UserHostAddress;
            ip = Request.Url.Host;
            title += "API：UploadFileTest； ";
            title += "用户Id：" + i_uId + "，上传图片：";
            Common.addLog(logType, title + msg);

            string port = Request.Url.Port.ToString();
            string host = "http://" + ip + ":" + port + "/";
            string userFace = string.Empty;
            if (this._saveImageUrl != string.Empty)
            {
                userFace = host + this._saveImageUrl;
            }

            object obj = new { status = status, msg = msg, userFace = userFace };
            string contentType = "text/json; charset=utf-8";
            return Json(obj, contentType);
        }

        public ActionResult Introduce()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

    }
}
