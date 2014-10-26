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

        private int _Id = 0;
        private int _uId = 0;

        private string _gName = string.Empty;

        private int _status = 1;


        #endregion

        #region 属性
        ///<summary>
        /// Id
        ///</summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
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

        #endregion

        public UserFriendGroup(): base()
        {
            init();
        }

        public UserFriendGroup(int Id)
        {
            init();

            string strSql = " Select table1.* from " + this._table + " as table1  where 1=1 and table1.Id = @Id  ";

            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            dt = base.GetDataList(strSql,para);

            if (dt.Rows.Count > 0)
            {
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());
                this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                this._gName = dt.Rows[0]["gName"].ToString();

                //status
                this._status = int.Parse(dt.Rows[0]["status"].ToString());


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
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());
                this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                this._gName = dt.Rows[0]["gName"].ToString();

                //status
                this._status = int.Parse(dt.Rows[0]["status"].ToString());


            }


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
            string value = "uId,gName,status";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@gName", _gName),
                new SqlParameter("@status", _status),
             
            };
            return base.Add(value,para);
        }

        public int AddBackId()
        {
            string value = "uId,gName,status";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@gName", _gName),
                new SqlParameter("@status", _status),
             
            };
            return base.AddBackId(value, para);
        }

        public int ModifyStatus()
        {
            string set = "status=@status";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@status", _status),
                new SqlParameter("@Id", _Id),
			};
            return base.Modify(set, para);
        }

        public int ModifyName()
        {
            string set = "gName=@gName";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@gName", _gName),
                new SqlParameter("@Id", _Id),
			};
            return base.Modify(set, para);
        }

    }
}