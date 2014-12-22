using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserFriendGroup:CommonModel
    {
        #region 参数

        private int _id = 0;
        private int _uId = 0;

        private string _gName = string.Empty;

        private int _status = 1;

        private DateTime _modifyTime = DateTime.Now;
        private int _isOnToHide = 0;
        private int _isOffToVisible = 0;


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
        /// GName
        ///</summary>
        public string GName
        {
            get { return _gName; }
            set { _gName = value; }
        }

        /// <summary>
        /// Status
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// ModifyTime
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this._modifyTime; }
            set { this._modifyTime = value; }
        }
        
        /// <summary>
        /// IsOnToHide,在线是否对用户分组隐身
        /// </summary>
        public int IsOnToHide
        {
            get { return _isOnToHide; }
            set { _isOnToHide = value; }
        }
        /// <summary>
        /// IsOffToVisible，隐身是否对用户分组可见
        /// </summary>
        public int IsOffToVisible
        {
            get { return _isOffToVisible; }
            set { _isOffToVisible = value; }
        }

        #endregion

        public UserFriendGroup(): base()
        {
            init();
        }

        public UserFriendGroup(int id)
        {
            init();

            if (id > 0)
            {
                string strSql = " Select table1.* from " + this._table + " as table1  where 1=1 and table1.Id = @Id  ";

                DataTable dt = new DataTable();

                SqlParameter[] para = new SqlParameter[]
			    {
				    new SqlParameter("@Id", id),
			    };
                dt = base.GetDataList(strSql, para);

                if (dt.Rows.Count > 0)
                {
                    this.SetField(dt);
                }
            }
        }

        public UserFriendGroup(int uId, string gName)
        {
            init();

            string strSql = " Select table1.* from " + this._table + " as table1  where 1=1 and table1.uId = @uId and table1.gName = @gName ";

            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@uId", uId),
				new SqlParameter("@gName", gName),
			};
            dt = base.GetDataList(strSql, para);

            if (dt.Rows.Count > 0)
            {
                this.SetField(dt);
                /*
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());
                this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                this._gName = dt.Rows[0]["gName"].ToString();

                //status
                this._status = int.Parse(dt.Rows[0]["status"].ToString());
                */

            }


        }

        protected void SetField(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._id = int.Parse(dt.Rows[0]["Id"].ToString());
                    this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                    this._gName = dt.Rows[0]["gName"].ToString();

                    //status
                    this._status = int.Parse(dt.Rows[0]["status"].ToString());

                    string modifyTime = dt.Rows[0]["modifyTime"].ToString();

                    this._modifyTime = DateTime.Parse(modifyTime);
                    this._isOnToHide = int.Parse(dt.Rows[0]["isOnToHide"].ToString());
                    this._isOffToVisible = int.Parse(dt.Rows[0]["isOffToVisible"].ToString());

                }
            }
            catch { }

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[userFriend_group]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "uId,gName,status,modifyTime,isOnToHide,isOffToVisible";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@gName", _gName),
                new SqlParameter("@status", _status),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@isOnToHide", _isOnToHide),
                new SqlParameter("@isOffToVisible", _isOffToVisible),             
            };
            return base.Add(value,para);
        }

        public int AddBackId()
        {
            string value = "uId,gName,status,modifyTime,isOnToHide,isOffToVisible";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@gName", _gName),
                new SqlParameter("@status", _status),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@isOnToHide", _isOnToHide),
                new SqlParameter("@isOffToVisible", _isOffToVisible),
            };
            return base.AddBackId(value, para);
        }

        public int ModifyStatus()
        {
            string set = "status=@status,modifyTime=@modifyTime";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@status", _status),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        public int ModifyOnline()
        {
            string set = "isOnToHide=@isOnToHide,modifyTime=@modifyTime";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@isOnToHide", _isOnToHide),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        public int ModifyOffline()
        {
            string set = "isOffToVisible=@isOffToVisible,modifyTime=@modifyTime";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@isOffToVisible", _isOffToVisible),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }


        /// <summary>
        /// 删除好友分组
        /// </summary>
        /// <returns></returns>
        public int DeleteUserFriendGroup()
        {
            this._status = 0;
            return this.ModifyStatus();
        }


        public int ModifyName()
        {
            string set = "gName=@gName";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@gName", _gName),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        public DataTable GetUserFriendGroup()
        {
            string strSql = string.Empty;
            int status = 0;
            strSql += " Select table1.* from " + this._table + " as table1 ";
            strSql += " where 1=1 and table1.uId = @uId and table1.status > @status ";
            strSql += " order by table1.Id desc ";

            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@uId", this._uId),
                new SqlParameter("@status", status),
			};
            dt = base.GetDataList(strSql, para);

            return dt;

        }


    }
}