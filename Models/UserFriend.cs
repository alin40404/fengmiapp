using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserFriend:CommonModel
    {
        #region 参数

        private int _id = 0;
        private int _uId = 0;
        //关联的好友UId
        private int _fuId = 0;
        //添加方式
        private int _addType = 0;
        //所属好友组
        private int _uFGroupId = 0;

        private DateTime _modifyTime = DateTime.Now;
        private int _status = 1;

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

        ///<summary>
        /// UfId ,好友表 Id
        ///</summary>
        public int AddType
        {
            get { return _addType; }
            set { _addType = value; }
        }

        ///<summary>
        /// UFGroupId ,所属好友分组
        ///</summary>
        public int UFGroupId
        {
            get { return _uFGroupId; }
            set { _uFGroupId = value; }
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

        public UserFriend(): base()
        {
            init();
        }

        public UserFriend(int uId, int fuId)
        {
            init();

            string strSql = " Select t.* from " + this._table + " as t  where 1=1 and t.uId = @uId and t.fuId = @fuId  ";

            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@uId", uId),
				new SqlParameter("@fuId", fuId),
			};
            dt = base.GetDataList(strSql,para);

            if (dt.Rows.Count > 0)
            {
                this._id = int.Parse(dt.Rows[0]["Id"].ToString());
                this._uId = uId;
                this._fuId = fuId;

                this._uFGroupId = int.Parse(dt.Rows[0]["uFGroupId"].ToString());

                int addType = 0;
                try
                {
                    addType = int.Parse(dt.Rows[0]["addType"].ToString());
                }
                catch
                {
                    addType = 0;
                }
                this._addType = addType;

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
            string table = "[userFriend]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "uId,fuId,addType,uFGroupId,status,modifyTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@uId", _uId),
                new SqlParameter("@fuId", _fuId),
                new SqlParameter("@addType", _addType),
                new SqlParameter("@uFGroupId", _uFGroupId),
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

        public int ModifyStatus(int uId, int uFGroupId, int status)
        {
            string set = "status=@status";
            string where = "uId=@uId and uFGroupId=@uFGroupId";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@status", status),
                new SqlParameter("@uFGroupId", uFGroupId),
                new SqlParameter("@uId", uId),
			};
            return base.Modify(set,where, para);
        }

        /// <summary>
        /// 互改多位好友状态
        /// </summary>
        /// <param name="fuIdList"></param>
        /// <returns></returns>
        public int ModifyStatus(string fuIdList)
        {
            string set = "status=@status";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@status", _status),
				new SqlParameter("@uId", _uId),
                //new SqlParameter("@fuIdList", fuIdList),
			};

            string sql = "UPDATE " + this._table + " set " + set +
        "  Where uId=@uId and fuId in ( " + fuIdList + " ) ";

            string sql_deleted = "UPDATE " + this._table + " set " + set +
        "  Where fuId=@uId and uId in ( " + fuIdList + " ) ";
            int result = 0;

            sql += " ; " + sql_deleted + " ; ";
            result = this.ExecuteNonQuery(CommandType.Text, sql, para);

            /*
            if (result > 0)
            {
                this.ExecuteNonQuery(CommandType.Text, sql_deleted, para);
            }
             */

            return result;
        }

        /// <summary>
        /// 删除分组时，修改好友里面的分组Id=0
        /// </summary>
        /// <returns></returns>
        public int DeleteUFGroupUsers()
        {
            int result = 0;
            int newUFGroupId = 0;
            this._uFGroupId = 0;

            string set = "uFGroupId=@newUFGroupId";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uFGroupId", _uFGroupId),
                new SqlParameter("@newUFGroupId", newUFGroupId),
			};

            string sql = "UPDATE " + this._table + " set " + set +
"  Where uFGroupId=@uFGroupId ; ";

            result = this.ExecuteNonQuery(CommandType.Text, sql, para);

            return result;

        }
        
        /// <summary>
        /// 把多位好友移除 好友分组
        /// </summary>
        /// <param name="fuIdList"></param>
        /// <returns></returns>
        public int DeleteUFGroupUsers(string fuIdList)
        {
            int result = 0;
            this._uFGroupId = 0;
            result=this.Modify_uFGroupId(fuIdList);
            return result;

        }

        /// <summary>
        /// 修改 多位好友所在的分组
        /// </summary>
        /// <param name="fuIdList">好友Id，','分隔</param>
        /// <returns></returns>
        public int Modify_uFGroupId(string fuIdList)
        {
            int result = 0;
            string set = "uFGroupId=@uFGroupId";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uFGroupId", _uFGroupId),
				new SqlParameter("@uId", _uId),
                //new SqlParameter("@fuIdList", fuIdList),
			};

            string sql = "UPDATE " + this._table + " set " + set +
        "  Where uId=@uId and fuId in ( " + fuIdList + " ) ";

            sql += " ; ";
            result = this.ExecuteNonQuery(CommandType.Text, sql, para);

            return result;

        }

        /// <summary>
        /// 修改 一个好友所在的分组
        /// </summary>
        /// <returns></returns>
        public int Modify_uFGroupId()
        {
            string set = "uFGroupId=@uFGroupId";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uFGroupId", _uFGroupId),
                new SqlParameter("@Id", _id),
			};
            return base.Modify(set, para);
        }

        /// <summary>
        /// 获取分组好友基本信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserFriends()
        {
            string strSql=string.Empty;
            int status = 0;
            strSql += " Select table1.*,table2.phone,table2.email,table2.realName,table2.nickName,table2.userFace,table2.identityCard,table2.birthDay,table2.address,table2.registerTime,table2.userExp,table2.status as userStatus,table2.interests from " + this._table + " as table1 ";
            strSql += " left join [user] as table2 on table1.fuId = table2.Id ";
            //strSql += " where 1=1 and table1.uId = @uId and table1.status > @status  ";
            strSql += " where 1=1 and table1.uId = @uId and table1.status > @status and table2.status > @status ";
            strSql += " order by table1.Id desc ";

            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@uId", this._uId),
                new SqlParameter("@status", status),

			};
            dt = base.GetDataList(strSql, para);

            //dt = this.GetDataList(strSql);

            return dt;

        }

        /// <summary>
        /// 获取隐身好友Id 列表
        /// </summary>
        /// <returns></returns>
        public string GetUserFriendsIdList(bool isGetAll)
        {
            int status = 2;
            string strSql = " Select t.* from " + this._table + " as t  where 1=1 and t.uId = @uId  ";
            if (isGetAll==false)
            {
                strSql += " and t.status = @status  ";
            }
            DataTable dt = new DataTable();

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@uId", this._uId),
                new SqlParameter("@status", status),

			};
            dt = base.GetDataList(strSql, para);
            string ufIdList = string.Empty;
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow dr = dt.Rows[i];
                string fuId = dr["fuId"].ToString();
                ufIdList += fuId+",";
            }
            //ufIdList = ufIdList.Trim(',');

            return ufIdList;

        }

    }
}