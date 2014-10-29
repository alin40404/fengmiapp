using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserAction:CommonModel
    {
        #region 参数

        private int _Id = 0;

        private int _uId = 0;
        private int _action = 1;
        private DateTime _modifyTime = DateTime.Now;
        private DateTime _uploadTime = DateTime.Now;

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
        /// Action, 默认1，摇一摇
        ///</summary>
        public int Action
        {
            get { return _action; }
            set { _action = value; }
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
        /// UploadTime
        /// </summary>
        public DateTime UploadTime
        {
            get { return this._uploadTime; }
            set { this._uploadTime = value; }
        }

        #endregion

        public UserAction(): base()
        {
            init();
        }

        public UserAction(int? Id)
        {
            init();

            DataTable dt = null;
            dt = base.GetDataById(Id);

            if (dt.Rows.Count > 0)
            {
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());

                this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                this._action = int.Parse(dt.Rows[0]["action"].ToString());

                string modifyTime = dt.Rows[0]["modifyTime"].ToString();
                this._modifyTime = DateTime.Parse(modifyTime);

                string uploadTime = dt.Rows[0]["uploadTime"].ToString();
                this._uploadTime = DateTime.Parse(uploadTime);

            }


        }

        public UserAction(int uId,int action)
        {
            init();

            DataTable dt = new DataTable();
            int number = 1;
            string strSql = String.Empty;
            strSql += " Select top " + number + " * from " + this._table + " as t where 1=1 ";
            strSql += " and t.uId = @uId  ";
            strSql += " and t.action  = @action  ";
            strSql += " order by t.modifyTime desc,t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uId", uId),
                new SqlParameter("@action", action),
			};

            dt= base.GetDataList(strSql, para);

            int count = dt.Rows.Count;
            if (count > 0)
            {
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());

                this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                this._action = int.Parse(dt.Rows[0]["action"].ToString());

                string modifyTime = dt.Rows[0]["modifyTime"].ToString();
                this._modifyTime = DateTime.Parse(modifyTime);

                string uploadTime = dt.Rows[0]["uploadTime"].ToString();
                this._uploadTime = DateTime.Parse(uploadTime);

            }


        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[userAction]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = " uId,action,modifyTime,uploadTime ";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uId", _uId),
                new SqlParameter("@action", _action),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};
            return base.Add(value, para);
        }

        public int ModifyInfo()
        {
            string set = " action=@action,modifyTime=@modifyTime,uploadTime=@uploadTime ";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@action", _action),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),
			};
            return base.Modify(set, para);
        }

        public DataTable GetActionList(int number)
        {
            if (number < 1)
            {
                number = 10;
            }

            string strSql = " Select top " + number + " * from " + this._table + " as t where 1=1 ";


            if (_Id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_action != 0)
            {
                strSql += " and t.action  = @action  ";
            }

            strSql += " order by t.modifyTime desc,t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@action", _action),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }

        public int GetActionListCount()
        {
            string strSql = " Select count(*) as amount from " + this._table + " as t where 1=1 ";


            if (_Id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_action != 0)
            {
                strSql += " and t.action  = @action  ";
            }

            //strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@action", _action),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};
            int total = 0;

            DataTable dt = new DataTable();

            int count = 0;
            try
            {
                dt = base.GetDataList(strSql, para);
                count = dt.Rows.Count;
            }
            catch { }

            if (count > 0)
            {
                total = int.Parse(dt.Rows[0]["amount"].ToString());
            }
            return total;
        }

        public DataTable GetActionUserList(int number)
        {
            if (number < 1)
            {
                number = 10;
            }

            string strSql = "";
            strSql += " Select top " + number + " table1.*,table2.phone,table2.email,table2.realName,table2.nickName,table2.userFace,table2.identityCard,table2.birthDay,table2.address,table2.registerTime,table2.userExp,table2.status,table2.interests from " + this._table + " as table1 ";
            strSql += " left join [user] as table2 on table1.uId = table2.Id ";
            strSql += " where 1=1 and table2.status > @status ";

            if (_Id != 0)
            {
                strSql += " and table1.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and table1.uId = @uId  ";
            }

            if (_action != 0)
            {
                strSql += " and table1.action  = @action  ";
            }

            strSql += " order by table1.modifyTime desc,table1.Id desc ";
            int status = 0;
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@action", _action),
                new SqlParameter("@status", status),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }

        public DataTable GetActionUserList(int number,double hours)
        {
            if (number < 1)
            {
                number = 10;
            }

            string strSql = "";
            strSql += " Select top " + number + " table1.*,table2.phone,table2.email,table2.realName,table2.nickName,table2.userFace,table2.identityCard,table2.birthDay,table2.address,table2.registerTime,table2.userExp,table2.status,table2.interests from " + this._table + " as table1 ";
            strSql += " left join [user] as table2 on table1.uId = table2.Id ";
            strSql += " where 1=1 and table2.status > @status ";

            if (_Id != 0)
            {
                strSql += " and table1.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and table1.uId = @uId  ";
            }

            if (_action != 0)
            {
                strSql += " and table1.action  = @action  ";
            }

            strSql += " and table1.modifyTime  > @modifyTime  ";

            string t_modifyTime = this._modifyTime.AddHours(hours).ToString();

            strSql += " order by table1.modifyTime desc,table1.Id desc ";

            int status = 0;
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@action", _action),
                new SqlParameter("@status", status),
                new SqlParameter("@modifyTime", t_modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }

    }
}