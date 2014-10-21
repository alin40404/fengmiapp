using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;
using fengmiapp.Models;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace fengmiapp
{
    public class Common
    {
        #region dataTable转换成Json格式
        /// <summary>      
        /// dataTable转换成Json格式      
        /// </summary>      
        /// <param name="dt"></param>      
        /// <returns></returns>      
        public static String ToJson(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<String, object>> list = new List<Dictionary<String, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }

            return serializer.Serialize(list);
        }
        #endregion dataTable转换成Json格式

        #region DataSet转换成Json格式
        /// <summary>      
        /// DataSet转换成Json格式      
        /// </summary>      
        /// <param name="ds">DataSet</param>      
        /// <returns></returns>      
        public static string ToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(ToJson(dt));
                json.Append("}");
            }
            return json.ToString();
        }
        #endregion

        #region  验证是否登录
        /// <summary> 
        /// 对管理员用户cookie进行检测
        /// </summary>      
        public static bool isLogin()
        {
            HttpCookie SysCookies = System.Web.HttpContext.Current.Request.Cookies["Systemcookie"];
            string strNoticeInfo = string.Empty;
            #region 检测Cookies对象
            if (SysCookies == null || SysCookies.Value == "")
            {
                return false;
            }
            #endregion

            #region 检测用户信息是否丢失
            SystemCookies CookieObj = new SystemCookies("Systemcookie");
            string adminIdStr = CookieObj.UserID;
            string powerStr = CookieObj.Role;
            string userName = CookieObj.UserName;

            if (adminIdStr == "")
            {
                return false;
            }
            System.Web.HttpContext.Current.Session["adminId"] = adminIdStr;
            System.Web.HttpContext.Current.Session["adminName"] = userName;
            System.Web.HttpContext.Current.Session["Power"] = powerStr;
            #endregion
            //通过检测
            return true;
        }

        #endregion

        #region  是否有权限
        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="actionPower"></param>
        /// <returns></returns>
        public static bool isHavePower(string actionPower)
        {
            try
            {
                string power = HttpContext.Current.Session["Power"].ToString();
                if (power.Contains(actionPower))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;

        }
        public static bool isHavePower()
        {
            try
            {
                string power = HttpContext.Current.Session["Power"].ToString();
                if (power=="1")
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;

        }
        #endregion

        #region 设置cookies，获取cookies
        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCookies(string key,string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookies(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            string value = "";
            if (cookie != null)
            {
                value = cookie.Value;
            }

            return value;
        }


        #endregion

        #region 添加日志

        public static void addLog(int logType, string describe)
        {
            DateTime logTime = DateTime.Now;
            Log log = new Log();
            log.LogType = logType;
            log.Describe = describe;
            log.Add();
            return;
        }

        public static void addLog(int logType, string describe, string ip, string emergeURL, int adminId)
        {
            DateTime logTime = DateTime.Now;
            Log log = new Log();
            log.LogType = logType;
            log.Describe = describe;
            log.IP = ip;
            log.EmergeURL = emergeURL;
            log.Userid = adminId;
            log.Add();
            return;
        }
        #endregion

        #region  获取权限
        public static string GetPower(string power, string xml)
        {
            string html = "";
            //提取xml文档
            XmlDocument xd = new XmlDocument();
            xd.Load(xml);
            //获取根节点
            XmlNode root = xd.DocumentElement;

            //获取节点列表
            XmlNodeList items = root.ChildNodes;
            //遍历item项
            string tempName = "";
            string tempValue = "";
            string tempTitle = "";
            string check = @"  checked=""checked""  ";

            foreach (XmlNode item in items)
            {
                //输出属性
                html += @"<div class=""rolesA clearfix"">";
                tempName = item.Attributes["name"].Value;
                tempValue = item.Attributes["value"].Value;
                tempTitle = item.Attributes["title"].Value;

                if (power.Contains(tempValue))
                {
                    check = @"  checked=""checked""  ";
                }
                else
                {
                    check = " ";
                }

                html += @"<h2><label>&nbsp;&nbsp;<input type=""hidden"" name=""" + tempName + @""" value=""" + tempValue + @"""" + check + " />" + tempTitle + "</label></h2>";
                //输出子节点
                html += @"<div class=""rolesB"">";

                foreach (XmlNode item1 in item)
                {
                    tempName = item1.Attributes["name"].Value;
                    tempValue = item1.Attributes["value"].Value;
                    tempTitle = item1.Attributes["title"].Value;

                    if (power.Contains(tempValue))
                    {
                        check = @"  checked=""checked""  ";
                    }
                    else
                    {
                        check = "  ";
                    }

                    html += @"<label><strong><input type=""checkbox"" name=""" + tempName + @""""" value=""" + tempValue + @"""" + check + " />" + tempTitle + "</strong></label>";

                    html += @"<div class=""rolesC"">";

                    foreach (XmlNode item2 in item1)
                    {
                        tempName = item2.Attributes["name"].Value;
                        tempValue = item2.Attributes["value"].Value;
                        tempTitle = item2.Attributes["title"].Value;
                        if (power.Contains(tempValue))
                        {
                            check = @"  checked=""checked"" ";
                        }
                        else
                        {
                            check = "  ";
                        }
                        html += @"<label><input type=""checkbox"" name=""" + tempName + @""" value=""" + tempValue + @"""" + check + ">" + tempTitle + "</label>&nbsp;&nbsp;&nbsp;";

                    }
                    html += @"</div>";

                }
                html += @"</div>";
                html += @"</div>";
            }

            return html;
        }

        public static string GetPower(string power, string xml,string disabled)
        {
            string html = "";
            //提取xml文档
            XmlDocument xd = new XmlDocument();
            xd.Load(xml);
            //获取根节点
            XmlNode root = xd.DocumentElement;

            //获取节点列表
            XmlNodeList items = root.ChildNodes;
            //遍历item项
            string tempName = "";
            string tempValue = "";
            string tempTitle = "";
            string check = @"  checked=""checked""  ";
            string disable = @"  disabled=""disabled""  ";
            if (string.IsNullOrEmpty(disabled)) {
                disable = "";
            }
            foreach (XmlNode item in items)
            {
                //输出属性
                html += @"<div class=""rolesA clearfix"">";
                tempName = item.Attributes["name"].Value;
                tempValue = item.Attributes["value"].Value;
                tempTitle = item.Attributes["title"].Value;

                if (power.Contains(tempValue))
                {
                    check = @"  checked=""checked""  ";
                }
                else
                {
                    check = " ";
                }

                html += @"<h2><label>&nbsp;&nbsp;<input type=""hidden"" name=""" + tempName + @""" value=""" + tempValue + @"""" + check + disable + " />" + tempTitle + "</label></h2>";
                //输出子节点
                html += @"<div class=""rolesB"">";

                foreach (XmlNode item1 in item)
                {
                    tempName = item1.Attributes["name"].Value;
                    tempValue = item1.Attributes["value"].Value;
                    tempTitle = item1.Attributes["title"].Value;

                    if (power.Contains(tempValue))
                    {
                        check = @"  checked=""checked""  ";
                    }
                    else
                    {
                        check = "  ";
                    }

                    html += @"<label><strong><input type=""checkbox"" name=""" + tempName + @""""" value=""" + tempValue + @"""" + check + disable + " />" + tempTitle + "</strong></label>";

                    html += @"<div class=""rolesC"">";

                    foreach (XmlNode item2 in item1)
                    {
                        tempName = item2.Attributes["name"].Value;
                        tempValue = item2.Attributes["value"].Value;
                        tempTitle = item2.Attributes["title"].Value;
                        if (power.Contains(tempValue))
                        {
                            check = @"  checked=""checked"" ";
                        }
                        else
                        {
                            check = "  ";
                        }
                        html += @"<label><input type=""checkbox"" name=""" + tempName + @""" value=""" + tempValue + @"""" + check + disable + ">" + tempTitle + "</label>&nbsp;&nbsp;&nbsp;";

                    }
                    html += @"</div>";

                }
                html += @"</div>";
                html += @"</div>";
            }

            return html;
        }

        #endregion

        public static void ClearSession(HttpSessionStateBase session)
        {
            session.RemoveAll();
            session.Clear();
        }
        public static void RemoveSession(string name,HttpSessionStateBase session)
        {
            session.Remove(name);
        }

        public static string getLine(string count)
        {
            string str = "";
            try
            {
                int num = int.Parse(count);
                for (int i = 1; i < num; i++)
                {
                    str += "--";
                }
            }
            catch { }
            return str;
        }

        public static string GetElement(string value,string xml)
        {
            string html = "";
            //提取xml文档
            XmlDocument xd = new XmlDocument();
            xd.Load(xml);
            //获取根节点
            XmlNode root = xd.DocumentElement;

            //获取节点列表
            XmlNodeList items = root.ChildNodes;
            //遍历item项
            string tempName = "";
            string tempValue = "";
            string tempTitle = "";
            string check = @"  checked=""checked""  ";
            string disabled = "";

            foreach (XmlNode item in items)
            {
                //输出属性
                //tempName = item.Attributes["name"].Value;
                //tempValue = item.Attributes["value"].Value;
                //tempTitle = item.Attributes["title"].Value;

                string inputType = "";
                try
                {
                    inputType = item.Attributes["inputType"].Value;
                }
                catch
                {
                    inputType = "";
                }
                if (string.IsNullOrEmpty(inputType))
                {
                    continue;
                }
                else
                {
                    //输出子节点
                    html += @"<div>";
                    switch (inputType)
                    {
                        case "radio":
                        case "checkbox":
                            tempName = item.Attributes["name"].Value;
                            foreach (XmlNode item1 in item)
                            {
                                tempValue = item1.Attributes["value"].Value;
                                tempTitle = item1.Attributes["title"].Value;
                                string datatype = item1.Attributes["datatype"].Value; 
                                datatype = @" datatype=""" + datatype+@"""";
                                string nullmsg = item1.Attributes["nullmsg"].Value;
                                nullmsg = @" nullmsg=""" + nullmsg + @"""";

                                string errormsg = item1.Attributes["errormsg"].Value;
                                errormsg = @" errormsg=""" + errormsg + @"""";

                                string classTemp = item1.Attributes["class"].Value;
                                classTemp = @" class=""" + classTemp + @"""";
                                if (value == tempValue) { check = @"  checked=""checked""  "; } else { check = ""; }

                                html += @"<input value=""" + tempValue + @"""  type=""" + inputType + @""" name=""" + tempName + @"""" + check + disabled +classTemp+datatype+nullmsg+errormsg+ "  />" + tempTitle + "&nbsp;&nbsp;";

                            }

                            break;
                        case "select":
                            foreach (XmlNode item1 in item)
                            {
                                tempValue = item1.Attributes["value"].Value;
                                tempTitle = item1.Attributes["title"].Value;

                                if (value == tempValue) { check = @"  selected=""selected""  "; } else { check = ""; }

                                html += @"<option value=""" + tempValue + @"""  type=""" + inputType + @""" " + check + disabled +"  />" + tempTitle + "</option>";

                            }

                            break;
                        default:
                            break;
                    }
                    html += @"</div>";
                }
            }

            return html;

        }

        public static string GetNameFromXML(string value, string xml)
        {
            string html = "";
            //提取xml文档
            XmlDocument xd = new XmlDocument();
            xd.Load(xml);
            //获取根节点
            XmlNode root = xd.DocumentElement;

            //获取节点列表
            XmlNodeList items = root.ChildNodes;
            //遍历item项
            string tempName = "";
            string tempValue = "";
            string tempTitle = "";
            string check = @"  checked=""checked""  ";
            string disabled = "";

            foreach (XmlNode item in items)
            {
                //输出属性
                //tempName = item.Attributes["name"].Value;
                //tempValue = item.Attributes["value"].Value;
                //tempTitle = item.Attributes["title"].Value;

                tempName = item.Attributes["name"].Value;
                foreach (XmlNode item1 in item)
                {
                    tempValue = item1.Attributes["value"].Value;
                    tempTitle = item1.Attributes["title"].Value;
                    if (tempValue == value)
                    {
                        html = tempTitle;
                        break;
                    }
                }

            }

            return html;

        }

        public static string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
            Vchar += ",A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            //int temp = -1;
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                //if (temp != -1)
                //{
                //    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                //}
                int t = rand.Next(36);
                //if (temp != -1 && temp == t)
                //{
                //    return RndNum(VcodeNum);
                //}
                //temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }
        /// <summary>
        ///  生成随机颜色
        /// </summary>
        /// <returns></returns>
        private static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ValidateCode"></param>
        public static byte[] CreateImage(string ValidateCode)
        {
            int int_ImageWidth = ValidateCode.Length * 14;
            int int_ImageHight = 29;
            Random newRandom = new Random();
            //  图高 px
            Bitmap theBitmap = new Bitmap(int_ImageWidth, int_ImageHight);
            Graphics theGraphics = Graphics.FromImage(theBitmap);
            //  白色背景
            theGraphics.Clear(Color.White);
            //  灰色边框
            theGraphics.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, int_ImageHight - 1);
            //  10pt的字体
            Font theFont = new Font("Arial", 12);
            for (int int_index = 0; int_index < ValidateCode.Length; int_index++)
            {
                string str_char = ValidateCode.Substring(int_index, 1);
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(int_index * 13 + 1 + newRandom.Next(3), 1 + newRandom.Next(3));
                theGraphics.DrawString(str_char, theFont, newBrush, thePos);
            }
            //  将生成的图片发回客户端
            MemoryStream ms = new MemoryStream();
            theBitmap.Save(ms, ImageFormat.Jpeg);

            theGraphics.Dispose();
            theBitmap.Dispose();

            return ms.ToArray();
        }

        public static string MD5(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                code = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(code, "MD5");
            }
            return code;
        }
    }
}