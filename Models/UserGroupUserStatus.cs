using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserGroupUserStatus:CommonModel
    {
        #region 参数

        private int _id = 0;
        private int _uId = 0;
        //群Id
        private int _uGId = 0;
        //群类型，工作群，好友群等

        private DateTime _addTime = DateTime.Now;
        private DateTime _uploadTime = DateTime.Now;
        private int _status = 0;


        #endregion

        #region 属性
        ///<summary>
        /// Id
        ///</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        ///<summary>
        /// UId
        ///</summary>
        public int UId
        {
            get { return _uId; }
            set { _uId = value; }
        }

        ///<summary>
        /// UGId ,群Id
        ///</summary>
        public int UGId
        {
            get { return _uGId; }
            set { _uGId = value; }
        }
        /// <summary>
        /// AddTime
        /// </summary>
        public DateTime AddTime
        {
            get { return this._addTime; }
            set { this._addTime = value; }
        }

        /// <summary>
        /// UploadTime
        /// </summary>
        public DateTime UploadTime
        {
            get { return this._uploadTime; }
            set { this._uploadTime = value; }
        }
        /// <summary>
        /// Status
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        public UserGroupUserStatus(): base()
        {
            init();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[userGroup_user_status]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "uId,uGId,status,addTime,modifyTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@uGId", _uGId),
                new SqlParameter("@status", _status),
                new SqlParameter("@addTime", _addTime),
                new SqlParameter("@uploadTime", _uploadTime),
             
            };
            return base.Add(value,para);
        }

        public int ModifyStatus()
        {
            string set = "status=@status";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@status", _status),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }



    }
}