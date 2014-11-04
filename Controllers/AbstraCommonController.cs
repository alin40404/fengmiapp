using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fengmiapp.Controllers
{
    public abstract class AbstraCommonController : Controller
    {
        private bool _isEffetive = false;
        private string _validMsg = string.Empty;

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        protected bool IsEffetive
        {
            get { return this._isEffetive; }
            set { this._isEffetive = value; }
        }

        /// <summary>
        /// 验证信息
        /// </summary>
        protected string ValidMsg
        {
            get { return this._validMsg; }
            set { this._validMsg = value; }
        }

        protected void init()
        {
            string t = Request.Params.Get("t");
            string token = Request.Params.Get("token");
            this._isEffetive = Common.IsValidSecretSuccess(token, t);
            if (this._isEffetive)
            {
                this._validMsg = "密钥验证通过";
            }
            else
            {
                this._validMsg = "密钥验证无效，服务器拒绝访问";
            }
        }
    }
}
