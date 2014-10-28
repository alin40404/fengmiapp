using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace fengmiapp.Models
{
    public class User
    {
        #region 参数
        private string _table = "[user]";

        private int _id = 0;
        private string _phone = String.Empty;
        private string _email = String.Empty;
        private string _password = String.Empty;

        private string _realName = String.Empty;
        private string _nickName = String.Empty;

        private string _userFace = String.Empty;
        private string _identityCard = String.Empty;
        private DateTime _birthDay = DateTime.Now;

        private string _address = String.Empty;
        private DateTime _registerTime = DateTime.Now;
        private float _userExp = 0;
        private int _status = 1;
        private int _isPermitAddFriend = 1;//默认允许被添加好友

        private string _interests = String.Empty;

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
        /// Email
        ///</summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone
        {
            get { return this._phone; }
            set { this._phone = value; }
        }

        ///<summary>
        /// PassWord
        ///</summary>
        public string PassWord
        {
            get { return _password; }
            set { _password = value; }
        }
        ///<summary>
        /// NickName
        ///</summary>
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }
        ///<summary>
        /// NickName
        ///</summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        /// <summary>
        /// UserFace
        /// </summary>
        public string UserFace
        {
            get { return this._userFace; }
            set { this._userFace = value; }
        }
        /// <summary>
        /// IdentityCard
        /// </summary>
        public string IdentityCard
        {
            get { return this._identityCard; }
            set { this._identityCard = value; }
        }
        /// <summary>
        /// Address
        /// </summary>
        public string Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        ///<summary>
        /// UserExp
        ///</summary>
        public float UserExp
        {
            get { return _userExp; }
            set { _userExp = value; }
        }

        /// <summary>
        /// BirthDay
        /// </summary>
        public DateTime BirthDay
        {
            get { return this._birthDay; }
            set { this._birthDay = value; }
        }
        /// <summary>
        /// RegisterTime
        /// </summary>
        public DateTime RegisterTime
        {
            get { return this._registerTime; }
            set { this._registerTime = value; }
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
        /// Interests
        /// </summary>
        public string Interests
        {
            get { return this._interests; }
            set { this._interests = value; }
        }

        /// <summary>
        /// IsPermitAddFriend
        /// </summary>
        public int IsPermitAddFriend
        {
            get { return _isPermitAddFriend; }
            set { _isPermitAddFriend = value; }
        }

        #endregion

        public User()
        {
        }
        
        public User(int tempId)
        {
            if (tempId >0)
            {
                this._id = tempId;

                string strSql = " Select admin.* from " + this._table + " as admin  where 1=1 and admin.Id = @Id order by admin.Id asc ";

                DataTable dt = new DataTable();

                SqlParameter[] para = new SqlParameter[]
			    {
				    new SqlParameter("@Id", _id),
			    };
                dt = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    this.SetField(dt);
                }
            }
        }

        public User(string phone)
        {
            if ( !string.IsNullOrEmpty(phone))
            {

                string strSql = " Select admin.* from " + this._table + " as admin  where 1=1 and admin.phone = @phone ";

                DataTable dt = new DataTable();

                SqlParameter[] para = new SqlParameter[]
			    {
				    new SqlParameter("@phone", phone),
			    };

                dt = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    this.SetField(dt);
                }
            }
        }

        protected void SetField(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._id = int.Parse(dt.Rows[0]["Id"].ToString());
                    this._email = dt.Rows[0]["email"].ToString();
                    this._phone = dt.Rows[0]["phone"].ToString();
                    this._password = dt.Rows[0]["password"].ToString();

                    this._realName = dt.Rows[0]["realName"].ToString();
                    this._nickName = dt.Rows[0]["nickName"].ToString();

                    this._identityCard = dt.Rows[0]["identityCard"].ToString();

                    this._userFace = dt.Rows[0]["userFace"].ToString();
                    //this._userFace = Encoding.UTF8.GetString((byte[])dt.Rows[0]["userFace"]);

                    this._address = dt.Rows[0]["address"].ToString();
                    this._interests = dt.Rows[0]["interests"].ToString();

                    float userExp = 0;
                    try
                    {
                        userExp = float.Parse(dt.Rows[0]["userExp"].ToString());
                    }
                    catch
                    {
                        userExp = 0;
                    }
                    this._userExp = userExp;
                    string birthDay = dt.Rows[0]["birthDay"].ToString();
                    if (string.IsNullOrEmpty(birthDay))
                    {
                        this._birthDay = DateTime.Now;
                    }
                    else
                    {
                        this._birthDay = DateTime.Parse(birthDay);
                    }

                    string registerTime = dt.Rows[0]["registerTime"].ToString();
                    this._registerTime = DateTime.Parse(registerTime);

                    //status
                    this._status = int.Parse(dt.Rows[0]["status"].ToString());
                    this._isPermitAddFriend = int.Parse(dt.Rows[0]["isPermitAddFriend"].ToString());

                }
            }
            catch { }

        }

        public int Add()
        {
            string sql =
            "INSERT INTO " + this._table + "(password,phone,email,realName,nickName,userFace,identityCard,birthDay,address,registerTime,userExp,status,interests,isPermitAddFriend) " +
            "VALUES (@password,@phone,@email,@realName,@nickName,@userFace,@identityCard,@birthDay,@address,@registerTime,@userExp,@status,@interests,@isPermitAddFriend)";

            //byte[] userFace = Encoding.UTF8.GetBytes(_userFace);
           

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@password", _password),
                new SqlParameter("@phone", _phone),
                new SqlParameter("@email", _email),

                new SqlParameter("@realName", _realName),
                new SqlParameter("@nickName", _nickName),
                new SqlParameter("@userFace", _userFace),
                new SqlParameter("@identityCard", _identityCard),
                new SqlParameter("@birthDay", _birthDay),
                new SqlParameter("@address", _address),
                new SqlParameter("@registerTime", _registerTime),
                new SqlParameter("@userExp", _userExp),
                new SqlParameter("@status", _status),
                new SqlParameter("@interests",_interests),
                new SqlParameter("@isPermitAddFriend", _isPermitAddFriend),
             
            };
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        #region 修改操作
        public int ModifyPWD()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set " +
                    "password=@password " +
                    "Where Id=@Id";

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", _id),
				new SqlParameter("@password", _password)
             
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        public int Modify()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set " +
                    "email=@email, " +
                    "realName=@realName, " +
                    "nickName=@nickName " +
                    "Where Id=@Id";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@email", _email),
                new SqlParameter("@realName", _realName),
                new SqlParameter("@nickName", _nickName),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        public int ModifyStatus()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set " +
                    " status=@status " +
                    " Where Id=@Id  ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@status", _status),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        public int ModifyPermit()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set " +
                    " isPermitAddFriend=@isPermitAddFriend " +
                    " Where Id=@Id  ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@isPermitAddFriend", _isPermitAddFriend),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        public int ModifyInfo()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set ";

            //"realName=@realName," +
            //"nickName=@nickName, " +
            //"birthDay=@birthDay, " +
            //"email=@email, " +
            //"userFace=@userFace, " +
            //"address=@address, " +
            //"identityCard=@identityCard " +
            //"Where Id=@Id";

            string set = "";
            if (!string.IsNullOrEmpty(this._realName))
            {
                set += "realName=@realName,";
            }
            if (!string.IsNullOrEmpty(this._nickName))
            {
                set += "nickName=@nickName,";
            }
            if (!string.IsNullOrEmpty(this._birthDay.ToString()))
            {
                set += "birthDay=@birthDay,";
            }

            if (!string.IsNullOrEmpty(this._email))
            {
                set += "email=@email,";
            }
            if (!string.IsNullOrEmpty(this._userFace))
            {
                set += "userFace=@userFace,";
            }
            if (!string.IsNullOrEmpty(this._address))
            {
                set += "address=@address,";
            }
            if (!string.IsNullOrEmpty(this._identityCard))
            {
                set += "identityCard=@identityCard";
            }
            if (!string.IsNullOrEmpty(this._interests))
            {
                set += "interests=@interests";
            }

            
            if (string.IsNullOrEmpty(set))
            {
                return 0;
            }
            else
            {
                set = set.TrimEnd(',');
                set = "  " + set + "  ";
            }
            sql += set + " Where Id=@Id ";


            //byte[] userFace = Encoding.UTF8.GetBytes(_userFace);

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@realName", _realName),
                new SqlParameter("@nickName", _nickName),
                new SqlParameter("@email", _email),
                new SqlParameter("@userFace", _userFace),
                new SqlParameter("@birthDay", _birthDay),
                new SqlParameter("@address", _address),
                new SqlParameter("@identityCard", _identityCard),
                new SqlParameter("@interests", _interests),

			};


            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }
       
        public int ModifyLoginInfo()
        {
            string sql = "UPDATE " + "" + this._table + "" + " set " +
                    "loginTimes=@loginTimes, " +
                    "lastLoginIP=@lastLoginIP, " +
                    "lastLoginTime=@lastLoginTime " +
                    "Where Id=@Id";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@loginTimes", _userExp),
                new SqlParameter("@lastLoginIP", _address),
                new SqlParameter("@lastLoginTime", _birthDay),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }
        #endregion

        #region 删除操作
        public int Delete(int Id)
        {
            string sql = "delete " + this._table + " Where Id=@Id ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        public int Delete(string Id)
        {
            string sql = "delete " + this._table + " Where Id in (@Id) ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }
        #endregion

        #region 获取数据操作
        public DataTable SelectByCount()
        {
            string sql = "select * from " + this._table + " Where adminName=@adminName ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@adminName", this._nickName),
             
			};
            return SQLHelper.ExecuteToDataSet(CommandType.Text, sql, para).Tables[0];
        }
        
        public DataTable GetDataList()
        {
            //string strSql = " Select admin.* from " + this._table + " as admin where 1=1  ";
            string table = "[category]";
            string strSql = " Select admin.*,c.cateName as department from " + this._table + "as admin left join " + table + " as c on admin.gId=c.Id where 1=1  ";

            if (_id != 0)
            {
                strSql += " and admin.Id != @Id  ";
            }

            if (_realName != String.Empty)
            {
                strSql += " and admin.realName like '%'+ @realName+'%'  ";
            }
            if (_nickName != String.Empty)
            {
                strSql += " and admin.adminName like '%'+  @adminName+'%' ";
            }
            if (_email != String.Empty)
            {
                strSql += " and admin.email like '%'+  @email+'%' ";
            }


            strSql += " order by admin.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
							new SqlParameter("@Id", _id),
							new SqlParameter("@realName", _realName),
							new SqlParameter("@adminName", _nickName),
							new SqlParameter("@email", _email),
			};

            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;

        }

        public DataTable GetDataList(string Id)
        {
            string strSql = " Select * from " + this._table + " where 1=1 and Id in (" + Id + ") ";
            strSql += " order by Id desc ";


            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(strSql);

            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;

        }
        #endregion 
       
        public void login()
        {
            string strSql = "select top 1 * from " + this._table + " where ( phone=@phone and password=@password ) order by Id asc";
            SqlParameter[] para = new SqlParameter[]
			{
               // new SqlParameter("@Id", _id),
				new SqlParameter("@phone", _phone),
				new SqlParameter("@password", _password),
               // new SqlParameter("@email", _email),
            };
            DataSet dts = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            DataTable dt = new DataTable();
            //dt = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para).Tables[0];

            if (dts != null && dts.Tables.Count > 0)
            {
                dt = dts.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    this.SetField(dt);
                }
                else
                {
                    this._id = 0;
                }
            }
            else
            {
                this._id = 0;
            }
            //return dt;
        }
    }
}