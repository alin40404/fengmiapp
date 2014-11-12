using fengmiapp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        /// 上传头像
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
                            string _fileNamePath = Request.Files["uploadfile"].FileName;
                            int fileLength = Request.Files["uploadfile"].ContentLength;
                            Stream fileStream = Request.Files["uploadfile"].InputStream;

                            byte[] fileData = new byte[fileLength];
                            fileStream.Read(fileData, 0, fileLength);

                            string _savedFileResult = this.uploadFile(_fileNamePath, fileData); //开始上传
                            //把编译成的json格式返回到前台
                            msg = _savedFileResult;
                            int result = 0;
                            if (this._isReusable)
                            {
                                adminUser.UserFace = this._saveImageUrl;
                                result = adminUser.ModifyUserFace();
                                if (result > 0)
                                {
                                    status = "succeed";
                                }
                                else
                                {
                                    status = "error";
                                    msg = "头像保存失败";

                                }
                            }
                            else
                            {
                                status = "error";
                            }
                        }
                        catch
                        {
                            status = "error";
                            msg = "未知错误";
                        }
                    }
                    else
                    {
                        status = "error";
                        msg = "修改头像失败,用户帐号不存在";
                    }
                }
            }
            else
            {
                msg = this.ValidMsg;
            }

            ip = Request.UserHostAddress;
            title += "API：UploadFile； ";
            title += "用户Id：" + i_uId + "，上传头像：";
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

        private string uploadFile(string filenamePath)
        {
            if (string.IsNullOrEmpty(filenamePath))
            {
                this._isReusable = false;
                return "图片文件为空";

            }
            //图片格式
            string fileNameExit = filenamePath.Substring(filenamePath.IndexOf('.')).ToLower();
            if (!this.checkfileExit(fileNameExit))
            {
                this._isReusable = false;
                return "图片格式不正确";
            }
            //保存路径
            string toFilePath = "../Content/UploadFiles/";
            //物理完整路径
            //HttpContext.Current.
            string toFileFullPath = Server.MapPath(toFilePath);
            //检查是否有该路径，没有就创建
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            //生成将要保存的随机文件名
            string toFileName =Common.GetFileName();

            //将要保存的完整路径
            string saveFile = toFileFullPath + toFileName + fileNameExit;

            //创建WebClient实例
            WebClient myWebClient = new WebClient();
            //设定window网络安全认证
            /*Credentials 属性包含的身份验证凭据用于访问主机上的资源。
             * 在多数客户端方案中，应使用 DefaultCredentials，这是当前登录的用户的凭据。
             * 为此，将 UseDefaultCredentials 属性设置为 true，而不是设置此属性。
             * */
            myWebClient.Credentials = CredentialCache.DefaultCredentials;

            //要上传的文件
            FileStream fs = new FileStream(filenamePath, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);
            //使用UploadFile方法可以用下面的格式
            //将本地文件发送到资源，并返回包含任何响应的 Byte 数组。
            myWebClient.UploadFile(saveFile, filenamePath);
            this._isReusable = true;
            return "图片保存成功";
        }

        private string uploadFile(string filenamePath, byte[] fileData)
        {
            if (string.IsNullOrEmpty(filenamePath))
            {
                this._isReusable = false;
                this._saveImageUrl = string.Empty;
                return "图片文件为空";

            }
            //图片格式
            string fileNameExit = filenamePath.Substring(filenamePath.IndexOf('.')).ToLower();
            if (!this.checkfileExit(fileNameExit))
            {
                this._isReusable = false;
                return "图片格式不正确";
            }
            //保存路径
            string toFilePath = "../Content/UploadFiles/";
            //物理完整路径
            //HttpContext.Current.
            string toFileFullPath = Server.MapPath(toFilePath);
            //检查是否有该路径，没有就创建
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            //生成将要保存的随机文件名
            string toFileName = Common.GetFileName();

            //将要保存的完整路径
            string saveFile = toFileFullPath + toFileName + fileNameExit;
            string saveUrlFile = toFilePath + toFileName + fileNameExit;

            Image headimage = this.GetPicture(fileData);
            headimage.Save(saveFile);
            this._saveImageUrl = saveUrlFile;
            this._isReusable = true;
            return "图片保存成功";
        }

        /// <summary>
        /// 检查图片格式
        /// </summary>
        /// <param name="_fileExit">文件后缀名</param>
        /// <returns></returns>
        private bool checkfileExit(string _fileExit)
        {
            string[] allowExit = new string[] { ".gif", ".jpg", ".png" };//判断文件类型
            for (int i = 0; i < allowExit.Length; i++)
            {
                if (allowExit[i] == _fileExit)
                {
                    //符合文件类型则返回true;
                    return true;
                }
            }
            return false;
        }
      
        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        private byte[] GetPictureData(string imagepath)
        {
            /**/
            ////根据图片文件的路径使用文件流打开，并保存为byte[] 
            FileStream fs = new FileStream(imagepath, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }

        /// <summary>
        /// 二进制转图片
        /// </summary>
        /// <param name="streamByte"></param>
        /// <returns></returns>
        public System.Drawing.Image GetPicture(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

    }
}
