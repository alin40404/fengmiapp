using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserGroupUser:CommonModel
    {
        #region 参数

        private int _id = 0;
        private int _uId = 0;
        //群Id
        private int _uGId = 0;

        //用户权限，管理员，普通群成员
        private int _uRole = 0;


        private DateTime _modifyTime = DateTime.Now;
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


        ///<summary>
        /// URole, 用户权限，管理员，普通群成员
        ///</summary>
        public int URole
        {
            get { return _uRole; }
            set { _uRole = value; }
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

        public UserGroupUser(): base()
        {
            init();
        }

        public UserGroupUser(int uId, int uGId)
        {
            init();

            string strSql = " Select table1.* from " + this._table + " as table1  where 1=1 and table1.uId = @uId and table1.uGId = @uGId  ";

            DataTable dt = null;

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("uId", uId),
				new SqlParameter("uGId", uGId),
			};
            dt = base.GetDataList(strSql,para);

            if (dt.Rows.Count > 0)
            {
                this._id = int.Parse(dt.Rows[0]["Id"].ToString());
                this._uId = uId;
                this._uGId = uGId;

                this._uRole = int.Parse(dt.Rows[0]["uRole"].ToString());

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
            string table = "[userGroup_user]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "uId,uGId,uRole,status,modifyTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@uGId", _uGId),
                new SqlParameter("@uRole", _uRole),
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
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        public int Modify_uRole()
        {
            string set = "uRole=@uRole";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uRole", _uRole),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        public DataTable GetUserGroupWithUid()
        {
            int uId = this._uId;
            string strSql = string.Empty;
            int status = 0;
            strSql += " Select table1.*,table1.name,table1.gType,table1.createUId  from " + this._table + " as table1 ";
            strSql += " left join [userGroup] as table2 on table1.uGId = table2.Id ";

            strSql += " where 1=1 and table1.uId = '" + uId + "' and table2.status > " + status;
            strSql += " order by table1.Id desc ";

            DataTable dt = new DataTable();

            dt = this.GetDataList(strSql);

            return dt;

        }

        public DataTable GetUserGroupWithUGid()
        {
            int uGId = this._uGId;
            string strSql = string.Empty;
            int status = 0;
            //strSql += " Select table1.* from " + this._table + " as table1 ";
            strSql += " Select table1.*,table2.phone,table2.email,table2.realName,table2.nickName,table2.userFace,table2.identityCard,table2.birthDay,table2.address,table2.registerTime,table2.userExp,table2.status,table2.interests from " + this._table + " as table1 ";
            strSql += " left join [user] as table2 on table1.uId = table2.Id ";

            strSql += " where 1=1 and table1.uGId = '" + uGId + "' and table1.status > " + status;
            strSql += " order by table1.Id desc ";

            DataTable dt = new DataTable();

            dt = this.GetDataList(strSql);

            return dt;

        }
    }
}