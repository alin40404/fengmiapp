using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Web;

namespace fengmiapp
{
    [Serializable()]
    public class SystemCookies
    {
        private string _UserID = String.Empty;
        private string _UserName = String.Empty;
        private string _Role = String.Empty;
        private string _CompanyID = String.Empty;

        /// <summary>
        /// UserID
        /// </summary>
        public string UserID
        {
            get { return this._UserID; }
            set { this._UserID = value; }
        }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return this._UserName; }
            set { this._UserName = value; }
        }
        /// <summary>
        /// CompanyID，暂时不用
        /// </summary>
        public string CompanyID
        {
            get { return this._CompanyID; }
            set { this._CompanyID = value; }
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        public string Role
        {
            get { return this._Role; }
            set { this._Role = value; }
        }

        public SystemCookies()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取Cookie对象
        /// </summary>
        public SystemCookies(string cookie)
        {

            SystemCookies systemCookiesObj = this.ReturnCookies(cookie); ;
            if (systemCookiesObj != null)
            {
                if (cookie == "Systemcookie")
                {
                    this._UserID = systemCookiesObj.UserID;
                    this._UserName = systemCookiesObj.UserName;
                    this._Role = systemCookiesObj.Role;
                    this._CompanyID = systemCookiesObj.CompanyID;

                }
            }
        }
        /// <summary>
        /// ReturnCookies
        /// </summary>
        private SystemCookies ReturnCookies(string type)
        {
            HttpRequest httpRequestObj = System.Web.HttpContext.Current.Request;
            SystemCookies systemCookiesObj = new SystemCookies();
            if (type == "Systemcookie")
            {
                if (httpRequestObj.Cookies["Systemcookie"] != null && httpRequestObj.Cookies["Systemcookie"].Value != "")
                {
                    systemCookiesObj = systemCookiesObj.GetSystemCookies(httpRequestObj.Cookies["Systemcookie"].Value);
                    return systemCookiesObj;
                }
                else
                {
                    return null;
                }
            }
            return systemCookiesObj;
        }

        /// <summary>
        /// 对象序列化为字符串
        /// </summary>
        /// <param name="systemCookiesObj"></param>
        /// <returns></returns>
        public string GetString(SystemCookies systemCookiesObj)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter formatterObj = new BinaryFormatter();

            try
            {
                formatterObj.Serialize(memStream, systemCookiesObj);
                byte[] binaryDataResult = memStream.ToArray();
                return this.GetStringsByByte(binaryDataResult);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                memStream.Close();
                //memStream.Dispose();
            }
        }

        /// <summary>
        /// 字符串反序列化为对象
        /// </summary>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public SystemCookies GetSystemCookies(string strObj)
        {
            byte[] binaryDataResult = this.GetByteByStrings(strObj);

            MemoryStream memStream = new MemoryStream(binaryDataResult);
            BinaryFormatter formatterObj = new BinaryFormatter();

            try
            {
                object obj = formatterObj.Deserialize(memStream);
                return (SystemCookies)obj;
            }
            catch
            {
                return null;
            }
            finally
            {
                memStream.Close();
                //memStream.Dispose();
            }
        }

        #region 字节和字符串相互转换的函数
        private string GetStringsByByte(byte[] binaryDataResult)
        {
            StringBuilder strBinaryString = new StringBuilder();
            strBinaryString.Append(",");
            for (int i = 0; i < binaryDataResult.Length; i++)
            {
                strBinaryString.Append(binaryDataResult[i]).Append(",");
            }

            if (strBinaryString.ToString() == ",")
            {
                return string.Empty;
            }
            else
            {
                return strBinaryString.ToString();
            }
        }
        private byte[] GetByteByStrings(string strBinaryString)
        {
            //传进来的是这样的一串字符串,例如:",0,0,1,255,255,80,10,13,12," 现在就是要取出其中的数字
            ArrayList list = new ArrayList();
            string strOneByte = "";
            int FirstCommaIndex = 0;
            int SecondCommaIndex = 0;

            while (SecondCommaIndex < strBinaryString.Length - 1)
            {
                if (strBinaryString.IndexOf(",", FirstCommaIndex + 1) == -1)
                {
                    //防止死循环
                    break;
                }

                SecondCommaIndex = strBinaryString.IndexOf(",", FirstCommaIndex + 1);
                strOneByte = strBinaryString.Substring(FirstCommaIndex + 1, SecondCommaIndex - FirstCommaIndex - 1);
                list.Add(strOneByte);
                FirstCommaIndex = SecondCommaIndex;
            }
            int intListCount = list.Count;

            byte[] binaryDataResult = new byte[intListCount];
            for (int i = 0; i < intListCount; i++)
            {
                binaryDataResult[i] = Convert.ToByte(list[i].ToString());
            }

            return binaryDataResult;
        }
        #endregion
    }
}
