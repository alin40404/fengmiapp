using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserGroup:CommonModel
    {
        #region 参数

        private int _Id = 0;
        private string _name =string.Empty;

        private int _createUId = 0;
        //群类型，工作群，好友群等
        private int _gType = 0;

        private DateTime _modifyTime = DateTime.Now;
        private int _status = 0;

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
        /// CreateUId
        ///</summary>
        public int CreateUId
        {
            get { return _createUId; }
            set { _createUId = value; }
        }

        ///<summary>
        /// Name
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        ///<summary>
        /// GType, 群类型，工作群，好友群等
        ///</summary>
        public int GType
        {
            get { return _gType; }
            set { _gType = value; }
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
        /// Status
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        public UserGroup(): base()
        {
            init();
        }

        public UserGroup(int? Id)
        {
            init();

            DataTable dt = null;
            dt = base.GetDataById(Id);

            if (dt.Rows.Count > 0)
            {
                this._Id = int.Parse(dt.Rows[0]["Id"].ToString());

                this._name = dt.Rows[0]["name"].ToString(); ;

                this._gType = int.Parse(dt.Rows[0]["gType"].ToString()); ;

                this._createUId = int.Parse(dt.Rows[0]["createUId"].ToString());


                string modifyTime = dt.Rows[0]["modifyTime"].ToString();
                this._modifyTime = DateTime.Parse(modifyTime);

                //status
                this._status = int.Parse(dt.Rows[0]["status"].ToString());

            }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[userGroup]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "name,gType,createUId,status,modifyTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@createUId", _createUId),
                new SqlParameter("@name", _name),
                new SqlParameter("@gType", _gType),
                new SqlParameter("@status", _status),
                new SqlParameter("@modifyTime", _modifyTime),
             
            };
            return base.Add(value,para);
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
            string set = "name=@name";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@name", _name),
                new SqlParameter("@Id", _Id),
			};
            return base.Modify(set, para);
        }

        public DataTable GetUserGroupList(int number)
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
            if (_createUId != 0)
            {
                strSql += " and t.createUId = @createUId  ";
            }

            if (_name != String.Empty)
            {
                strSql += " and t.name like '%'+ @name+'%'  ";
            }

            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@createUId", _createUId),
                new SqlParameter("@name", _name),
                new SqlParameter("@gType", _gType),
			};

            return base.GetDataList(strSql, para);

        }

        public int GetUserGroupId()
        {
            int number = 1;
            string strSql = " Select top " + number + " * from " + this._table + " as t where 1=1 ";

            if (_Id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_createUId != 0)
            {
                strSql += " and t.createUId = @createUId  ";
            }

            if (_name != String.Empty)
            {
                strSql += " and t.name = @name  ";
            }

            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _Id),
                new SqlParameter("@createUId", _createUId),
                new SqlParameter("@name", _name),
                new SqlParameter("@gType", _gType),
			};
            DataTable dt = new DataTable();
            dt = base.GetDataList(strSql, para);
            int count=dt.Rows.Count;
            if (count>0)
            {
                int i = 0;
                string IdStr = dt.Rows[i]["Id"].ToString();
                this._Id = int.Parse(IdStr);
            }
            return this._Id;
        }
    }
}