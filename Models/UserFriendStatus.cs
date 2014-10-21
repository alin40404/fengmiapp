using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserFriendStatus:CommonModel
    {
        #region 参数

        private int _id = 0;
        private int _uId = 0;
        //关联的好友UId
        private int _fuId = 0;

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
        /// FuId ,关联的好友UId
        ///</summary>
        public int FuId
        {
            get { return _fuId; }
            set { _fuId = value; }
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

        public UserFriendStatus():base()
        {
            init();
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            this.Table = "[userFriend_status]";
        }

        public int Add()
        {
            string value = "uId,fuId,status,addTime,uploadTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@fuId", _fuId),
                new SqlParameter("@status", _status),
                new SqlParameter("@addTime", _addTime),
                new SqlParameter("@uploadTime", _uploadTime),
             
            };
            return base.Add(value,para);
        }

        public int Modify()
        {
            string set =
                    "status=@status";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@status", _status),
			};
            return base.Modify(set,para);
        }


    }
}