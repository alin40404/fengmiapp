using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace fengmiapp
{
    public class Email
    {
        #region 参数
        //ehoneynet@126.com
        //123456aaaaaa
        public string _mailFrom = "support@ehoneynet.com";
        public string _mailPwd = "abc@123";
        //public string _host = "smtp.126.com";
        public string _host = "smtp.qq.com";
        
        public string[] _mailToArray = null;
        public string[] _mailCcArray = null;
        public string _mailSubject = string.Empty;
        public string _mailBody = string.Empty;
        public bool _isbodyHtml = true;
        public string[] _attachmentsPath = null;
        
        #endregion

        #region 属性
        /// <summary>
        /// 发送者
        /// </summary>
        public string MailFrom {
            get { return this._mailFrom; } 
            set { this._mailFrom = value; } 
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] MailToArray {
            get { return this._mailToArray; }
            set { this._mailToArray = value; } 
        }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] MailCcArray
        {
            get { return this._mailCcArray; }
            set { this._mailCcArray = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string MailSubject
        {
            get { return this._mailSubject; }
            set { this._mailSubject = value; }
        }

        /// <summary>
        /// 正文
        /// </summary>
        public string MailBody
        {
            get { return this._mailBody; }
            set { this._mailBody = value; }
        }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string MailPwd
        {
            get { return this._mailPwd; }
            set { this._mailPwd = value; }
        }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string Host
        {
            get { return this._host; }
            set { this._host = value; }
        }

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool IsbodyHtml
        {
            get { return this._isbodyHtml; }
            set { this._isbodyHtml = value; }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] AttachmentsPath
        {
            get { return this._attachmentsPath; }
            set { this._attachmentsPath = value; }
        }
        #endregion

        public Email() { }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public bool Send()
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(MailFrom);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();


            //向收件人地址集合添加邮件地址
            if (MailToArray != null)
            {
                for (int i = 0; i < MailToArray.Length; i++)
                {
                    myMail.To.Add(MailToArray[i].ToString());
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (MailCcArray != null)
            {
                for (int i = 0; i < MailCcArray.Length; i++)
                {
                    myMail.CC.Add(MailCcArray[i].ToString());
                }
            }
            //发件人地址
            myMail.From = maddr;

            //电子邮件的标题
            myMail.Subject = MailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = MailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            myMail.Priority = MailPriority.High;

            myMail.IsBodyHtml = IsbodyHtml;

            //在有附件的情况下添加附件
            try
            {
                if (AttachmentsPath != null && AttachmentsPath.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in AttachmentsPath)
                    {
                        attachFile = new Attachment(path);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                string tempMsg = string.Empty;
                tempMsg += "Email类，Send方法异常,在添加附件时有错误：";
                tempMsg += err.Message;
                Common.addLog(0, tempMsg);


                throw new Exception("在添加附件时有错误:" + err);
            }

            SmtpClient smtp = new SmtpClient();
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(MailFrom, MailPwd);


            //设置SMTP邮件服务器
            smtp.Host = Host;

            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                return true;

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                string tempMsg = string.Empty;
                tempMsg += "Email类，Send方法异常：";
                tempMsg += ex.Message;
                Common.addLog(0, tempMsg);

                return false;
            }

        }
    
    }
}